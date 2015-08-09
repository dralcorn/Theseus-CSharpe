using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
     * An instance of the arc class is used by the Graph class to repesent a(n) corridor/edge/arc between two nodes in the graph.
     * 
     * An arc stores its cost, as well as references to the two nodes that it is composed of, stored in a hashSet.
     */

    class Arc
    {
        /*************************
         *  VARIABLES
         *************************/

        private double _cost;

        private string _arcName;

        private HashSet<Node> _nodesInArc;

        /*************************
         *  GETTERS AND SETTERS
         *************************/

        public double Cost // Read/Write
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public string Name //Read-Only
        {
            get { return _arcName; }
        }

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * Arc method constructs an instance of the Arc class--receiving two nodes, adding them to a hashSet,
         * and setting the cost of the arc instance to a default value of 0.
         * 
         * Method is called by the Graph class through its AddArc method.
         */

        public Arc(Node node1, Node node2)
        {
            _nodesInArc = new HashSet<Node>();

            _cost = 0;

            _nodesInArc.Add(node1);

            _nodesInArc.Add(node2);

            _arcName = node1.Name + "<-->" + node2.Name;
        }

        /*************************
         *  METHODS
         *************************/

        /*
         * ContainsNode method receives a node and returns a bool--true if the node is present in the arc instance and 
         * false if it is not.
         * 
         * Method should always be called before calling the GetOtherNode method.
         */

        public bool ContainsNode(Node node)
        {
            //System.Console.WriteLine("\nIn ContainsNode()");

            //System.Console.WriteLine("Leaving ContainsNode()\n");

            return _nodesInArc.Contains(node);
        }

        /*
         * ContainsBothNodes method works the same as the ContainsNode method, except it receives two nodes when called and 
         * both must be present in the arc for the method to return true.
         */

        public bool ContainsBothNodes(Node node1, Node node2)
        {
            //System.Console.WriteLine("\nIn ContainsBothNodes()");

            //System.Console.WriteLine("Leaving ContainsBothNodes()\n");

            return (_nodesInArc.Contains(node1) && _nodesInArc.Contains(node2));
        }

        /*
         * GetOtherNode method returns the node that is paired with the node the method receives.
         * 
         * It should only be used after ContainsNode has returned true for the node being passed to the method.
         */

        public Node GetOtherNode(Node node)
        {
            //System.Console.WriteLine("\nIn GetOtherNode()");

            // Should only be used after checking that node is part of arc [i.e. ContainsNode(node) == true]

            if (_nodesInArc.First().Equals(node))
            {
                //System.Console.WriteLine("Leaving GetOtherNode()\n");

                return (_nodesInArc.Last());
            }

            //System.Console.WriteLine("Leaving GetOtherNode()\n");

            return (_nodesInArc.First());
        }
    }
}
