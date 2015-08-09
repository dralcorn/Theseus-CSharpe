using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
        * An instance of the GraphNavigatorIntermediate class is used by an instance of the ManualAgent or AutonomousAgent 
        * classes to travese a graph. It interprets what options for movemment are available given 
        * the agent's current position in the graph and relays that information in a simplified format to the agent. 
        * 
        * When the agent chooses a move, the GraphNavidatorIntermediate implements that move and updates its current position.
        *
        * The GraphNavidatorIntermediate class is designed to mimmic the behavior of a driver for an actual robot traversing 
        * a maze, relaying very simple signals/instructions between the agent class and the robot's sensors and motors.  
        */

    class GraphNavigatorIntermediate : Intermediate
    {
        /*************************
         *  VARIABLES
         *************************/

        Graph _graphBeingNavigated;

        Node _currentNode;

        string _orientation;

        /*************************
         *  GETTERS AND SETTERS
         *************************/

        public string Orientation // Read-Only
        {
            get { return _orientation; }
        }

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * GraphNavigatorIntermediate constructor takes a graph, starting node, and orientation ("north", "east", "south", or "west")
         * with that node, setting up the frame work in which an agent class will recieve information about it navigation options
         * within the graph and traverse the graph. 
         */

        public GraphNavigatorIntermediate(Graph graph, Node startingNode, string startingOrientation)
        {
            //System.Console.WriteLine("\nIn GraphNavigatorIntermediate()");

            if (!graph.ContainsNode(startingNode.Name))
            {
                System.Console.WriteLine("Error; Graph does not contain specified starting node");

                return;
            }

            _graphBeingNavigated = graph;

            _currentNode = startingNode;

            _orientation = startingOrientation;

            //System.Console.WriteLine("Leaving GraphNavigatorIntermediate()\n");
        }

        /*************************
         *  METHODS
         *************************/

        /*
         * CanGoNorth method returns true if going north is an option, false if it is not.
         */

        bool Intermediate.CanGoNorth()
        {

            //System.Console.WriteLine("\nIn CanGoNorth()");

            if (_currentNode.NorthArcOptionExists)
            {
                //System.Console.WriteLine("Returning true\nLeaving CanGoNorth()\n");

                return true;
            }

            //System.Console.WriteLine("Returning false\nLeaving CanGoNorth()\n");

            return false;
        }

        /*
         * CanGoEast method returns true if going east is an option, false if it is not.
         */

        bool Intermediate.CanGoEast()
        {
            //System.Console.WriteLine("\nIn CanGoEast()");

            if (_currentNode.EastArcOptionExists)
            {
                //System.Console.WriteLine("Returning true\nLeaving CanGoEast()\n");

                return true;
            }

            //System.Console.WriteLine("Returning false\nLeaving CanGoEast()\n");

            return false;
        }

        /*
         * CanGoSouth method returns true if going south is an option, false if it is not.
         */

        bool Intermediate.CanGoSouth()
        {
            //System.Console.WriteLine("\nIn CanGoSouth()");

            if (_currentNode.SouthArcOptionExists)
            {
                //System.Console.WriteLine("Returning true\nLeaving CanGoSouth()\n");

                return true;
            }

            //System.Console.WriteLine("Returning false\nLeaving CanGoSouth()\n");

            return false;
        }

        /*
         * CanGoWest method returns true if going west is an option, false if it is not.
         */

        bool Intermediate.CanGoWest()
        {
            //System.Console.WriteLine("\nIn CanGoWest()");

            if (_currentNode.WestArcOptionExists)
            {
                //System.Console.WriteLine("Returning true\nLeaving CanGoWest()\n");

                return true;
            }

            //System.Console.WriteLine("Returning false\nLeaving CanGoWest()\n");

            return false;
        }

        /*
         * GoNorth method moves the agent's current position to the node located directly to the north.
         */

        void Intermediate.GoNorth()
        {
            //System.Console.WriteLine("\nIn GoNorth()");

            _currentNode = _currentNode.NodeConnectedViaNorthArcOption;

            _orientation = "north";

            //System.Console.WriteLine("Leaving GoNorth()\n");
        }

        /*
         * GoEast method moves the agent's current position to the node located directly to the east.
         */

        void Intermediate.GoEast()
        {
            //System.Console.WriteLine("\nIn GoEast()");

            _currentNode = _currentNode.NodeConnectedViaEastArcOption;

            _orientation = "east";

            //System.Console.WriteLine("Leaving GoEast()\n");
        }

        /*
         * GoSouth method moves the agent's current position to the node located directly to the south.
         */

        void Intermediate.GoSouth()
        {
            //System.Console.WriteLine("\nIn GoSouth()");

            _currentNode = _currentNode.NodeConnectedViaSouthArcOption;

            _orientation = "south";

            //System.Console.WriteLine("Leaving GoSouth()\n");
        }

        /*
         * GoWest method moves the agent's current position to the node located directly to the west.
         */

        void Intermediate.GoWest()
        {
            //System.Console.WriteLine("\nIn GoWest()");

            _currentNode = _currentNode.NodeConnectedViaWestArcOption;

            _orientation = "west";

            //System.Console.WriteLine("Leaving GoWest()\n");
        }

        /*
         * TargetReached method returns if the target of the graph has been reached, false if it has not.
         */

        bool Intermediate.TargetReached()
        {
            //System.Console.WriteLine("\nIn TargetReached()");

            if (_currentNode.IsTarget)
            {
                //System.Console.WriteLine("Returning true\nLeaving TargetReached()\n");

                return true;
            }

            //System.Console.WriteLine("Returning false\nLeaving TargetReached()\n");

            return false;
        }

        /*
         * StartNewRun method sets the class's _currentNode variable to a random node in the graph. 
         * That node will be used as the starting node for the next run of the graph, which will be initiated by the agent class.  
         */

        void Intermediate.StartNewRun()
        {
            //System.Console.WriteLine("\nIn StartNewRun()");

            System.Console.WriteLine("Press c to choose the next run's starting node; Press any other key to have it choosen by random: ");

            string optionSelect = Console.ReadLine();

            if (optionSelect.Equals("c"))
            {
                System.Console.WriteLine("Enter the name of the node (A-Y): ");

                string selectedNodeAsString = Console.ReadLine();

                Char selectedNodeAsChar = Convert.ToChar(selectedNodeAsString[0]);

                while (selectedNodeAsChar < 'A' || selectedNodeAsChar > 'Y')
                {
                    System.Console.WriteLine("Node not recognized; Enter the name of the node (A-Y): ");

                    selectedNodeAsString = Console.ReadLine();

                    selectedNodeAsChar = Convert.ToChar(selectedNodeAsString[0]);
                }

                selectedNodeAsString = Convert.ToString(selectedNodeAsChar);

                _currentNode = _graphBeingNavigated.GetNode(selectedNodeAsString);
            }
            else
            {
                int randomNumber = Helper.RandomNumberBetweenRange(0, _graphBeingNavigated.NumNodes());

                Node randomStartingNode = _graphBeingNavigated.NodesSet.ElementAt(randomNumber);

                System.Console.WriteLine("There are " + _graphBeingNavigated.NumNodes() + " nodes in the graph\nNode number " +
                                      randomNumber + " was chosen\nIts name is " + randomStartingNode.Name);

                _currentNode = randomStartingNode;
            }

            //System.Console.WriteLine("Leaving StartNewRun()\n");
        }

        /*
         * Orient method updates the agent's orientation after each move. In the MazeNavigationIntermediate class this method
         * can set the _orientation variable by utilizing a robot's compass sensor. It can be called within the construtor and each 
         * of the Go methods. If a compass sensor is not available, the variable can be set explicitly as it is above. Tracking the 
         * orientation of the agent is only important in situations where it would effect the agent's movements. In a simulated,
         * abstract environment like a graph it is not required information.
         * 
         * This method is not used by this class and is only here for the purpose of symmetry between the classes. It is also a 
         * reminder of where code utilizing compass sensor signals would go if such a sensor was installed on the robot
         * and there was the intent to use the sensor as it traversed the maze.
         */

        void Intermediate.Orient()
        {
            //System.Console.WriteLine("\nIn Orient()");

            //System.Console.WriteLine("Leaving Orient()\n");
        }
    }
}
