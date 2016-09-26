using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PHPAnalyzer.JavascriptObjects
{
    class JavascriptFile : CodeFile
    {
       
        public List<JavascriptFunction> Functions = new List<JavascriptFunction>();

        public JavascriptFile(string fullFileText, string filePath, string fileName, PHPAppData parentTree)
        {
            FullFileText = fullFileText;
            FullFilePath = filePath;
            FileName = fileName;
            ParentTree = parentTree;
            ParseFile();
        }

        /// <summary>
        /// Parses full Javascript code to find functions and stores them.  Functions contained within defined functions will be included within the top level function and not as a separate function.
        /// </summary>
        private void ParseFile()
        {
            string functionStartRegex = "(^|[\\r\\n]{1})[ \\t]*\\S*[ \\t]*(function\\s*\\(|function\\s+\\w+\\s*\\()";
            Regex functionStartRE = new Regex(functionStartRegex, RegexOptions.IgnoreCase);

            //pull out functions
            MatchCollection functionStarts = functionStartRE.Matches(FullFileText);
            
            int lastFunctionEnd = -1;


            foreach(Match f in functionStarts)
            {
                if (f.Index>lastFunctionEnd)
                {
                    //find opening of function
                    int functionStartIndex = f.Index;
                    int functionCloseIndex = functionStartIndex;

                    //find closest end bracket and count brackets in between
                    int openBracketCount = 0;
                    int closeBracketCount = 0;

                    bool foundEnd = false;
                    int i = functionStartIndex;
                    while (!foundEnd && i < FullFileText.Length)
                    {
                        if(FullFileText[i] == '{')
                        {
                            openBracketCount++;
                        }
                        else if(FullFileText[i] == '}')
                        {
                            closeBracketCount++;
                            if(closeBracketCount==openBracketCount)
                            {
                                functionCloseIndex = i;
                                foundEnd = true;
                                lastFunctionEnd = i;
                            }     
                            else if (closeBracketCount > openBracketCount)
                            {
                                break;
                            }
                        }
                        i++;
                    }


                    if (foundEnd && functionCloseIndex > functionStartIndex)
                    {
                        //pull function from text
                        string functionText = FullFileText.Substring(functionStartIndex, functionCloseIndex - functionStartIndex + 1);

                        //get function name, if one exists (not all function's have names in Javascript)
                        string functionName = "";

                        string functionRegex = "function";
                        Regex functionRE = new Regex(functionRegex, RegexOptions.IgnoreCase);

                        Match functionMatch = functionRE.Match(functionText);

                        functionName = functionText.Substring(functionMatch.Index, functionMatch.Length);
                        functionName = functionName.Trim();
                        if(functionName.IndexOf(' ') > 0)
                        {
                            functionName = functionName.Substring(functionName.IndexOf(' '));
                            functionName = functionName.Trim();
                            int openIndex = functionName.IndexOf('(');
                            functionName = functionName.Substring(0, openIndex);
                            functionName = functionName.Trim();
                        }
                        else
                        {
                            functionName = "";
                        }
                        

                        JavascriptFunction function = new JavascriptFunction(functionText, functionName, functionStartIndex, functionCloseIndex);
                        Functions.Add(function);
                    }
                    else
                    {
                        //throw new Exception("No matching function closing bracket found");
                    } 
                }
            }
        }

        /// <summary>
        /// Get the function that matches the name parameter.  Functions that do not have names cannot be found this way.
        /// </summary>
        /// <param name="name">Name of function to get</param>
        /// <returns>Returns JavascriptFunction whose name matches Name parameter, null if not found.</returns>
        public JavascriptFunction GetFunction(string name)
        {
            JavascriptFunction results = null;
            foreach(JavascriptFunction j in Functions)
            {
                if(name!=null && name.ToLower() == j.Name.ToLower())
                {
                    results = j;
                    break;
                }
            }
            return results;
        }

        /// <summary>
        /// Determines if current JavascriptFile contains a function matching the name parameter
        /// </summary>
        /// <param name="name">Name of function to find</param>
        /// <returns>Returns true if function is found, false otherwise</returns>
        public bool HasFunction(string name)
        {
            return false;
        }
    }
}
