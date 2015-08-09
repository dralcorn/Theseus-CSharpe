using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Would you like to traverse a graph or a maze? ");

            string userOption1 = System.Console.ReadLine();

            while (!(userOption1.Equals("graph") || userOption1.Equals("maze")))
            {
                System.Console.WriteLine("Input not recognized. Please enter \"graph\" or \"maze\".");

                userOption1 = System.Console.ReadLine();
            }

            Intermediate intermediate;

            if (userOption1.Equals("graph")) 
            {
                System.Console.WriteLine("Graph option selected");

                Node node1 = new Node("A", false, false, true, false);
                Node node2 = new Node("B", false, true, false, false);
                Node node3 = new Node("C", false, true, true, true);
                Node node4 = new Node("D", false, false, false, true);
                Node node5 = new Node("E", false, false, true, false);
                Node node6 = new Node("F", true, false, true, false);
                Node node7 = new Node("G", false, true, true, false);
                Node node8 = new Node("H", true, true, true, true);
                Node node9 = new Node("I", false, false, true, true);
                Node node10 = new Node("J", true, false, true, false);
                Node node11 = new Node("K", true, true, true, false);
                Node node12 = new Node("L", true, false, true, true);
                Node node13 = new Node("M", true, false, false, false);
                Node node14 = new Node("N", true, true, true, false);
                Node node15 = new Node("O", true, false, true, true);
                Node node16 = new Node("P", true, false, true, false);
                Node node17 = new Node("Q", true, true, false, false);
                Node node18 = new Node("R", false, true, true, true);
                Node node19 = new Node("S", true, false, false, true);
                Node node20 = new Node("T", true, false, true, false);
                Node node21 = new Node("U", true, false, false, false);
                Node node22 = new Node("V", false, true, false, false);
                Node node23 = new Node("W", true, true, false, true);
                Node node24 = new Node("X", false, false, false, true);
                Node node25 = new Node("Y", true, false, false, false);

                node13.IsTarget = true;

                Graph graph1 = new Graph();

                graph1.AddNode(node1);
                graph1.AddNode(node2);
                graph1.AddNode(node3);
                graph1.AddNode(node4);
                graph1.AddNode(node5);
                graph1.AddNode(node6);
                graph1.AddNode(node7);
                graph1.AddNode(node8);
                graph1.AddNode(node9);
                graph1.AddNode(node10);
                graph1.AddNode(node11);
                graph1.AddNode(node12);
                graph1.AddNode(node13);
                graph1.AddNode(node14);
                graph1.AddNode(node15);
                graph1.AddNode(node16);
                graph1.AddNode(node17);
                graph1.AddNode(node18);
                graph1.AddNode(node19);
                graph1.AddNode(node20);
                graph1.AddNode(node21);
                graph1.AddNode(node22);
                graph1.AddNode(node23);
                graph1.AddNode(node24);
                graph1.AddNode(node25);

                graph1.AddArc(node1, "south", node6);

                graph1.AddArc(node2, "east", node3);

                graph1.AddArc(node3, "east", node4);
                graph1.AddArc(node3, "south", node8);

                graph1.AddArc(node5, "south", node10);

                graph1.AddArc(node6, "south", node11);

                graph1.AddArc(node7, "east", node8);
                graph1.AddArc(node7, "south", node12);

                graph1.AddArc(node8, "east", node9);
                graph1.AddArc(node8, "south", node13);

                graph1.AddArc(node9, "south", node14);

                graph1.AddArc(node10, "south", node15);

                graph1.AddArc(node11, "east", node12);
                graph1.AddArc(node11, "south", node16);

                graph1.AddArc(node12, "south", node17);

                graph1.AddArc(node14, "east", node15);
                graph1.AddArc(node14, "south", node19);

                graph1.AddArc(node15, "south", node20);

                graph1.AddArc(node16, "south", node21);

                graph1.AddArc(node17, "east", node18);

                graph1.AddArc(node18, "east", node19);
                graph1.AddArc(node18, "south", node23);

                graph1.AddArc(node20, "south", node25);

                graph1.AddArc(node22, "east", node23);

                graph1.AddArc(node23, "east", node24);

                /* List of nodes in graph should be: 
                 * 
                 *  A through Y (25 total)
                 * 
                 * List of arcs in graph should be: 
                 *  
                 *  A<-->F
                 *  B<-->C
                 *  C<-->D
                 *  C<-->H
                 *  E<-->J
                 *  F<-->K
                 *  G<-->H
                 *  G<-->L
                 *  H<-->I
                 *  H<-->M
                 *  I<-->N
                 *  J<-->O
                 *  K<-->L
                 *  K<-->P
                 *  L<-->Q
                 *  N<-->O
                 *  N<-->S
                 *  O<-->T
                 *  P<-->U
                 *  Q<-->R
                 *  R<-->S
                 *  R<-->W
                 *  T<-->Y
                 *  V<-->W
                 *  W<-->X
                 * 
                 * Graph structure looks like:
                 * 
                 *  A   B---C---D   E       
                 *  |       |       |
                 *  F   G---H---I   J
                 *  |   |   |   |   |
                 *  K---L   M   N---O
                 *  |   |       |   |
                 *  P   Q---R---S   T
                 *  |       |       |
                 *  U   V---W---X   Y
                 *  
                 * Graph target node is:
                 * 
                 *  M
                 *  
                 */

                intermediate = new GraphNavigatorIntermediate(graph1, node1, "north");
            }
            else
            {
                System.Console.WriteLine("Maze option selected");

                // When maze option is ready, it will go here.

                System.Console.WriteLine("Enter the arc's lighter color reading:");

                double lighterColorReading = Convert.ToDouble(System.Console.ReadLine());

                System.Console.WriteLine("Enter the arc's darker color reading:");

                double darkerColorReading = Convert.ToDouble(System.Console.ReadLine());

                System.Console.WriteLine("Enter the arc's reading for white:");

                double whiteReading = Convert.ToDouble(System.Console.ReadLine());

                intermediate = new MazeNavigatorIntermediate(darkerColorReading, lighterColorReading, whiteReading, "north");
            }

            System.Console.WriteLine("Would you like to traverse the graph manually? [n/y]");

            string userOption2 = System.Console.ReadLine();

            while (!(userOption2.Equals("y") || userOption2.Equals("n"))) 
            {

                System.Console.WriteLine("Input not recognized. Please enter \"n\" or \"y\".");

                userOption2 = System.Console.ReadLine();
            }

            if (userOption2.Equals("y")) 
            {

                System.Console.WriteLine("Manual option seleted");

                ManualAgent agent1 = new ManualAgent(intermediate);

                agent1.Traverse();
            }
            else {

                System.Console.WriteLine("Automatic option seleted");

                AutonomousAgent agent2 = new AutonomousAgent(intermediate);
            
                agent2.Traverse();

            }

            System.Console.WriteLine("------------------------");
            System.Console.WriteLine("- Press any key to end program");
            System.Console.WriteLine("------------------------\n\n");

            System.Console.ReadLine();
        }
    }
}
