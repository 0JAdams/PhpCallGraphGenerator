using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class CodeFile
    {
        public string FullFileText { get; protected set; }
        public string FullFilePath { get; protected set; }
        public string FileName { get; protected set; }
        protected PHPAppData ParentTree;
    }
}
