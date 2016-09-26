using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class PhpClass
    {
        public string ClassCode { get; }

        public List<PhpFunction> Functions { get; }
        public string ClassMembers { get; private set; }
        public PhpFile ParentFile { get; private set; }
        public string ClassName { get; private set; }
        public PhpClass(string code, PhpFile parent, string name)
        {
            ClassCode = code;
            Functions = new List<PhpFunction>();
            ParentFile = parent;
            ClassName = name;
            ParseClass();
        }

        /// <summary>
        /// Searches class for object Type for a given class member variable
        /// </summary>
        /// <param name="classMemberName">Name of class member variable to identify. In form: varName (no $)</param>
        /// <returns></returns>
        public string FindTypeOfClassMemberVariable(string classMemberName)
        {
            string results = "";
            string insNew = classMemberName + "\\s*=\\s*new\\s+\\S+\\s*\\(.*\\)";
            string insFactory = classMemberName + "\\s*=\\s*\\S+::\\S+\\(.*\\)";
            Regex insNewRE = new Regex(insNew, RegexOptions.IgnoreCase);
            Regex insFactoryRE = new Regex(insFactory, RegexOptions.IgnoreCase);
            if(insNewRE.IsMatch(ClassCode))
            {
                Match iMatch = insNewRE.Match(ClassCode);
                string cName = ClassCode.Substring(iMatch.Index, iMatch.Length);
                cName = cName.Substring(cName.IndexOf("new ") + "new ".Length);
                cName = cName.Substring(0, cName.IndexOf('('));
                results = cName;
            }
            else if(insFactoryRE.IsMatch(ClassCode))
            {
                Match iMatch = insFactoryRE.Match(ClassCode);
                string cName = ClassCode.Substring(iMatch.Index, iMatch.Length);
                cName = cName.Substring(cName.IndexOf('=')+1, cName.IndexOf("::") - (cName.IndexOf('=') + 1));
                cName = cName.Trim();
                results = cName;
            }
            return results;
        }

        private void ParseClass()
        {
            string regexSearch = "^(\\s|public|private|protected|abstract|final|static)*\\s*function\\s+";
            string regexClassSearch = "^\\s*(abstract\\s|final\\s)?class\\s.*\\{?\\s*$";
            Regex phpClassRegex = new Regex(regexClassSearch, RegexOptions.IgnoreCase);
            Regex functionRE = new Regex(regexSearch, RegexOptions.IgnoreCase);
            bool currentlyInFunction = false; //flag to track if we are already in a function
            int totalBracketCount = 0;
            string currentFunctionName = "";
            using (StringReader sr = new StringReader(ClassCode))
            {
                string codeBlock = "";
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    //ignore first line (class declaration), but count its bracket(s)
                    if (phpClassRegex.IsMatch(line))
                    {
                        int openBCount = line.Count(x => x == '{');
                        int closeBCount = line.Count(x => x == '}');
                        totalBracketCount += openBCount;
                        totalBracketCount -= closeBCount;
                    }
                    else
                    {
                        if (currentlyInFunction)
                        {
                            //if inside of a function, find end of function and add to functions list
                            codeBlock += line + "\r\n";
                            int openBCount = line.Count(x => x == '{');
                            int closeBCount = line.Count(x => x == '}');
                            totalBracketCount += openBCount;
                            totalBracketCount -= closeBCount;
                            if (totalBracketCount <= 1) // the class brackets will keep it at 1 when outside of a function
                            {
                                //found end of function
                                currentlyInFunction = false;
                                PhpFunction foundFunction = new PhpFunction(codeBlock, this, currentFunctionName);
                                Functions.Add(foundFunction);
                                currentFunctionName = "";
                            }
                        }
                        else
                        {
                            //check for start of function
                            if (functionRE.IsMatch(line))
                            {
                                Match functionMatch = functionRE.Match(line);
                                int startFunctionName = functionMatch.Index + functionMatch.Length;
                                int endFunctionName = line.IndexOf('(');
                                currentFunctionName = line.Substring(startFunctionName, endFunctionName - startFunctionName);
                                codeBlock = line + "\r\n";
                                currentlyInFunction = true;
                            }
                            else
                            {
                                //if not in a function add lines to classMembers
                                ClassMembers += line + "\r\n";
                            }
                            int openBCount = line.Count(x => x == '{');
                            int closeBCount = line.Count(x => x == '}');
                            totalBracketCount += openBCount;
                            totalBracketCount -= closeBCount;
                        }
                    }
                }
            }
        }


        public List<PhpFunction> ScanForSQL(string tableName, bool limitToTable, string attributeName, bool limitToAttribute, bool includeSelect, bool includeInsert, bool includeUpdate, bool includeDelete)
        {
            List<PhpFunction> foundFunctions = new List<PhpFunction>();

            foreach (PhpFunction ph in Functions)
            {
                if (ph.ScanForSQL(tableName, limitToTable, attributeName, limitToAttribute, includeSelect, includeInsert, includeUpdate, includeDelete))
                {
                    foundFunctions.Add(ph);
                }
            }
            return foundFunctions;
        }
    }
}
