using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    struct GraphEdge
    {
        public PhpCodeObject FromThisFunction { get; set; }
        public PhpCodeObject ToThisFunction { get; set; }
    }

    class PhpClassGraph
    {
        public List<PhpCodeObject> PhpFunctionNodes;
        public List<GraphEdge> PhpFunctionEdges;
        private List<PhpCodeObject> usedFunctions;

        public PhpClassGraph()
        {
            PhpFunctionNodes = new List<PhpCodeObject>();
            PhpFunctionEdges = new List<GraphEdge>();
            usedFunctions = new List<PhpCodeObject>();
        }



        public void CreateCallGraphForEntireTree(PHPAppData data)
        {
            Queue<PhpCodeObject> phpCodeToSearch = new Queue<PhpCodeObject>();

            
            //get all PHP code objects that we need to build a graph of and use them as nodes
            foreach(PhpFile f in data.CodeFiles.OfType<PhpFile>())
            {
                if(f.Snippet!=null)
                {
                    phpCodeToSearch.Enqueue(f.Snippet);
                    PhpFunctionNodes.Add(f.Snippet);
                }

                foreach(PhpClass c in f.Classes)
                {
                    foreach(PhpFunction func in c.Functions)
                    {
                        phpCodeToSearch.Enqueue(func);
                        PhpFunctionNodes.Add(func);
                    }
                }
            }

            
            PhpCodeObject code = null;
            while (phpCodeToSearch.Count>0)
            {

                code = phpCodeToSearch.Dequeue();

                //get objects this code calls
                List<PhpCodeObject> calleeFuncs = data.FindCalleeObjects(code);

                //add edges from this code object to all of its callees.
                foreach(PhpCodeObject pco in calleeFuncs)
                {
                    if (code != pco) 
                        PhpFunctionEdges.Add(new GraphEdge { FromThisFunction = code, ToThisFunction = pco });
                }

               
            }
        }

        /// <summary>
        /// Traces php function calls to build a call graph centered around a particular function
        /// </summary>
        /// <param name="data">PHPAppData object that contains full PHP hierarchy</param>
        /// <param name="Functions"></param>
        public void CreateCallGraphForSpecificFunctions(PHPAppData data, List<PhpFunction> Functions)
        {
            //start with initial functions and build the tree upwards to find initial calls
            Queue<PhpFunction> functionQueue = new Queue<PhpFunction>();
            foreach(PhpFunction pf in Functions)
            {
                functionQueue.Enqueue(pf);
            }
            PhpFunction currentFunc;
            while(functionQueue.Count > 0)
            {
                //Get first function phpFunction to node list
                currentFunc = functionQueue.Dequeue();
                PhpFunctionNodes.Add(currentFunc);

                //find all functions that call this function, add them to queue, and create edges
                List<PhpCodeObject> callerFuncs = data.FindCallerObjects(currentFunc);
                foreach(PhpCodeObject pf in callerFuncs)
                {
                    PhpFunctionEdges.Add(new GraphEdge { FromThisFunction = pf, ToThisFunction = currentFunc });
                    if(pf is PhpSnippet && !usedFunctions.Contains(pf))
                    {
                        //add snippets to node list since it won't be processed later from queue
                        PhpFunctionNodes.Add(pf);
                        usedFunctions.Add(pf);
                    }
                    else if (pf is PhpFunction && !usedFunctions.Contains(pf) && !functionQueue.Contains(pf)) 
                    {
                        functionQueue.Enqueue(pf as PhpFunction);
                    }
                }
                //add currentFunc to list of visited functions so that we don't revisit functions
                usedFunctions.Add(currentFunc);
                //repeat until queue is empty
            }


            //start with initial functions and build the tree downwards to trace all calls made within our code            
            usedFunctions.Clear();
            functionQueue = new Queue<PhpFunction>();
            foreach (PhpCodeObject co in PhpFunctionNodes)
            {
                if(co is PhpFunction) 
                {
                    functionQueue.Enqueue(co as PhpFunction); 
                }
            }

            //loop through finding callees until queue is empty and entire graph is built
            while(functionQueue.Count > 0)
            {
                currentFunc = functionQueue.Dequeue();
                if(!PhpFunctionNodes.Contains(currentFunc))
                {
                    PhpFunctionNodes.Add(currentFunc);
                }

                List<PhpCodeObject> calleeFuncs = data.FindCalleeObjects(currentFunc);
                foreach(PhpCodeObject pc in calleeFuncs)
                {
                    PhpFunctionEdges.Add(new GraphEdge { FromThisFunction = currentFunc, ToThisFunction = pc });
                    //there shouldn't be any way to call a snippet from a function, so we only have to handle calls to functions this time
                    if(!usedFunctions.Contains(pc) && !functionQueue.Contains(pc))
                    {
                        functionQueue.Enqueue(pc as PhpFunction);
                    }

                    usedFunctions.Add(currentFunc);
                }
            }



        }

       
    }
}
