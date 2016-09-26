using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer.JavascriptObjects
{
    class JavascriptFunction
    {
        public string Function { get; set; }
        public string Name { get; set; }

        public int StartIndexInOriginalFile { get; set; }
        public int EndIndexInOriginalFile { get; set; }

        public JavascriptFunction(string function, string name, int startIndex, int endIndex)
        {
            Function = function;
            Name = name;
            StartIndexInOriginalFile = startIndex;
            EndIndexInOriginalFile = endIndex;
        }

        public void AddHTMLFormValidationCheckToFunction(string codeToAdd)
        {
            throw new NotImplementedException();
        }

        public void AddInputParameterToFunction(string parameterToAdd)
        {
            throw new NotImplementedException();
        }
    }
}
