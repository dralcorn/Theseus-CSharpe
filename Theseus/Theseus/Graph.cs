using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
        * An instance of the graph class uses the Arc and Node classes to construct a graph data structure.
        */

    class Graph
    {
        /*************************
         *  VARIABLES
         *************************/

        private HashSet<Node> _nodes;

        private HashSet<Arc> _arcs;

        /*************************
         *  GETTERS AND SETTERS
         *************************/

        public HashSet<Node> NodesSet // Read-Only (Used only by the GraphNavigatorIntermediate when using its StartNewRun method)
        {
            get { return _nodes; }
        }

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        public Graph()
        {
            //System.Console.WriteLine("\nIn Graph()");

            _nodes = new HashSet<Node>();

            _arcs = new HashSet<Arc>();

            //System.Console.WriteLine("Leaving Graph()\n");
        }

        /*************************
         *  METHODS
         *************************/

        /*
         * NumnNodes return the number of nodes in the graph.
         */

        public int NumNodes()
        {
            return _nodes.Count;
        }

        /*
         * NumArcs return the number of arcs in the graph.
         */

        public int NumArcs()
        {
            return _arcs.Count;
        }

        /*
         * AddNode adds an instance of node to the graph.
         */

        public void AddNode(Node node)
        {
            //System.Console.WriteLine("\nIn AddNode()");

            if (ContainsNode(node.Name))
            {
                System.Console.WriteLine("A node by that name already exists in the graph; Nothing Done");

                //System.Console.WriteLine("Leaving AddNode()\n");

                return;
            }

            _nodes.Add(node);

            //System.Console.WriteLine("Leaving AddNode()\n");
        }

        /*
         * AddArc adds an arc between two nodes to the graph.
         */

        public void AddArc(Node node1, string node1ConnectionOption, Node node2)
        {
            //System.Console.WriteLine("\nIn AddArc()");

            if (ContainsArc(node1.Name + "<-->" + node2.Name))
            {
                System.Console.WriteLine("An arc already exists between " + node1.Name + " and node " + node2.Name + "; Nothing done");

                //System.Console.WriteLine("Leaving AddArc()\n");

                return;
            }

            string node2ConnectionOption = Helper.GiveOppositeDirection(node1ConnectionOption);

            if (!(node1.NodeArcOptionIsOpen(node1ConnectionOption) && node2.NodeArcOptionIsOpen(node2ConnectionOption)))
            {
                if (!node2.NodeArcOptionIsOpen(node1ConnectionOption))
                {

                    System.Console.WriteLine("That arc option for node1 is not open; Nothing Done");
                }
                else
                {
                    System.Console.WriteLine("That arc option for node2 is not open; Nothing Done");
                }

                //System.Console.WriteLine("Leaving AddArc()\n");

                return;
            }

            node1.ConnectNodeToAnotherViaSpecifiedArcOption(node2, node1ConnectionOption);

            node2.ConnectNodeToAnotherViaSpecifiedArcOption(node1, node2ConnectionOption);

            Arc newArc = new Arc(node1, node2);

            _arcs.Add(newArc);

            //System.Console.WriteLine("Leaving AddArc()\n");
        }

        /*
         * RemoveNode removes a instance of node from the graph.
         */

        public void RemoveNode(Node node)
        {
            //System.Console.WriteLine("\nIn RemoveNode()");

            if (!ContainsNode(node.Name))
            {
                System.Console.WriteLine("Node not found in list of nodes");

                //System.Console.WriteLine("Leaving RemoveNode()\n");

                return;
            }

            // Check which arcOptions a node has and if those options are connected to other nodes

            if (node.NorthArcOptionExists)
            {
                if (!node.NodeConnectedViaNorthArcOption.Name.Equals("Z"))
                {
                    System.Console.WriteLine("Removing arc connecting node " + node.Name + " to node " + node.NodeConnectedViaNorthArcOption.Name);

                    RemoveArc(node, node.NodeConnectedViaNorthArcOption);
                }
            }

            if (node.EastArcOptionExists)
            {
                if (!node.NodeConnectedViaEastArcOption.Name.Equals("Z"))
                {
                    System.Console.WriteLine("Removing arc connecting node " + node.Name + " to node to node " + node.NodeConnectedViaEastArcOption.Name);

                    RemoveArc(node, node.NodeConnectedViaEastArcOption);
                }
            }

            if (node.SouthArcOptionExists)
            {
                if (!node.NodeConnectedViaSouthArcOption.Name.Equals("Z"))
                {
                    System.Console.WriteLine("Removing arc connecting node " + node.Name + " to node " + node.NodeConnectedViaSouthArcOption.Name);

                    RemoveArc(node, node.NodeConnectedViaSouthArcOption);
                }
            }

            if (node.WestArcOptionExists)
            {
                if (!node.NodeConnectedViaWestArcOption.Name.Equals("Z"))
                {
                    System.Console.WriteLine("Removing arc connecting node " + node.Name + " to node " + node.NodeConnectedViaWestArcOption.Name);

                    RemoveArc(node, node.NodeConnectedViaWestArcOption);
                }
            }

            // Remove the node from the list of nodes

            _nodes.Remove(node);

            //System.Console.WriteLine("Leaving RemoveNode()\n");
        }

        /*
         * RemoveArc removes from the graph an arc between two nodes.
         */

        public void RemoveArc(Node node1, Node node2)
        {
            //System.Console.WriteLine("\nIn RemoveArc()");

            if (!ContainsArc(node1.Name + "<-->" + node2.Name))
            {
                System.Console.WriteLine("Arc not found in list of arcs; Nothing done");

                //System.Console.WriteLine("Leaving RemoveArc()\n");

                return;
            }

            // Remove actual connection between nodes

            if (node1.NodeConnectedViaNorthArcOption.Equals(node2))
            {
                node1.ResetSpecifiedArcOptionToDefault("north");

                node2.ResetSpecifiedArcOptionToDefault("south");
            }
            else if (node1.NodeConnectedViaEastArcOption.Equals(node2))
            {
                node1.ResetSpecifiedArcOptionToDefault("east");

                node2.ResetSpecifiedArcOptionToDefault("west");
            }
            else if (node1.NodeConnectedViaSouthArcOption.Equals(node2))
            {
                node1.ResetSpecifiedArcOptionToDefault("south");

                node2.ResetSpecifiedArcOptionToDefault("north");
            }
            else if (node1.NodeConnectedViaWestArcOption.Equals(node2))
            {
                node1.ResetSpecifiedArcOptionToDefault("west");

                node2.ResetSpecifiedArcOptionToDefault("east");
            }
            else
            {
                System.Console.WriteLine("Error; Connection between nodes not found");

                //System.Console.WriteLine("Leaving RemoveArc()\n");

                return;
            }

            // Remove arc from set

            string arcName1 = node1.Name + "<-->" + node2.Name;

            string arcName2 = node2.Name + "<-->" + node1.Name;

            System.Console.WriteLine("Looking for arc in set by name of " + arcName1 + " or " + arcName2);

            foreach (Arc arc in _arcs)
            {
                if (arc.Name.Equals(arcName1) || arc.Name.Equals(arcName2))
                {
                    System.Console.WriteLine("Arc " + arc.Name + " found in list of arcs and being removed");

                    _arcs.Remove(arc);

                    break;  // The list of arc has shrunk by one. If the foreach is not broken it will 
                    // eventually attempt to access one more than the list's current size.
                }
            }

            //System.Console.WriteLine("Leaving RemoveArc()\n");
        }

        /*
         * ContainsNode return true if a given node name is already present in the list of nodes  
         */

        public bool ContainsNode(string nodeName)
        {
            //System.Console.WriteLine("\nIn ContainsNode()");

            System.Console.WriteLine("Looking for a node named \"" + nodeName + "\"");

            foreach (Node node in _nodes)
            {
                if (node.Name.Equals(nodeName))
                {
                    System.Console.WriteLine("A node by that name was found");

                    //System.Console.WriteLine("Leaving ContainsNode()\n");

                    return true;
                }
            }

            System.Console.WriteLine("A node by that name was NOT found");

            //System.Console.WriteLine("Leaving ContainsNode()\n");

            return false;
        }

        /*
         * ContainsArc return true if a given arc name is already present in the list of arc  
         */

        public bool ContainsArc(string arcName)
        {
            //System.Console.WriteLine("\nIn ContainsArc()");

            string reverseArcName = arcName.Substring(5, 1) + "<-->" + arcName.Substring(0, 1);

            System.Console.WriteLine("Looking for arc " + reverseArcName + ", as well as " + arcName);

            foreach (Arc arc in _arcs)
            {
                if (arc.Name.Equals(arcName) || arc.Name.Equals(reverseArcName))
                {
                    System.Console.WriteLine("An arc by the name of " + arc.Name + " was found");

                    //System.Console.WriteLine("Leaving ContainsArc()\n");

                    return true;
                }
            }

            System.Console.WriteLine("An arc by that name was NOT found");

            //System.Console.WriteLine("Leaving ContainsArc()\n");

            return false;
        }

        /*
         * GetNode returns the node that has the same specified name
         */

        public Node GetNode(string nodeName)
        {
            //System.Console.WriteLine("\nIn GetNode()");

            Node blankNode = new Node();

            foreach (Node node in _nodes)
            {
                if (node.Name.Equals(nodeName))
                {
                    System.Console.WriteLine("Node found");

                    //System.Console.WriteLine("Leaving GetNode()\n");

                    return node;
                }
            }

            System.Console.WriteLine("Node NOT found; Return default/blank node");

            //System.Console.WriteLine("Leaving GetNode()\n");

            return blankNode;
        }

        /*
         * GetArc returns the arc that has the same specified name
         */

        public Arc GetArc(string arcName)
        {
            //System.Console.WriteLine("\nIn GetArc()");

            Node node1 = new Node();

            Node node2 = new Node();

            Arc blankArc = new Arc(node1, node2);

            string reverseArcName = arcName.Substring(5, 1) + "<-->" + arcName.Substring(0, 1);

            System.Console.WriteLine("Looking for arc " + reverseArcName + ", as well as " + arcName);

            foreach (Arc arc in _arcs)
            {
                if (arc.Name.Equals(arcName) || arc.Name.Equals(reverseArcName))
                {
                    System.Console.WriteLine("Arc " + arc.Name + " found");

                    //System.Console.WriteLine("Leaving GetArc()\n");

                    return arc;
                }
            }

            System.Console.WriteLine("Arc NOT found; Return default/blank arc");

            //System.Console.WriteLine("Leaving GetArc()\n");

            return blankArc;
        }

        /*
         * ListNodes lists all nodes added to the graph.
         */

        public void ListNodes()
        {
            //System.Console.WriteLine("\nIn ListNodes()");

            foreach (Node node in _nodes)
            {
                System.Console.WriteLine(node.Name + " [" + node.XCoordinate + "," + node.YCoordinate + "]");
            }

            //System.Console.WriteLine("Leaving ListNodes()\n");
        }

        /*
         * ListArcs list all arc between nodes that have been added to the graph.
         */

        public void ListArcs()
        {
            //System.Console.WriteLine("\nIn ListArcs()");

            foreach (Arc arc in _arcs)
            {
                System.Console.WriteLine(arc.Name);
            }

            //System.Console.WriteLine("Leaving ListArcs()\n");
        }

        /*
         * Display Graph writes a repesentation of the graph to the console.
         */

        public void DisplayGraph()
        {
            //System.Console.WriteLine("\nIn DisplayGraph()");

            // UNDER CONSTRUCTION

            //System.Console.WriteLine("Leaving DisplayGraph()\n");
        }
    }
}
