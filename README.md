# PhpCallGraphGenerator
This is a C# winforms application for generating call graphs of PHP code.  I built it in order to better visualize some PHP code that I was working on.

The program parses through a folder (and all subfolders) of PHP, and then builds a graph based on the calls found.  The call graph is visualized using the **[GraphX library](https://github.com/panthernet/GraphX)**.

Here is a sample graph built:

![Sample Code Graph](/Documents/SampleCodeGraph.png)

No guarantees are made in regards to the correctness of the results produced, or the quality of this code.