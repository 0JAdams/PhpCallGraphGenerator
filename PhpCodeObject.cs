using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class PhpCodeObject
    {
        public string PhpCode { get; protected set; }
      
        public PhpCodeObject(string code)
        {
            PhpCode = code;
            
        }

        public virtual bool MakesCallToCallee(PhpFunction callee)
        {
            bool result = false;
            string calleeNameRegex = "(::|->)?" + callee.FunctionName + "\\s*\\(?";
            string calleeName = callee.FunctionName;
            string calleeClassName = callee.ParentClass.ClassName;
            string newInstanceOfClass = "\\$.*\\s*=\\s*new\\s*" + calleeClassName + "\\s*\\(";
            string sameClassCall = "\\$this->\\s?" + callee.FunctionName + "\\s?\\(";
            Regex calleeNameRE = new Regex(calleeNameRegex, RegexOptions.IgnoreCase);
            Regex newInstanceOfClassRE = new Regex(newInstanceOfClass, RegexOptions.IgnoreCase);
            Regex sameClassCallRE = new Regex(sameClassCall, RegexOptions.IgnoreCase);
            //check if we are in the same class as callee, if so check for calls of format $this->calleeName()

            if (calleeNameRE.IsMatch(this.PhpCode))
            {
                //if matched that means there is a call to that function name
                //to be certain of a match we need to verify that it's the correct class and not just an identically named function
                List<string> classInstances = new List<string>();
                Regex spaceEqualsRE = new Regex("\\s?=", RegexOptions.IgnoreCase);
                using (StringReader sr = new StringReader(this.PhpCode))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (newInstanceOfClassRE.IsMatch(line))
                        {
                            //found an instantiation of the callee's class
                            //get instance name
                            string nameSubstring = line.Substring(line.IndexOf('$'));
                            if (spaceEqualsRE.IsMatch(nameSubstring))
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
                if (classInstances.Count > 0)
                {
                    foreach (string s in classInstances)
                    {
                        Regex instanceCallToCallee = new Regex("\\" + s + "->" + calleeName + "\\(", RegexOptions.IgnoreCase);
                        if (instanceCallToCallee.IsMatch(this.PhpCode))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    
               

        private struct functionClassPair
        {
            public string functionName { get; set; }
            public string className { get; set; }
        }

        private struct variableFunctionPair
        {
            public string functionName { get; set; }
            public string variableName { get; set; }
        }

        /// <summary>
        /// Searches class for object Type for a given class member variable
        /// </summary>
        /// <param name="variableName">Name of class member variable to identify. In form: varName (no $)</param>
        /// <returns></returns>
        public virtual string FindTypeOfVariable(string variableName)
        {
            string results = "";
            string insNew = variableName + "\\s*=\\s*new\\s+\\S+\\s*\\(.*\\)";
            string insFactory = variableName + "\\s*=\\s*\\S+::\\S+\\(.*\\)";
            Regex insNewRE = new Regex(insNew, RegexOptions.IgnoreCase);
            Regex insFactoryRE = new Regex(insFactory, RegexOptions.IgnoreCase);
            if (insNewRE.IsMatch(PhpCode))
            {
                Match iMatch = insNewRE.Match(PhpCode);
                string cName = PhpCode.Substring(iMatch.Index, iMatch.Length);
                cName = cName.Substring(cName.IndexOf("new ") + "new ".Length);
                cName = cName.Substring(0, cName.IndexOf('('));
                results = cName;
            }
            else if (insFactoryRE.IsMatch(PhpCode))
            {
                Match iMatch = insFactoryRE.Match(PhpCode);
                string cName = PhpCode.Substring(iMatch.Index, iMatch.Length);
                cName = cName.Substring(cName.IndexOf('=') + 1, cName.IndexOf("::") - (cName.IndexOf('=') + 1));
                cName = cName.Trim();
                results = cName;
            }
            return results;
        }

        /// <summary>
        /// Gets a list of all php functions called to from this code object
        /// </summary>
        /// <param name="tree">PhpFileTree reference used to find functions</param>
        /// <returns>List of functions called to from this php code</returns>
        public List<PhpCodeObject> ListOfFunctionsCalledTo(PHPAppData tree)
        {
            //TODO: fix current limitation of using line based regex calls.  This can cause issues if a line of code is broken into two lines
            // This applies to a lot of these functions, even if not mentioned everywhere

            //TODO: fix issue where the variable doesn't directly make the call: var->sub->function();

            //this will scan the function code and find other functions called to and return those functions
            List<PhpCodeObject> calledFunctions = new List<PhpCodeObject>();
            //scan text and find objects that make function calls
            string regexFunctionCall = "\\$[^\\$\\s\\.\'\",\\{\\}\\)]+->[^\\$\\s\\.\'\",\\{\\}\\)]+\\(";
            Regex functionCallRE = new Regex(regexFunctionCall, RegexOptions.IgnoreCase);


            List<variableFunctionPair> variables = new List<variableFunctionPair>();  //hold our list of variables we find that make calls to functions

            List<functionClassPair> foundPairs = new List<functionClassPair>();
            using (StringReader sr = new StringReader(this.PhpCode))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    //find function calls
                    if (functionCallRE.IsMatch(line))
                    {
                        Match fmatch = functionCallRE.Match(line);
                        int vindex = fmatch.Index;
                        string vName = line.Substring(vindex, fmatch.Length);
                        string endLine = vName.Substring(vName.IndexOf("->") + "->".Length);
                        vName = vName.Substring(0, vName.IndexOf("->"));

                        //get the name of the function being called
                        string fName = line.Substring(vindex);
                        fName = fName.Substring(fName.IndexOf('>') + 1);
                        fName = fName.Substring(0, fName.IndexOf('('));


                        if (endLine.Contains("->"))
                        {
                            //this means that the vName isn't what's being called directly
                            string secondVariable = endLine.Substring(0, endLine.IndexOf("->"));
                            string classType = FindTypeOfVariable(secondVariable);
                            if (fName.Contains("->"))
                            {
                                fName = fName.Substring(fName.IndexOf("->") + "->".Length);
                            }
                            if (classType != null && !classType.Equals(""))
                            {
                                foundPairs.Add(new functionClassPair { functionName = fName, className = classType });
                            }
                        }
                        else if (this is PhpFunction && vName.Equals("$this")) // handle case where it's not a variable, but just self-referencing the class. Only applies to functions within classes, not other php code snippets.
                        {
                                foundPairs.Add(new functionClassPair { functionName = fName, className = ((PhpFunction)this).ParentClass.ClassName });                            
                        }
                        else
                        {
                            variables.Add(new variableFunctionPair { functionName = fName, variableName = vName });
                        }

                    }
                }
            }

            //find the class for those objects
            foreach (variableFunctionPair vp in variables)
            {
                string insNew = "\\" + vp.variableName + "\\s*=\\s*new\\s+\\S+\\s*\\(.*\\)";
                string insFactory = "\\" + vp.variableName + "\\s*=\\s+\\S+::\\S+\\(.*\\)";
                Regex insNewRE = new Regex(insNew, RegexOptions.IgnoreCase);
                Regex insFactoryRE = new Regex(insFactory, RegexOptions.IgnoreCase);

                //handle case where variable is instantiated using a call to "new" class()
                if (insNewRE.IsMatch(this.PhpCode))
                {
                    Match insMatch = insNewRE.Match(this.PhpCode); //making assumption that it is only ever instantiated as a single class type - this could break from polymorphism
                    string cMatch = this.PhpCode.Substring(insMatch.Index);
                    int indexOfNew = cMatch.IndexOf("new ") + "new ".Length;
                    int indexOfP = cMatch.IndexOf('(');
                    cMatch = cMatch.Substring(indexOfNew, indexOfP - indexOfNew);

                    foundPairs.Add(new functionClassPair { functionName = vp.functionName, className = cMatch });
                }
                else if (insFactoryRE.IsMatch(this.PhpCode)) //handle case where variable is instantiated using a factory: class->MakeSomething()
                {
                    Match insFmatch = insFactoryRE.Match(this.PhpCode);
                    string cMatch = this.PhpCode.Substring(insFmatch.Index);
                    int startI = cMatch.IndexOf("= ") + "= ".Length;
                    int endI = cMatch.IndexOf("->");
                    cMatch = cMatch.Substring(startI, endI - startI);
                    foundPairs.Add(new functionClassPair { functionName = vp.functionName, className = cMatch });
                }
                else
                {
                    //TODO: Handle case where variable making the function call is a global class member not initialized in this function
                }
            }

            //if the class is found in our PHP tree, add it to the list of results
            foreach (functionClassPair fp in foundPairs)
            {
                PhpFunction foundFunction = tree.FindPHPFunction(fp.functionName, fp.className);
                if (foundFunction != null)
                {
                    calledFunctions.Add(foundFunction);
                }
            }
            return calledFunctions;
        }
    }
}
