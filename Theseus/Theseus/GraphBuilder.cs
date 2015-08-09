using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
      * An insatance of the GraphBuilder class is used by an agent class to record what has been learned about
      * the maze/graph that is being navigated.
      */
    class GraphBuilder
    {
        /*************************
         *  VARIABLES
         *************************/

        // VARIABLES FOR CURRENT RUN GRAPH

        private Graph _currentGraph;

        private Node _currentNode; // Also used by Master Graph

        private Node _previousNode; // Also used by Master Graph

        private string _directionTraveled; // Also used by Master Graph

        private int _xCoordinate; // Also used by Master Graph

        private int _yCoordinate; // Also used by Master Graph

        private List<string> _alreadyIssuedCoordinates;

        private List<string> _nodeNamesList; // Also used by Master Graph

        private int _nodeNamesListCounter;

        private List<string> _descriptionsOfNodesUsedInCurrentRun;

        private List<string> _directionsTraveledInCurrentRun;

        // VARIABLES FOR MASTER GRAPH

        private Graph _masterGraph;

        private List<string> _masterGraphAlreadyIssuedCoordinates;

        private int _masterGraphNodeNamesListCounter;

        private Node _masterGraphTargetNode;

        private int _masterGraphTargetNodeXCoordinate;

        private int _masterGraphTargetNodeYCoordinate;

        /*************************
         *  GETTERS AND SETTERS
         *************************/

        public Node CurrentNode // Read-Only
        {
            get { return _currentNode; }
        }

        public int XCoordinate // Read-Only
        {
            get { return _xCoordinate; }
        }

        public int YCoordinate // Read-Only
        {
            get { return _yCoordinate; }
        }

        public List<string> AleardyIssuedCoordinatess // Read-Only
        {
            get { return _alreadyIssuedCoordinates; }
        }

        public Graph CurrentGraph // Read-Only
        {
            get { return _currentGraph; }
        }

        public Graph MasterGraph // Read-Only
        {
            get { return _masterGraph; }
        }

        public List<string> NodeNameList // Read-Only
        {
            get { return _nodeNamesList; }
        }

        public List<string> DescriptionsOfNodesUsedInCurrentRun // Read-Only
        {
            get { return _descriptionsOfNodesUsedInCurrentRun; }
        }

        public List<string> DirectionsTraveledInCurrentRun // Reaad-Only
        {
            get { return _directionsTraveledInCurrentRun; }
        }

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * GraphBuilder constructs an instance of the GraphBuilder class. 
         */

        public GraphBuilder()
        {
            //System.Console.WriteLine("\nIn GraphBuilder()");

            _currentGraph = new Graph();

            _xCoordinate = 0;

            _yCoordinate = 0;

            _alreadyIssuedCoordinates = new List<string>();

            _currentNode = new Node();

            _previousNode = new Node();

            _directionTraveled = "none";

            _nodeNamesList = new List<string> {"A", "B", "C", "D", "E",
                                               "F", "G", "H", "I", "J",
                                               "K", "L", "M", "N", "O",
                                               "P", "Q", "R", "S", "T",
                                               "U", "V", "W", "X", "Y",};

            _nodeNamesListCounter = 0;

            _descriptionsOfNodesUsedInCurrentRun = new List<string>();

            _directionsTraveledInCurrentRun = new List<string>();

            _masterGraph = new Graph();

            _masterGraphAlreadyIssuedCoordinates = new List<string>();

            _masterGraphNodeNamesListCounter = 0;

            _masterGraphTargetNode = new Node();

            _masterGraphTargetNodeXCoordinate = 0;

            _masterGraphTargetNodeYCoordinate = 0;

            //System.Console.WriteLine("Leaving GraphBuilder()\n");
        }

        /*************************
         *  METHODS
         *************************/

        /*
         * RecordAndConnectNode takes a description of the intersection/node the agent is currently at and if that intersection/node
         * has not already been visited by the agent, records it in the form node within a graph, attaching it via an arc to the 
         * intersection/node the agent just traveled from. If the node is not new, but the arc the agent traversed to get to it was
         * up till then unexplored, the method will only add the arc between the two known nodes to the graph. If the node is not new
         * and the arc traversed to get there was already explored, the method will add nothing to the graph.
         */

        public void RecordAndConnectNode(bool northArcOption, bool eastArcOption, bool southArcOption, bool westArcOption)
        {
            //System.Console.WriteLine("\nIn RecordAndConnectNode()");

            if (_directionTraveled.Equals("none"))
            {
                System.Console.WriteLine("Setting up and adding first node to GraphBuilder's Current Run Graph");

                _currentNode = new Node(_nodeNamesList.ElementAt(_nodeNamesListCounter), northArcOption, eastArcOption, southArcOption, westArcOption);

                _descriptionsOfNodesUsedInCurrentRun.Add(_currentNode.ArcOptions);

                _currentNode.XCoordinate = _xCoordinate;

                _currentNode.YCoordinate = _yCoordinate;

                _currentGraph.AddNode(_currentNode);

                _alreadyIssuedCoordinates.Add("[" + _xCoordinate + "," + _yCoordinate + "]");

                _nodeNamesListCounter += 1;
            }
            else
            {
                _previousNode = _currentNode;

                if (_previousNode.WasSpecifiedArcOptionExplored(_directionTraveled) &&
                    _alreadyIssuedCoordinates.Contains("[" + _xCoordinate + "," + _yCoordinate + "]"))
                {
                    System.Console.WriteLine("Have traveled to an already known node along an already known path");

                    int tempIndex = _alreadyIssuedCoordinates.IndexOf("[" + _xCoordinate + "," + _yCoordinate + "]");

                    _currentNode = _currentGraph.GetNode(_nodeNamesList.ElementAt(tempIndex));

                    _descriptionsOfNodesUsedInCurrentRun.Add(_currentNode.ArcOptions);
                }
                else if (_alreadyIssuedCoordinates.Contains("[" + _xCoordinate + "," + _yCoordinate + "]"))
                {
                    System.Console.WriteLine("Have traveled to an already known node along a previously unknown path");

                    int tempIndex = _alreadyIssuedCoordinates.IndexOf("[" + _xCoordinate + "," + _yCoordinate + "]");

                    _currentNode = _currentGraph.GetNode(_nodeNamesList.ElementAt(tempIndex));

                    _descriptionsOfNodesUsedInCurrentRun.Add(_currentNode.ArcOptions);

                    _previousNode.SpecifiedArcOptionWasExplored(_directionTraveled);

                    _currentGraph.AddArc(_previousNode, _directionTraveled, _currentNode);

                    _currentNode.SpecifiedArcOptionWasExplored(Helper.GiveOppositeDirection(_directionTraveled));
                }
                else
                {
                    System.Console.WriteLine("Have traveled to an unknown node");

                    _previousNode.SpecifiedArcOptionWasExplored(_directionTraveled);

                    _currentNode = new Node(_nodeNamesList.ElementAt(_nodeNamesListCounter), northArcOption, eastArcOption, southArcOption, westArcOption);

                    _currentNode.SpecifiedArcOptionWasExplored(Helper.GiveOppositeDirection(_directionTraveled));

                    _descriptionsOfNodesUsedInCurrentRun.Add(_currentNode.ArcOptions);

                    _currentNode.XCoordinate = _xCoordinate;

                    _currentNode.YCoordinate = _yCoordinate;

                    _alreadyIssuedCoordinates.Add("[" + _xCoordinate + "," + _yCoordinate + "]");

                    _currentGraph.AddNode(_currentNode);

                    _currentGraph.AddArc(_previousNode, _directionTraveled, _currentNode);

                    _nodeNamesListCounter += 1;
                }
            }

            //System.Console.WriteLine("Leaving ()\n");
        }

        /*
         * GoNorth records that the last arc option traveled was in a northernly direction, updateing the _directionTraveled
         * and _yCoordianate variables accordingly. It is called by the agent when it also tells the intermediate that it 
         * would like to travel north.
         */

        public void GoNorth()
        {
            //System.Console.WriteLine("\nIn GoingNorth()");

            _directionTraveled = "north";

            _yCoordinate += 1;

            _directionsTraveledInCurrentRun.Add("north");

            //System.Console.WriteLine("Leaving GoingNorth()\n");
        }

        /*
         * GoEast records that the last arc option traveled was in a easternly direction, updateing the _directionTraveled
         * and _xCoordianate variables accordingly. It is called by the agent when it also tells the intermediate that it 
         * would like to travel east.
         */

        public void GoEast()
        {
            //System.Console.WriteLine("\nIn GoingEast()");

            _directionTraveled = "east";

            _xCoordinate += 1;

            _directionsTraveledInCurrentRun.Add("east");

            //System.Console.WriteLine("Leaving GoingEast()\n");
        }

        /*
         * GoSouth records that the last arc option traveled was in a southernly direction, updateing the _directionTraveled
         * and _yCoordianate variables accordingly. It is called by the agent when it also tells the intermediate that it 
         * would like to travel south.
         */

        public void GoSouth()
        {
            //System.Console.WriteLine("\nIn GoingSouth()");

            _directionTraveled = "south";

            _yCoordinate -= 1;

            _directionsTraveledInCurrentRun.Add("south");

            //System.Console.WriteLine("Leaving GoingSouth()\n");
        }

        /*
         * GoWest records that the last arc option traveled was in a westernly direction, updateing the _directionTraveled
         * and _xCoordianate variables accordingly. It is called by the agent when it also tells the intermediate that it 
         * would like to travel west.
         */

        public void GoWest()
        {
            //System.Console.WriteLine("\nIn GoingWest()");

            _directionTraveled = "west";

            _xCoordinate -= 1;

            _directionsTraveledInCurrentRun.Add("west");

            //System.Console.WriteLine("Leaving GoingWest()\n");
        }

        /*
         * ListWhatWasLearnedThroughLastRun lists all the nodes and arcs that have been added to the graph through out the last run.
         * It is called by the agent when the target node has been reached.
         */

        public void ListWhatWasLearnedThroughLastRun()
        {
            //System.Console.WriteLine("\nIn ListWhatWasLearnedThroughLastRun()");

            _currentGraph.ListNodes();

            _currentGraph.ListArcs();

            //System.Console.WriteLine("Leaving ListWhatWasLearnedThroughLastRun()\n");
        }

        /*
         * StartNewRun prepares the GraphBuilder to begin an new run by updating the Master Graph and reseting all Current Run Graph
         * variables to their start state.
         */

        public void StartNewRun()
        {
            //System.Console.WriteLine("\nIn StartNewRun()");

            UpdateMasterGraph();

            _currentGraph = new Graph();

            _xCoordinate = 0;

            _yCoordinate = 0;

            _alreadyIssuedCoordinates = new List<string>();

            _currentNode = new Node();

            _previousNode = new Node();

            _directionTraveled = "none";

            _nodeNamesListCounter = 0;

            _descriptionsOfNodesUsedInCurrentRun = new List<string>();

            _directionsTraveledInCurrentRun = new List<string>();

            //System.Console.WriteLine("Leaving StartNewRun()\n");
        }

        /*
         * UpdateMasterGraph is used to add what was learned in the last run to the Master Graph.
         */

        private void UpdateMasterGraph()
        {
            //System.Console.WriteLine("\nIn UpdateMasterGraph()");

            List<string> reversedPathOfDirectionsTraveledInCurrentRun = new List<string>();

            System.Console.WriteLine("The path traveled in the last run was...");

            foreach (string direction in _directionsTraveledInCurrentRun)
            {
                System.Console.Write(direction + " -- ");

                reversedPathOfDirectionsTraveledInCurrentRun.Insert(0, Helper.GiveOppositeDirection(direction));
            }

            System.Console.WriteLine("AT TARGET NODE");

            System.Console.WriteLine("Reversing that path from the target node, the path is now...");

            foreach (string reversedDirection in reversedPathOfDirectionsTraveledInCurrentRun)
            {
                System.Console.Write(reversedDirection + " -- ");
            }

            System.Console.WriteLine("AT LAST RUN'S STARTING NODE");

            List<string> reversedListOfDescriptionsOfNodesUsedInCurrentRun = new List<string>();

            System.Console.WriteLine("The nodes used to get to the target node, in order, look as follows...");

            foreach (String nodeDescription in _descriptionsOfNodesUsedInCurrentRun)
            {
                System.Console.Write(nodeDescription + " -- ");

                reversedListOfDescriptionsOfNodesUsedInCurrentRun.Insert(0, nodeDescription);
            }

            System.Console.WriteLine("AT TARGET NODE");

            System.Console.WriteLine("That list of node descriptions reversed look as follows...");

            foreach (String nodeDescriptionFromReversedList in reversedListOfDescriptionsOfNodesUsedInCurrentRun)
            {
                System.Console.Write(nodeDescriptionFromReversedList + " -- ");
            }

            System.Console.WriteLine("AT LAST RUN'S STARTING NODE");

            if (_masterGraph.NumNodes() == 0)
            {
                _masterGraph = _currentGraph;

                _masterGraphTargetNode = _currentNode;

                _masterGraphTargetNode.IsTarget = true;

                _masterGraphTargetNodeXCoordinate = _xCoordinate;

                _masterGraphTargetNodeYCoordinate = _yCoordinate;

                _masterGraphAlreadyIssuedCoordinates = _alreadyIssuedCoordinates;

                _masterGraphNodeNamesListCounter = _nodeNamesListCounter;
            }
            else
            {
                _currentNode = _masterGraphTargetNode; // Not necessary; Simply explicative  

                _xCoordinate = _masterGraphTargetNodeXCoordinate;

                _yCoordinate = _masterGraphTargetNodeYCoordinate;

                int backtrackingCounter = 0;

                while (backtrackingCounter < reversedPathOfDirectionsTraveledInCurrentRun.Count)
                {
                    string backtrackingDirectionTraveled = reversedPathOfDirectionsTraveledInCurrentRun.ElementAt(backtrackingCounter);

                    if (backtrackingDirectionTraveled.Equals("north"))
                    {
                        GoNorth();
                    }

                    if (backtrackingDirectionTraveled.Equals("east"))
                    {
                        GoEast();
                    }

                    if (backtrackingDirectionTraveled.Equals("south"))
                    {
                        GoSouth();
                    }

                    if (backtrackingDirectionTraveled.Equals("west"))
                    {
                        GoWest();
                    }

                    bool northArcOptionIsPresent = false;

                    bool eastArcOptionIsPresent = false;

                    bool southArcOptionIsPresent = false;

                    bool westArcOptionIsPresent = false;

                    if (reversedListOfDescriptionsOfNodesUsedInCurrentRun.ElementAt(backtrackingCounter + 1).Contains("N"))
                    {
                        northArcOptionIsPresent = true;
                    }

                    if (reversedListOfDescriptionsOfNodesUsedInCurrentRun.ElementAt(backtrackingCounter + 1).Contains("E"))
                    {
                        eastArcOptionIsPresent = true;
                    }

                    if (reversedListOfDescriptionsOfNodesUsedInCurrentRun.ElementAt(backtrackingCounter + 1).Contains("S"))
                    {
                        southArcOptionIsPresent = true;
                    }

                    if (reversedListOfDescriptionsOfNodesUsedInCurrentRun.ElementAt(backtrackingCounter + 1).Contains("W"))
                    {
                        westArcOptionIsPresent = true;
                    }

                    UpdateMasterGraphHelper(northArcOptionIsPresent, eastArcOptionIsPresent, southArcOptionIsPresent, westArcOptionIsPresent);

                    backtrackingCounter += 1;
                }
            }

            System.Console.WriteLine("The GraphBuilder's Master Graph is now setup as follows...");

            _masterGraph.ListNodes();

            _masterGraph.ListArcs();

            System.Console.WriteLine("The GraphBuilder's Master Graph target node is: " +
                                        _masterGraphTargetNode.Name + " [" + _masterGraphTargetNodeXCoordinate +
                                        "," + _masterGraphTargetNodeYCoordinate + "]");

            //System.Console.WriteLine("Leaving UpdateMasterGraph()\n");
        }

        /*
         * UpdateMasterGraphHelper works similar to RecordAndConnectNode, but adds nodes to the Master Graph instead of the Current
         * Graph.
         */

        public void UpdateMasterGraphHelper(bool northArcOption, bool eastArcOption, bool southArcOption, bool westArcOption)
        {
            //System.Console.WriteLine("\nIn UpdateMasterGraphHelper()");

            _previousNode = _currentNode;

            if (_previousNode.WasSpecifiedArcOptionExplored(_directionTraveled) &&
                _masterGraphAlreadyIssuedCoordinates.Contains("[" + _xCoordinate + "," + _yCoordinate + "]"))
            {
                System.Console.WriteLine("Have traveled to an already known node along an already known path");

                int tempIndex = _masterGraphAlreadyIssuedCoordinates.IndexOf("[" + _xCoordinate + "," + _yCoordinate + "]");

                _currentNode = _masterGraph.GetNode(_nodeNamesList.ElementAt(tempIndex));
            }
            else if (_masterGraphAlreadyIssuedCoordinates.Contains("[" + _xCoordinate + "," + _yCoordinate + "]"))
            {
                System.Console.WriteLine("Have traveled to an already known node along a previously unknown path");

                int tempIndex = _masterGraphAlreadyIssuedCoordinates.IndexOf("[" + _xCoordinate + "," + _yCoordinate + "]");

                _currentNode = _masterGraph.GetNode(_nodeNamesList.ElementAt(tempIndex));

                _previousNode.SpecifiedArcOptionWasExplored(_directionTraveled);

                _masterGraph.AddArc(_previousNode, _directionTraveled, _currentNode);

                _currentNode.SpecifiedArcOptionWasExplored(Helper.GiveOppositeDirection(_directionTraveled));
            }
            else
            {
                System.Console.WriteLine("Have traveled to an unknown node");

                _previousNode.SpecifiedArcOptionWasExplored(_directionTraveled);

                _currentNode = new Node(_nodeNamesList.ElementAt(_masterGraphNodeNamesListCounter), northArcOption, eastArcOption, southArcOption, westArcOption);

                _currentNode.SpecifiedArcOptionWasExplored(Helper.GiveOppositeDirection(_directionTraveled));

                _currentNode.XCoordinate = _xCoordinate;

                _currentNode.YCoordinate = _yCoordinate;

                _masterGraphAlreadyIssuedCoordinates.Add("[" + _xCoordinate + "," + _yCoordinate + "]");

                _masterGraph.AddNode(_currentNode);

                _masterGraph.AddArc(_previousNode, _directionTraveled, _currentNode);

                _masterGraphNodeNamesListCounter += 1;
            }

            //System.Console.WriteLine("Leaving UpdateMasterGraphHelper()\n");
        }
    }
}
