using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class PhpFunction : PhpCodeObject
    {

        public string FunctionName { get; private set; }

        public PhpClass ParentClass { get; private set; }

                
        public PhpFunction(string function, PhpClass parent, string name) : base(function)
        {
            ParentClass = parent;
            FunctionName = name;
        }
        
        public override string FindTypeOfVariable(string classMemberName)
        {
            return ParentClass.FindTypeOfClassMemberVariable(classMemberName);
        }

        public override bool MakesCallToCallee(PhpFunction callee)
        {
            bool result = false;
            string calleeNameRegex = "(::|->)?" + callee.FunctionName + "\\s*\\(?";
            string calleeName = callee.FunctionName;
            string calleeClassName = callee.ParentClass.ClassName;
            string newInstanceOfClass = "\\$.*\\s*=\\s*new\\s*" + calleeClassName + "\\s*\\(";
            string sameClassCall = "\\$this->\\s?" + callee.FunctionName + "\\s?\\(";
            string staticFunctionCall = calleeClassName + "::" + callee.FunctionName + "\\s*\\(";
            Regex calleeNameRE = new Regex(calleeNameRegex, RegexOptions.IgnoreCase);
            Regex newInstanceOfClassRE = new Regex(newInstanceOfClass, RegexOptions.IgnoreCase);
            Regex sameClassCallRE = new Regex(sameClassCall, RegexOptions.IgnoreCase);
            Regex staticFunctionCallRE = new Regex(staticFunctionCall, RegexOptions.IgnoreCase);
            //check if we are in the same class as callee, if so check for calls of format $this->calleeName()
            if(this.ParentClass.ClassName.Equals(calleeClassName))
            {
                if(sameClassCallRE.IsMatch(this.PhpCode))
                {
                    //if we are in the same class and have a call of this format we can be certain we are calling the callee
                    result = true;
                }
            }
            else if (calleeNameRE.IsMatch(this.PhpCode))
            {
                //if matched that means there is a call to that function name
                //to be certain of a match we need to verify that it's the correct class and not just an identically named function
                List<string> classInstances = new List<string>();
                Regex spaceEqualsRE = new Regex("\\s?=", RegexOptions.IgnoreCase);
                using (StringReader sr = new StringReader(this.PhpCode))
                {
                    string line = "";
                    while((line = sr.ReadLine()) != null)
                    {
                        if(newInstanceOfClassRE.IsMatch(line))
                        {
                            //found an instantiation of the callee's class
                            //get instance name
                            string nameSubstring = line.Substring(line.IndexOf('$'));
                            if(spaceEqualsRE.IsMatch(nameSubstring))
                            {
                                Match instanceMatch = spaceEqualsRE.Match(nameSubstring);
                                string instancename = nameSubstring.Substring(0, instanceMatch.Index);
                                //add instance name to list
                                classInstances.Add(instancename);
                            }
                            else
                            {
                                //something's wrong..
                            }
                            
                        
                        }
                    }
                }
                //if there are any instances, check if any of them made a call to our callee function
                if(classInstances.Count>0)
                {
                    foreach(string s in classInstances)
                    {
                        Regex instanceCallToCallee = new Regex("\\" + s + "->" + calleeName + "\\s?\\(", RegexOptions.IgnoreCase);
                        if(instanceCallToCallee.IsMatch(this.PhpCode))
                        {
                            result = true;
                        }
                    }
                }
                else if (staticFunctionCallRE.IsMatch(this.PhpCode))
                {
                    result = true;
                }
            }
            return result;
        }

       
        

        public bool ScanForSQL(string tableName, bool limitToTable, string attributeName, bool limitToAttribute, bool includeSelect, bool includeInsert, bool includeUpdate, bool includeDelete)
        {
            string table = tableName;
            string attribute = attributeName;

            string regexinserttable = "[\"\'\\s]+INSERT(\\s+|\\s+.*\\s+)INTO\\s+" + table + "[\\s(,\']";
            string regexupdatetable = "[\"\'\\s]+UPDATE\\s" + table + "[\\s(,\']";
            string regexDeleteFromTable = "[\"\'\\s]DELETE(\\s+|\\s+.*\\s+)FROM\\s+" + table + "[\\s(,\']";
            string regexSelect = "[\"\'\\s]SELECT\\s";
            string regexfrom = "[\"\'\\s]FROM\\s";
            string regextable = "\\s" + table + "\\s";
            string regexAttribute = attribute + "[\\s,]";
            string selectString = "SELECT";
            string fromString = "FROM";
            string setString = "SET";
            string whereString = "WHERE";
            string regexAsterisk = "\\*\\s";

            Regex sqlInsertTable = new Regex(regexinserttable, RegexOptions.IgnoreCase);
            Regex sqlUpdateTable = new Regex(regexupdatetable, RegexOptions.IgnoreCase);
            Regex sqlDeleteFromTable = new Regex(regexDeleteFromTable, RegexOptions.IgnoreCase);
            Regex sqlFrom = new Regex(regexfrom, RegexOptions.IgnoreCase);
            Regex sqlTable = new Regex(regextable, RegexOptions.IgnoreCase);
            Regex sqlAttribute = new Regex(regexAttribute, RegexOptions.IgnoreCase);
            Regex sqlSelect = new Regex(regexSelect, RegexOptions.IgnoreCase);
            Regex sqlAsterisk = new Regex(regexAsterisk, RegexOptions.IgnoreCase);


            bool foundMatch = false;

            //parse function for just SQL
            if (includeUpdate && sqlUpdateTable.IsMatch(PhpCode))
            {
                if (limitToAttribute)
                {
                    //get string between SET and WHERE and check for desired attribute name
                    int setIndex = 0;
                    int lastSetIndex = 0;
                    int whereIndex = 0;
                    bool endOfMatches = false;
                    while (!endOfMatches && !foundMatch)
                    {
                        setIndex = PhpCode.IndexOf(setString, lastSetIndex + 1, StringComparison.CurrentCultureIgnoreCase); //we should be able to know there is at least one select based on the regex
                        if (setIndex == -1)
                        {
                            endOfMatches = true;
                            break;
                        }
                        else
                        {
                        
                            whereIndex = PhpCode.IndexOf(whereString, setIndex, StringComparison.CurrentCultureIgnoreCase);
                            string setSubstring = "";
                            if (whereIndex == -1)
                            {
                                //use entire rest of string
                                setSubstring = PhpCode.Substring(setIndex);
                            }
                            else
                            {
                                //check from FROM to WHERE
                                setSubstring = PhpCode.Substring(setIndex, whereIndex - setIndex);
                            }

                            if (sqlAttribute.IsMatch(setSubstring))
                            {
                                //this means the attribute we are looking for is found between SET and WHERE
                                foundMatch = true;
                            }
                        
                        }
                        lastSetIndex = setIndex;
                    }
                }
                else
                {
                    foundMatch = true;
                }
            }

            if (includeInsert && sqlInsertTable.IsMatch(PhpCode))
            {
                //assumed that all inserts affect the attribute in question
                foundMatch = true;
            }

            if (includeDelete && sqlDeleteFromTable.IsMatch(PhpCode))
            {
                //assumed that all deletes affect the attribute in question
                foundMatch = true;
            }

            if (includeSelect && sqlSelect.IsMatch(PhpCode))
            {
                //pull all SQL out of function to analyze needs to be broken into individual queries.... but how, when they're conditionally combined?
                int selectIndex = 0;
                int lastSelectIndex = 0;
                int fromIndex = 0;
                int whereIndex = 0;
                bool endOfMatches = false;
                while (!endOfMatches && !foundMatch)
                {
                    selectIndex = PhpCode.IndexOf(selectString, lastSelectIndex + 1, StringComparison.CurrentCultureIgnoreCase); //we should be able to know there is at least one select based on the regex
                    if (selectIndex == -1)
                    {
                        endOfMatches = true;
                        break;
                    }
                    fromIndex = PhpCode.IndexOf(fromString, selectIndex, StringComparison.CurrentCultureIgnoreCase); // get index of first from after select
                    if (fromIndex == -1)
                    {
                        endOfMatches = true; //if there are no more FROM statements in the whole function, we won't be able to find a match
                        break;
                    }
                    else
                    {
                        whereIndex = PhpCode.IndexOf(whereString, fromIndex, StringComparison.CurrentCultureIgnoreCase);
                        string fromSubstring = "";
                        if (whereIndex == -1)
                        {
                            //use entire rest of string
                            fromSubstring = PhpCode.Substring(fromIndex);
                        }
                        else
                        {
                            //check from FROM to WHERE
                            fromSubstring = PhpCode.Substring(fromIndex, whereIndex - fromIndex);
                        }

                        if (sqlTable.IsMatch(fromSubstring))
                        {
                            //this means our select function uses the right table
                            //now we need to check for the attribute if requested
                            if(limitToAttribute)
                            {
                                string selectSubstring = PhpCode.Substring(selectIndex, fromIndex - selectIndex);
                                if (sqlAttribute.IsMatch(selectSubstring) || sqlAsterisk.IsMatch(selectSubstring))
                                {
                                    foundMatch = true;
                                }
                            }
                            else
                            {
                                foundMatch = true;
                            }
                            
                        }
                    }
                    lastSelectIndex = selectIndex;
                }
                //
            }
            return foundMatch;            
        }

       
    }
}
