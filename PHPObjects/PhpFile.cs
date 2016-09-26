using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class PhpFile : CodeFile
    {
        public List<PhpClass> Classes { get; private set; } = new List<PhpClass>();
        public PhpSnippet Snippet { get; private set; } //snippet is all blocks of PHP code that are not contained within a class
     
        public PhpFile(string fileText, string filePath, string fileName, PHPAppData parentTree)
        {
            FullFileText = fileText;            
            FullFilePath = filePath;
            FileName = fileName;
            ParentTree = parentTree;
            ParseFile();
        }

        

        public List<PhpFunction> ScanForSQL(string tableName, bool limitToTable, string attributeName, bool limitToAttribute, bool includeSelect, bool includeInsert, bool includeUpdate, bool includeDelete)
        {
            List<PhpFunction> foundFunctions = new List<PhpFunction>();

            foreach (PhpClass pc in Classes)
            {
                List<PhpFunction> funcs = pc.ScanForSQL(tableName, limitToTable, attributeName, limitToAttribute, includeSelect, includeInsert, includeUpdate, includeDelete);

                foundFunctions.AddRange(funcs);
            }
            return foundFunctions;
        }
        
       

        /// <summary>
        /// This function just parses what is immediately in the file into classes and snippets.  Classes are then parsed within the PhpClass object constructor
        /// </summary>
        private void ParseFile()
        {
            
            int totalBracketCount = 0;            

            string regexClassSearch = "^(\\s*|abstract\\s|final\\s)class\\s.*\\{?\\s*$";
            string regexstartphp = "<\\?\\s*php";
            string regexendphp = "\\?\\s*>";            
            Regex phpStartRE = new Regex(regexstartphp, RegexOptions.IgnoreCase);
            Regex phpEndRE = new Regex(regexendphp, RegexOptions.IgnoreCase);

            Regex phpClassRegex = new Regex(regexClassSearch, RegexOptions.IgnoreCase);
            bool currentlyInClass = false; //flag to track if we are already in a class
            bool currentlyInPHP = false; //flag to track if we are already in a php block
            string snippetText = "";

            //TODO: update this to parse through a complete string of code, rather than line by line.
            using (StringReader sr = new StringReader(FullFileText))
            {
                //get full code for updated approach and parsing forms
              
                string codeBlock = "";
                string line = "";
                string currentClassName = "";
                while((line = sr.ReadLine()) != null)
                {
                    //scan until we enter PHP
                    if(phpStartRE.IsMatch(line))
                    {
                        Match phpStartMatch = phpStartRE.Match(line);
                        int start = phpStartMatch.Index;
                        if (currentlyInPHP)
                        {
                            //TODO:  handle error of finding a PHP codeblock start when we (think we) are already in a PHP block
                        }
                        else if (phpEndRE.IsMatch(line)) //cover case where both the start and end of php block are on same line
                        {
                            //for now we assume that an entire class connot be contained within one of these single line blocks
                            Match phpEndMatch = phpEndRE.Match(line);
                            int end = phpEndMatch.Index + phpEndMatch.Length;
                            string phpSubstring = line.Substring(start, end - start);
                            snippetText += phpSubstring + "\r\n";
                        }
                        else
                        {
                           codeBlock = line.Substring(start) + "\r\n"; //override old codeblock with our new start
                           currentlyInPHP = true;
                        }
                    }                    
                    else if (currentlyInPHP) //while in PHP
                    {
                        if(!currentlyInClass)
                        {
                            if (phpClassRegex.IsMatch(line))
                            {
                                //identify starts of classes and store held snippet
                                snippetText += codeBlock + "\r\n";
                                codeBlock = line;
                                currentlyInClass = true;
                                //count brackets in line after class opening
                                Match classMatch = phpClassRegex.Match(line);
                                int classStart = classMatch.Index;
                                int classNameStart = line.IndexOf("class")+"class".Length+1;
                                string classSub = line.Substring(classNameStart);
                                int classNameLength = classSub.IndexOf(' ');
                                if (classNameLength == -1)
                                {
                                    classNameLength = classSub.IndexOf('{');
                                    if(classNameLength == -1)
                                        classNameLength = classSub.Length - 1;
                                }
                                    
                                       
                                    
                                currentClassName = line.Substring(classNameStart, classNameLength);
                                string classOnlyString = line.Substring(classStart);
                                int openCount = classOnlyString.Count(x => x == '{');
                                int closeCount = classOnlyString.Count(x => x == '}');
                                totalBracketCount += openCount;
                                totalBracketCount -= closeCount;
                            }
                            else
                            {
                                //keep out-of-class code snippets
                                codeBlock += line + "\r\n";
                            }
                        }
                        else
                        {
                            //find end of class and store entire class into php class objects
                            codeBlock += line + "\r\n";
                            int openCount = line.Count(x => x == '{');
                            int closeCount = line.Count(x => x == '}');
                            totalBracketCount += openCount;
                            totalBracketCount -= closeCount;

                            if (totalBracketCount<=0) //we've found end of class in this line
                            {
                                PhpClass foundClass = new PhpClass(codeBlock, this, currentClassName);
                                Classes.Add(foundClass);
                                //we assume a new class cannot open on the same line a previous class closed
                                totalBracketCount = 0;
                                currentlyInClass = false;
                                currentClassName = "";
                                codeBlock = "";
                            }
                        }

                        //continue scanning code snipppets until we leave PHP
                        if (phpEndRE.IsMatch(line))
                        {
                            //store remaning code as snippet with ending                            
                            snippetText += codeBlock + "\r\n";
                            codeBlock = "";
                            currentlyInPHP = false;
                        }
                    }
                }
                Snippet = new PhpSnippet(snippetText, this);
            }
        }
    }
}
