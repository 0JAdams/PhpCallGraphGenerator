using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class ParsedFile
    {
        public FileInfo file { get; set; }
        public int functionCount { get { return functions.Count; } }
        public List<ParsedFunction> functions { get; set; }
        
        public ParsedFile()
        {
            functions = new List<ParsedFunction>();
        }


    }
}
