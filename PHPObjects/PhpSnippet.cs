using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{

    class PhpSnippet : PhpCodeObject
    {
        public PhpFile ParentFile { get; private set; }

        public PhpSnippet(string snip, PhpFile parentFile) : base(snip)
        {
            PhpCode = snip;
            ParentFile = parentFile;            
        }
        
    }
}
