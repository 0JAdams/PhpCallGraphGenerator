using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class ParsedFunction
    {
        public string functionText { get; set; }
        public string functionName { get; set; }
        public string containingClass { get; set; }
        public ParsedFunction() { }
    }
}
