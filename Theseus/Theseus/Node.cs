using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
     * An instance of the node class is used by the Graph class to repesent an intersection/vertex/node in the graph.
     * 
     * A node stores information such as which directionally fixed arc options they possess (North, East, Sout, West),
     * whether that option has been explored or not, a reference to the node connected to that option if that option has been 
     * explored, and whether the node is or is not the target node in the graph.
     */

    public class Node
    {
        /*************************
         *  VARIABLES
         *************************/

        private string _name; // Currently restricted to one letter (for example "A") due to the way in which the ContainsArc and 
        // GetArc methods in the Graph class are constucted.

        private bool _northArcOptionExists;

        private bool _eastArcOptionExists;

        private bool _southArcOptionExists;

        private bool _westArcOptionExists;

        private string _arcOptions;

        private bool _northArcOptionIsExplored;

        private bool _eastArcOptionIsExplored;

        private bool _southArcOtionIsExplored;

        private bool _westArcOptionIsExplored;

        private Node _nodeConnectedViaNorthArcOption;

        private Node _nodeConnectedViaEastArcOption;

        private Node _nodeConnectedViaSouthArcOption;

        private Node _nodeConnectedViaWestArcOption;

        private bool _isTarget;

        private double _cost;

        private double _northArcOptionCost;

        private double _eastArcOptionCost;

        private double _southArcOptionCost;

        private double _wesArcOptionCost;

        private int _xCoordinate;

        private int _yCoordinate;

        /*************************
         *  GETTERS AND SETTERS
         *************************/

        public string Name // Read-Only
        {
            get { return _name; }
        }

        public bool NorthArcOptionExists // Read-Only
        {
            get { return _northArcOptionExists; }
        }

        public bool EastArcOptionExists // Read-Only
        {
            get { return _eastArcOptionExists; }
        }

        public bool SouthArcOptionExists // Read-Only
        {
            get { return _southArcOptionExists; }
        }

        public bool WestArcOptionExists // Read-Only
        {
            get { return _westArcOptionExists; }
        }

        public string ArcOptions // Read-Only
        {
            get { return _arcOptions; }
        }

        public bool NorthArcOptionIsExplored // Read-Only
        {
            get { return _northArcOptionIsExplored; }
        }

        public bool EastArcOptionIsExplored // Read-Only
        {
            get { return _eastArcOptionIsExplored; }
        }

        public bool SouthArcOptionIsExplored // Read-Only
        {
            get { return _southArcOtionIsExplored; }
        }

        public bool WestArcOptionIsExplored // Read-Only
        {
            get { return _westArcOptionIsExplored; }
        }

        public Node NodeConnectedViaNorthArcOption // Read-Only
        {
            get { return _nodeConnectedViaNorthArcOption; }
        }

        public Node NodeConnectedViaEastArcOption // Read-Only
        {
            get { return _nodeConnectedViaEastArcOption; }
        }

        public Node NodeConnectedViaSouthArcOption // Read-Only
        {
            get { return _nodeConnectedViaSouthArcOption; }
        }

        public Node NodeConnectedViaWestArcOption // Read-Only
        {
            get { return _nodeConnectedViaWestArcOption; }
        }

        public bool IsTarget // Read/Write
        {
            get { return _isTarget; }
            set { _isTarget = value; }
        }

        public double Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public double NorthArcOptionCost
        {
            get { return _northArcOptionCost; }
            set { _northArcOptionCost = value; }
        }

        public double EastArcOptionCost
        {
            get { return _eastArcOptionCost; }
            set { _eastArcOptionCost = value; }
        }

        public double SouthArcOptionCost
        {
            get { return _southArcOptionCost; }
            set { _southArcOptionCost = value; }
        }

        public double WestArcOptionCost
        {
            get { return _wesArcOptionCost; }
            set { _wesArcOptionCost = value; }
        }

        public int XCoordinate // Read/Write
        {
            get { return _xCoordinate; }
            set { _xCoordinate = value; }
        }

        public int YCoordinate // Read/Write
        {
            get { return _yCoordinate; }
            set { _yCoordinate = value; }
        }

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * Node methods (2) construct the an instance of the node class. Called without parameters, a place-holder node 
         * is created. Place-holder nodes should not be present in a full created graph.
         * 
         * Node method with parameters builds the nodes that make up the actually traversable graph. 
         * It takes a name for the node and booleans for its arc options. True means that arc option exists 
         * (e.g. "true" for northArcOption means the node has an arc connected to the north), false means that 
         * arc option does not exist.
         * 
         * Node (with pararmeters) is called by the Graph class when it uses its AddNode method. Node (without parameters)
         * is called by Node (with parameters) as part of the traverable node construction process.
         * 
         * Node (without parameters) is also called by the Graph class when it uses it RemoveArc method, and must reset a node's 
         * connected neighbor node to the default, place-holder node value.
         */

        public Node()
        {
            //System.Console.WriteLine("\nIn Node() [Place-holder Constructor]");

            _name = "Z";

            _arcOptions = "None; Place-holder node";

            //System.Console.WriteLine("Leaving Node() [Place-holder Construtor]\n");
        }

        public Node(string name, bool northArcOption, bool eastArcOption, bool southArcOption, bool westArcOption)
        {
            //System.Console.WriteLine("\nIn Node()");

            _name = name;

            _northArcOptionExists = northArcOption;

            _eastArcOptionExists = eastArcOption;

            _southArcOptionExists = southArcOption;

            _westArcOptionExists = westArcOption;

            _northArcOptionIsExplored = false;

            _eastArcOptionIsExplored = false;

            _southArcOtionIsExplored = false;

            _westArcOptionIsExplored = false;

            StringBuilder tempNodeDescription = new StringBuilder();

            if (northArcOption == true)
            {
                tempNodeDescription.Append("N");

                _nodeConnectedViaNorthArcOption = new Node();
            }

            if (eastArcOption == true)
            {
                tempNodeDescription.Append("E");

                _nodeConnectedViaEastArcOption = new Node();
            }

            if (southArcOption == true)
            {
                tempNodeDescription.Append("S");

                _nodeConnectedViaSouthArcOption = new Node();
            }

            if (westArcOption == true)
            {
                tempNodeDescription.Append("W");

                _nodeConnectedViaWestArcOption = new Node();
            }

            _arcOptions = tempNodeDescription.ToString();

            _isTarget = false;

            _cost = 0;

            _northArcOptionCost = 0;

            _eastArcOptionCost = 0;

            _southArcOptionCost = 0;

            _wesArcOptionCost = 0;

            _xCoordinate = 0;

            _yCoordinate = 0;

            //WriteDetailedDescriptionOfNodeToConsole();

            //System.Console.WriteLine("Leaving Node()\n");
        }

        /*************************
         *  METHODS
         *************************/

        /*
         * ConnectNodeToAnotherViaSpecifiedArcOption method receives a reference to a neighboring node that will be connected to
         * the node invoking the method, along with the arcOption through which that neighboring node will be connected. 
         * 
         * The method is used by the Graph class when it uses its AddArc method.
         */

        public void ConnectNodeToAnotherViaSpecifiedArcOption(Node connectedNode, String arcOption)
        {
            //System.Console.WriteLine("\nIn UpdateNodeConnectedViaSpecifiedArcOption()");

            if (this.Equals(connectedNode))
            {
                System.Console.WriteLine("Node " + _name + " cannot be connected with itself; Nothing done");
            }
            else if (arcOption.Equals("north"))
            {
                if (_northArcOptionExists == true)
                {
                    _nodeConnectedViaNorthArcOption = connectedNode;

                    System.Console.WriteLine("Node " + _name + "'s north arc option now connects with node " + connectedNode._name);
                }
                else
                {
                    System.Console.WriteLine("Node " + _name + " does not have a north arc option; Nothing done");
                }
            }
            else if (arcOption.Equals("east"))
            {
                if (_eastArcOptionExists == true)
                {
                    _nodeConnectedViaEastArcOption = connectedNode;

                    System.Console.WriteLine("Node " + _name + "'s east arc option now connects to node " + connectedNode._name);
                }
                else
                {
                    System.Console.WriteLine("Node " + _name + " does not have a east arc option; Nothing done");
                }
            }
            else if (arcOption.Equals("south"))
            {
                if (_southArcOptionExists == true)
                {
                    _nodeConnectedViaSouthArcOption = connectedNode;

                    System.Console.WriteLine("Node " + _name + "'s south arc option now connects to node " + connectedNode._name);
                }
                else
                {
                    System.Console.WriteLine("Node " + _name + " does not have a south arc option; Nothing done");
                }
            }
            else if (arcOption.Equals("west"))
            {
                if (_westArcOptionExists == true)
                {
                    _nodeConnectedViaWestArcOption = connectedNode;

                    System.Console.WriteLine("Node " + _name + "'s west arc option now connects to node " + connectedNode._name);
                }
                else
                {
                    System.Console.WriteLine("Node " + _name + " does not have a west arc option; Nothing done");
                }
            }
            else
            {
                System.Console.WriteLine("Arc option not recognized; Nothing done");
            }

            //System.Console.WriteLine("Leaving UpdateNodeConnectedViaSpecifiedArcOption()\n");
        }

        /*
         * ResetSpecifiedArcOptionToDefault method receives an arcOption and connects a placer-holder node to that option.
         * 
         * The method is used by the Graph class when it uses its RemoveArc method. 
         */

        public void ResetSpecifiedArcOptionToDefault(string arcOption)
        {
            //System.Console.WriteLine("\nIn ResetSpecifiedArcOptionToDefault()");

            Node defaultNode = new Node();

            if (arcOption == "north" || arcOption == "east" || arcOption == "south" || arcOption == "west")
            {
                ConnectNodeToAnotherViaSpecifiedArcOption(defaultNode, arcOption);
            }
            else
            {
                System.Console.WriteLine("Arc option not recognized; Nothing done");
            }

            //System.Console.WriteLine("Leaving ResetSpecifiedArcOptionToDefault()\n");

        }

        /*
         * NodeArcOptionIsOpen gives a true or false responce to whether a specifed arc option is free/open to have another node
         * connected to it.
         */

        public bool NodeArcOptionIsOpen(string arcOption)
        {
            //System.Console.WriteLine("\nIn NodeArcOptionIsOpen()");

            bool answer = false;

            if (arcOption == "north")
            {
                if (_northArcOptionExists && _nodeConnectedViaNorthArcOption._name == "Z")
                {
                    answer = true;
                }
                else
                {
                    System.Console.WriteLine("North option is not open");
                }
            }
            else if (arcOption == "east")
            {
                if (_eastArcOptionExists && _nodeConnectedViaEastArcOption._name == "Z")
                {
                    answer = true;
                }
                else
                {
                    System.Console.WriteLine("East option is not open");
                }
            }
            else if (arcOption == "south")
            {
                if (_southArcOptionExists && _nodeConnectedViaSouthArcOption._name == "Z")
                {
                    answer = true;
                }
                else
                {
                    System.Console.WriteLine("South option is not open");
                }
            }
            else if (arcOption == "west")
            {
                if (_westArcOptionExists && _nodeConnectedViaWestArcOption._name == "Z")
                {
                    answer = true;
                }
                else
                {
                    System.Console.WriteLine("West option is not open");
                }
            }
            else
            {
                System.Console.WriteLine("Arc option not recognized; Nothing done");
            }

            //System.Console.WriteLine("Leaving NodeArcOptionIsOpen()\n");

            return answer;
        }

        /*
         * WriteDetailedDecriptionOfNodeToConsole prints the name, arc options, and connected neighbor nodes of a node.
         * 
         * The method is used by the node itself when being constructed for debugging purposes.
         */

        public void WriteDetailedDescriptionOfNodeToConsole()
        {
            //System.Console.WriteLine("\nIn WriteDetailedDescribeOfNodeToConsole()");

            System.Console.WriteLine("Name: " + _name +
                                     "\nDescription: " + _arcOptions +
                                     "\nCost: " + _cost);

            if (_northArcOptionExists == true)
            {
                System.Console.WriteLine("North explored: " + _northArcOptionIsExplored);

                System.Console.WriteLine("North node set to: " + _nodeConnectedViaNorthArcOption._name
                                + " - " + _nodeConnectedViaNorthArcOption._arcOptions);

                System.Console.WriteLine("North arc cost: " + _northArcOptionCost);
            }

            if (_eastArcOptionExists == true)
            {
                System.Console.WriteLine("East explored: " + _eastArcOptionIsExplored);

                System.Console.WriteLine("East node set to: " + _nodeConnectedViaEastArcOption._name
                                            + " - " + _nodeConnectedViaEastArcOption._arcOptions);

                System.Console.WriteLine("East arc cost: " + _eastArcOptionCost);
            }

            if (_southArcOptionExists == true)
            {
                System.Console.WriteLine("South explored: " + _southArcOtionIsExplored);

                System.Console.WriteLine("South node set to: " + _nodeConnectedViaSouthArcOption._name
                           + " - " + _nodeConnectedViaSouthArcOption._arcOptions);

                System.Console.WriteLine("South arc cost: " + _southArcOptionCost);
            }

            if (_westArcOptionExists == true)
            {
                System.Console.WriteLine("West explored: " + _westArcOptionIsExplored);

                System.Console.WriteLine("West node set to: " + _nodeConnectedViaWestArcOption._name
                                          + " - " + _nodeConnectedViaWestArcOption._arcOptions);

                System.Console.WriteLine("West arc cost: " + _wesArcOptionCost);
            }

            //System.Console.WriteLine("Leaving WriteDetailedDescribeOfNodeToConsole()\n");
        }

        /*
         * WasSpecifiedArcOptionExplored returns true if the arcOtion was explored, false if it was not. The arc option is specifed
         * by giving a string describing the direction of the arc option (i.e. "north", "south", "east", "west").
         */

        public bool WasSpecifiedArcOptionExplored(string arcOption)
        {
            //System.Console.WriteLine("\nIn WasSpecifiedArcOptionExplored()");

            if (arcOption.Equals("north"))
            {
                if (_northArcOptionIsExplored)
                {
                    //System.Console.WriteLine("Leaving WasSpecifiedArcOptionExplored()\n");

                    return true;
                }
            }
            else if (arcOption.Equals("east"))
            {
                if (_eastArcOptionIsExplored)
                {
                    //System.Console.WriteLine("Leaving WasSpecifiedArcOptionExplored()\n");

                    return true;
                }
            }
            else if (arcOption.Equals("south"))
            {
                if (_southArcOtionIsExplored)
                {
                    //System.Console.WriteLine("Leaving WasSpecifiedArcOptionExplored()\n");

                    return true;
                }
            }
            else if (arcOption.Equals("west"))
            {
                if (_westArcOptionIsExplored)
                {
                    //System.Console.WriteLine("Leaving WasSpecifiedArcOptionExplored()\n");

                    return true;
                }
            }

            //System.Console.WriteLine("Leaving WasSpecifiedArcOptionExplored()\n");

            return false;
        }

        /*
         * SpecifiedArcOptionWasExplored takes a string specifing an arc option (i.e. "north", "south", "east", "west")
         * and sets that arc option as having been explored.
         */

        public void SpecifiedArcOptionWasExplored(string arcOption)
        {
            //System.Console.WriteLine("\nIn SpecifiedArcOptionWasExplored()");

            if (arcOption.Equals("north"))
            {
                _northArcOptionIsExplored = true;
            }
            else if (arcOption.Equals("east"))
            {
                _eastArcOptionIsExplored = true;
            }
            else if (arcOption.Equals("south"))
            {
                _southArcOtionIsExplored = true;
            }
            else if (arcOption.Equals("west"))
            {
                _westArcOptionIsExplored = true;
            }

            //System.Console.WriteLine("Leaving SpecifiedArcOptionWasExplored()\n");
        }

        /*
         * NeighboringNodeToSpecifiedDirection returns the node which neighbors to the specified direction.
         */

        public Node NeighboringNodeToSpecifiedDirection(string arcOption)
        {
            //System.Console.WriteLine("In NeighboringNodeToSpecifiedDirection()\n");

            Node neighboringNode = new Node();

            if (arcOption.Equals("north"))
            {
                neighboringNode = _nodeConnectedViaNorthArcOption;
            }
            else if (arcOption.Equals("east"))
            {
                neighboringNode = _nodeConnectedViaEastArcOption;
            }
            else if (arcOption.Equals("south"))
            {
                neighboringNode = _nodeConnectedViaSouthArcOption;
            }
            else if (arcOption.Equals("west"))
            {
                neighboringNode = _nodeConnectedViaWestArcOption;
            }

            //System.Console.WriteLine("Leaving NeighboringNodeToSpecifiedDirection()\n");

            return neighboringNode;
        }
    }
}
