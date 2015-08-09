using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
     * An instance of the AutonomousAgent class autonomously navigates a maze/graph. The AutonomousAgent gives instructions 
     * and recieves all information from an instance of an intermediate class. 
     */

    class AutonomousAgent
    {
        /*************************
         *  VARIABLES
         *************************/

        Intermediate _intermediate;

        GraphBuilder _graphBuilder;

        List<string> _pathFromStartToCurrentPosition;

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * AutonomousAgent constructor sets a passed instance of an intermediate class as the source of everything the agent will 
         * know of the maze/graph it is traversing and the destination for all of the agent's movement instructions. 
         */

        public AutonomousAgent(Intermediate intermediate)
        {
            //System.Console.WriteLine("\nIn AutonomousAgent()");

            this._intermediate = intermediate;

            _graphBuilder = new GraphBuilder();

            _pathFromStartToCurrentPosition = new List<string>();

            //System.Console.WriteLine("Leaving AutonomousAgent()");
        }

        /*************************
        *  METHODS
        *************************/

        /*
         * Traverse method starts the ManualAgents movement through the maze/graph. It uses the below methods ListNavigationOptions,
         * GetNavigationOptions, and ExecuteNavigationOptions to do this.
         */

        public void Traverse()
        {
            //System.Console.WriteLine("\nIn Traverse()");

            while (true)
            {
                if (FindTarget())
                {
                    bool northOption = _intermediate.CanGoNorth();

                    bool eastOption = _intermediate.CanGoEast();

                    bool southOption = _intermediate.CanGoSouth();

                    bool westOption = _intermediate.CanGoWest();

                    _graphBuilder.RecordAndConnectNode(northOption, eastOption, southOption, westOption);

                    System.Console.WriteLine("Target reached; Here's what was learned");

                    _graphBuilder.ListWhatWasLearnedThroughLastRun();

                    System.Console.WriteLine("Enter DONE to quit");

                    if (Console.ReadLine().Equals("DONE"))
                    {
                        System.Console.WriteLine("Ending traversal");

                        break;
                    }

                    System.Console.WriteLine("Starting new run of the same maze");

                    _intermediate.StartNewRun();

                    _graphBuilder.StartNewRun();

                    _pathFromStartToCurrentPosition = new List<string>();
                }
            }

            //System.Console.WriteLine("Leaving Traverse()\n");
        }

        /*
         * FindTarget begins the recursive process of searching for the target. It is not actually used recursively, but rather
         * acts as the first "set up" step which makes the recursive backtracking used in FindTargetHelper possible. It is
         * identical to FindTargetHelper in terms of code, except that it makes no intial or corrective movements of the agent, 
         * and calls the AvailableArcOptionsForCurrentNode with the directionTraveledFromPreviousNode parameter set to "none", thus
         * excluding none of it available arc options.
         */

        public bool FindTarget()
        {
            //System.Console.WriteLine("\nIn FindTarget()");

            System.Console.WriteLine("Path from starting point to current position: ");

            foreach (string element in _pathFromStartToCurrentPosition)
            {
                System.Console.Write(element + " - ");
            }

            System.Console.Write("Current position");

            System.Console.ReadLine();

            // Base Case

            if (_intermediate.TargetReached())
            {
                return true;
            }

            // All available arc options are recorded

            List<string> availableArcOptions = AvailableArcOptionsForCurrentNode("none");

            _pathFromStartToCurrentPosition.Add(_graphBuilder.CurrentNode.Name);

            List<string> possibleCurrentLocations = NodesAgentMightBeLocatedAtInMasterGraph();

            List<string> preferredArcOptions = new List<string>();

            foreach (string nodeName in possibleCurrentLocations)
            {
                System.Console.WriteLine("Finding out the direction the agent should go if it were at node " + nodeName);

                string suggestedDirection = FindShortestPathToTargetFromSpecifiedNode(nodeName);

                if (availableArcOptions.Contains(suggestedDirection))
                {
                    System.Console.WriteLine("Suggested direction is one the agent may procede in");

                    if (preferredArcOptions.Contains(suggestedDirection))
                    {
                        System.Console.WriteLine("... but direction is already present in the preferred are options list; Nothing done");
                    }
                    else
                    {
                        System.Console.WriteLine("Direction added to list of prefered arc options");

                        availableArcOptions.Remove(suggestedDirection);

                        preferredArcOptions.Add(suggestedDirection);
                    }

                }
                else
                {
                    System.Console.WriteLine("Suggested arc option was already added to preferred list and removed from available list.");
                }
            }

            while (preferredArcOptions.Count != 0 || availableArcOptions.Count != 0)
            {
                System.Console.WriteLine("Preferred arc options at current position include:");

                foreach (string option in preferredArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                System.Console.WriteLine("Remaining available arc options at current position include:");

                foreach (string option in availableArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                // Choosing of an arc option begins here

                string chosenArcOption;

                if (preferredArcOptions.Count != 0)
                {
                    chosenArcOption = preferredArcOptions.ElementAt(0);

                    preferredArcOptions.RemoveAt(0);
                }
                else
                {
                    if (availableArcOptions.Count == 1)
                    {
                        chosenArcOption = availableArcOptions.ElementAt(0);
                    }
                    else
                    {

                        int randomNumber = Helper.RandomNumberBetweenRange(0, availableArcOptions.Count);

                        chosenArcOption = availableArcOptions.ElementAt(randomNumber);
                    }

                    availableArcOptions.Remove(chosenArcOption);
                }

                System.Console.WriteLine("Arc option chosen: " + chosenArcOption);

                // Choosing of acr option done; The choice is now implemented starting here

                _pathFromStartToCurrentPosition.Add(chosenArcOption);

                System.Console.WriteLine("Preferred arc options not yet tried from this location include:");

                foreach (string option in preferredArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                System.Console.WriteLine("Available arc options not yet tried from this location include:");

                foreach (string option in availableArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                if (FindTargetHelper(chosenArcOption))
                {
                    return true;
                }

                // The choice has failed; A new choice will be made and tried
            }

            System.Console.WriteLine("Error. The agent tried all arc options available to it and was unable to find the target");

            return false;

            //System.Console.WriteLine("Leaving FindTarget()\n");
        }

        /*
         * FindTargetHelper searchesfor the target by means of recursive backtracking. Unlike FindTarget, it includes code
         * for making intial and corrective movements of the agent and calls the AvailableArcOptionsForCurrentNode with the 
         * directionTraveledFromPreviousNode parameter set to the value that was used to make the intial movement, thus excluding 
         * the option of going backwards as an option for moving forwards.
         */

        public bool FindTargetHelper(string directionToGo)
        {
            //System.Console.WriteLine("\nIn FindTargetHelper()");

            if (directionToGo.Equals("north"))
            {
                _intermediate.GoNorth();

                _graphBuilder.GoNorth();
            }
            else if (directionToGo.Equals("east"))
            {
                _intermediate.GoEast();

                _graphBuilder.GoEast();
            }
            else if (directionToGo.Equals("south"))
            {
                _intermediate.GoSouth();

                _graphBuilder.GoSouth();
            }
            else
            {
                _intermediate.GoWest();

                _graphBuilder.GoWest();
            }

            System.Console.WriteLine("Path from starting point to current position: ");

            foreach (string element in _pathFromStartToCurrentPosition)
            {
                System.Console.Write(element + " - ");
            }

            System.Console.Write("Current position");

            System.Console.ReadLine();

            if (_intermediate.TargetReached())
            {
                return true;
            }

            // All available arc options are recorded

            List<string> availableArcOptions = AvailableArcOptionsForCurrentNode(directionToGo);

            _pathFromStartToCurrentPosition.Add(_graphBuilder.CurrentNode.Name);

            List<string> possibleCurrentLocations = NodesAgentMightBeLocatedAtInMasterGraph();

            List<string> preferredArcOptions = new List<string>();

            foreach (string nodeName in possibleCurrentLocations)
            {
                System.Console.WriteLine("Finding out the direction the agent should go if it were at node " + nodeName);

                string suggestedDirection = FindShortestPathToTargetFromSpecifiedNode(nodeName);

                if (availableArcOptions.Contains(suggestedDirection))
                {
                    System.Console.WriteLine("Suggested direction is one the agent may procede in");

                    if (preferredArcOptions.Contains(suggestedDirection))
                    {
                        System.Console.WriteLine("... but direction is already present in the preferred are options list; Nothing done");
                    }
                    else
                    {
                        System.Console.WriteLine("Direction added to list of prefered arc options");

                        availableArcOptions.Remove(suggestedDirection);

                        preferredArcOptions.Add(suggestedDirection);
                    }

                }
                else
                {
                    System.Console.WriteLine("Suggested arc option was already added to preferred list and removed from available list or...\n... would take the agent back onto its own path and was ignored to keep search orderly and complete");
                }
            }

            while (preferredArcOptions.Count != 0 || availableArcOptions.Count != 0)
            {
                System.Console.WriteLine("Preferred arc options at current position include:");

                foreach (string option in preferredArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                System.Console.WriteLine("Remaining available arc options at current position include:");

                foreach (string option in availableArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                // Choosing of an arc option begins here

                string chosenArcOption;

                if (preferredArcOptions.Count != 0)
                {
                    chosenArcOption = preferredArcOptions.ElementAt(0);

                    preferredArcOptions.RemoveAt(0);
                }
                else
                {
                    if (availableArcOptions.Count == 1)
                    {
                        chosenArcOption = availableArcOptions.ElementAt(0);
                    }
                    else
                    {

                        int randomNumber = Helper.RandomNumberBetweenRange(0, availableArcOptions.Count);

                        chosenArcOption = availableArcOptions.ElementAt(randomNumber);
                    }

                    availableArcOptions.Remove(chosenArcOption);
                }

                System.Console.WriteLine("Arc option chosen: " + chosenArcOption);

                // Choosing of acr option done; The choice is now implemented starting here

                _pathFromStartToCurrentPosition.Add(chosenArcOption);

                System.Console.WriteLine("Preferred arc options not yet tried from this location include:");

                foreach (string option in preferredArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                System.Console.WriteLine("Available arc options not yet tried from this location include:");

                foreach (string option in availableArcOptions)
                {
                    System.Console.WriteLine(option);
                }

                if (FindTargetHelper(chosenArcOption))
                {
                    return true;
                }

                // The choice has failed; A new choice will be made and tried
            }

            System.Console.WriteLine("All options exhausted; Backtracking " + Helper.GiveOppositeDirection(directionToGo));

            if (directionToGo.Equals("north"))
            {
                _intermediate.GoSouth();

                _graphBuilder.GoSouth();

                _pathFromStartToCurrentPosition.Add("south");
            }
            else if (directionToGo.Equals("east"))
            {
                _intermediate.GoWest();

                _graphBuilder.GoWest();

                _pathFromStartToCurrentPosition.Add("west");
            }
            else if (directionToGo.Equals("south"))
            {
                _intermediate.GoNorth();

                _graphBuilder.GoNorth();

                _pathFromStartToCurrentPosition.Add("north");
            }
            else
            {
                _intermediate.GoEast();

                _graphBuilder.GoEast();

                _pathFromStartToCurrentPosition.Add("east");
            }

            /*
             * A list of available arc options is not needed at this point, but this method should be called so that the 
             * _graphbuilder can update what it knows as the previous node. If this method is not called, the program will
             * still work fine; however, some of the debugging output will be nonsensical as the _graphbuilder runs checks
             * to see if it can create arcs between the node it just arrived at and the node from which it first started 
             * backtracking, which may be several arc taversals back, which invariably return false.
             */

            AvailableArcOptionsForCurrentNode("backtracking");

            _pathFromStartToCurrentPosition.Add(_graphBuilder.CurrentNode.Name);

            return false;

            //System.Console.WriteLine("Leaving FindTargetHelper()\n");
        }

        /*
         * AvailableArcOptionsForCurrentNode returns a list of all possible arc options the agent may go in order to move forward.
         * In order to make this list, the methods takes the direction that was traveled form the previous node (e.g. "north")
         * and excludes the opposite of that direction from that list, as that direction would take the agent backwards, to the
         * node it just came from. The method compiles its list of available arc options by asking the intermediate which otpions
         * exist.
         */

        public List<string> AvailableArcOptionsForCurrentNode(string directionTraveledFromPreviousNode)
        {
            //System.Console.WriteLine("\nIn AvailableArcOptionsForCurrentNode()");

            bool northOption = _intermediate.CanGoNorth();

            bool eastOption = _intermediate.CanGoEast();

            bool southOption = _intermediate.CanGoSouth();

            bool westOption = _intermediate.CanGoWest();

            _graphBuilder.RecordAndConnectNode(northOption, eastOption, southOption, westOption);

            List<string> arcOptions = new List<string>();

            if (!directionTraveledFromPreviousNode.Equals("backtracking"))
            {
                string arcOptionLeadingBackwards;

                if (!directionTraveledFromPreviousNode.Equals("none"))
                {
                    arcOptionLeadingBackwards = Helper.GiveOppositeDirection(directionTraveledFromPreviousNode);
                }
                else
                {
                    arcOptionLeadingBackwards = directionTraveledFromPreviousNode;
                }

                List<string> arcOptionsToExclude = ArcOptionsToExcludeBecauseTheyLoopAgentBackOntoItsOwnPath();

                arcOptionsToExclude.Add(arcOptionLeadingBackwards);

                if (northOption && !arcOptionsToExclude.Contains("north"))
                {
                    arcOptions.Add("north");
                }

                if (eastOption && !arcOptionsToExclude.Contains("east"))
                {
                    arcOptions.Add("east");
                }

                if (southOption && !arcOptionsToExclude.Contains("south"))
                {
                    arcOptions.Add("south");
                }

                if (westOption && !arcOptionsToExclude.Contains("west"))
                {
                    arcOptions.Add("west");
                }
            }

            return arcOptions;

            //System.Console.WriteLine("Leaving AvailableArcOptionsForCurrentNode()\n");
        }

        /*
         * ArcOptionsToExcludeBecauseTheyLoopAgentBackOntoItsOwnPath uses the x/y coordinate scheme to check if the 
         * x/y cooridiates of the current nodes neighbors to the north, east, south, and west have been added to list 
         * of already assigned x/y coordinates recorded by the graph builder class. If they have been, the method adds
         * the direction of that neighbor to a list that will be returned and used to exculded that arc option from the
         * agents available arc options.
         */

        public List<string> ArcOptionsToExcludeBecauseTheyLoopAgentBackOntoItsOwnPath()
        {
            //System.Console.WriteLine("\nIn ArcOptionsToExcludeBecauseTheyLoopAgentBackOntoItsOwnPath()");

            List<string> arcOptionsToExlude = new List<string>();

            int xCoordinateOfCurrentNodeInGraphBuilder = _graphBuilder.XCoordinate;

            int yCoordinateOfCurrentNodeInGraphBuilder = _graphBuilder.YCoordinate;

            string xYCoordinateOfNeighborToTheNorth = "[" + xCoordinateOfCurrentNodeInGraphBuilder + ","
                                                          + (yCoordinateOfCurrentNodeInGraphBuilder + 1) + "]";

            string xYCoordinateOfNeighborToTheEast = "[" + (xCoordinateOfCurrentNodeInGraphBuilder + 1)
                                                         + "," + yCoordinateOfCurrentNodeInGraphBuilder + "]";

            string xYCoordinateOfNeighborToTheSouth = "[" + xCoordinateOfCurrentNodeInGraphBuilder + ","
                                                          + (yCoordinateOfCurrentNodeInGraphBuilder - 1) + "]";

            string xYCoordinateOfNeighborToTheWest = "[" + (xCoordinateOfCurrentNodeInGraphBuilder - 1) + ","
                                                         + yCoordinateOfCurrentNodeInGraphBuilder + "]";

            if (_graphBuilder.CurrentNode.NorthArcOptionExists && _graphBuilder.AleardyIssuedCoordinatess.Contains(xYCoordinateOfNeighborToTheNorth))
            {
                arcOptionsToExlude.Add("north");

                CreateALoopByConnectingCurrentNodeWithItsNeighbor("north", xYCoordinateOfNeighborToTheNorth);
            }

            if (_graphBuilder.CurrentNode.EastArcOptionExists && _graphBuilder.AleardyIssuedCoordinatess.Contains(xYCoordinateOfNeighborToTheEast))
            {
                arcOptionsToExlude.Add("east");

                CreateALoopByConnectingCurrentNodeWithItsNeighbor("east", xYCoordinateOfNeighborToTheEast);
            }

            if (_graphBuilder.CurrentNode.SouthArcOptionExists && _graphBuilder.AleardyIssuedCoordinatess.Contains(xYCoordinateOfNeighborToTheSouth))
            {
                arcOptionsToExlude.Add("south");

                CreateALoopByConnectingCurrentNodeWithItsNeighbor("south", xYCoordinateOfNeighborToTheSouth);
            }

            if (_graphBuilder.CurrentNode.WestArcOptionExists && _graphBuilder.AleardyIssuedCoordinatess.Contains(xYCoordinateOfNeighborToTheWest))
            {
                arcOptionsToExlude.Add("west");

                CreateALoopByConnectingCurrentNodeWithItsNeighbor("west", xYCoordinateOfNeighborToTheWest);
            }

            //System.Console.WriteLine("Leaving ArcOptionsToExcludeBecauseTheyLoopAgentBackOntoItsOwnPath()\n");

            return arcOptionsToExlude;
        }

        /*
         * CreateALoopByConnectingCurrentNodeWithItsNeighbor is used to connect the node the agent is currently at with a neighboring
         * node already recorded as being part of the agent's path, without having to actually travel to that node. This makes it
         * possible for the agent to record loops that are present in the graph without the need to actually travel them. 
         */

        public void CreateALoopByConnectingCurrentNodeWithItsNeighbor(string directionOfNeighbor, string xYCoordinateOfNeighboringNode)
        {
            //System.Console.WriteLine("\nIn CreateALoopByConnectingCurrentNodeWithItsNeighbor()");

            int indexCounter = 0;

            foreach (string xYCoordinate in _graphBuilder.AleardyIssuedCoordinatess)
            {
                if (xYCoordinate.Equals(xYCoordinateOfNeighboringNode))
                {
                    break;
                }

                indexCounter++;
            }

            string nameOfNeighboringNode = _graphBuilder.NodeNameList.ElementAt(indexCounter);

            Node neighboringNode = _graphBuilder.CurrentGraph.GetNode(nameOfNeighboringNode);

            if (neighboringNode.NodeArcOptionIsOpen(Helper.GiveOppositeDirection(directionOfNeighbor)) &&
                _graphBuilder.CurrentNode.NodeArcOptionIsOpen(directionOfNeighbor))
            {
                System.Console.WriteLine("Arc between nodes " + _graphBuilder.CurrentNode.Name + " and " +
                                         neighboringNode.Name + " resulting in a loop in the current run's graph recorded");

                _graphBuilder.CurrentGraph.AddArc(_graphBuilder.CurrentNode, directionOfNeighbor, neighboringNode);

                _graphBuilder.CurrentNode.SpecifiedArcOptionWasExplored(directionOfNeighbor);

                neighboringNode.SpecifiedArcOptionWasExplored(Helper.GiveOppositeDirection(directionOfNeighbor));

                /*
                 * As GraphBuilder's master graph updates itself after the first run by using the path the agent actually travelled,
                 * not the state of the current graph, the arc that was just recorded will not be added to the master graph unless 
                 * the path of the agent as seen by the GraphBuilder is ammended to appear as if the agent acutally travelled to the
                 * neighboring node from the current node, and then back.
                 */

                // Simulating going to the neighboring node
                _graphBuilder.DirectionsTraveledInCurrentRun.Add(directionOfNeighbor);

                // Recording the description of the neighboring node
                _graphBuilder.DescriptionsOfNodesUsedInCurrentRun.Add(neighboringNode.ArcOptions);

                // Simulating coming back to the current node
                _graphBuilder.DirectionsTraveledInCurrentRun.Add(Helper.GiveOppositeDirection(directionOfNeighbor));

                // Recoding the decription of the current node, as if the agent just arrived at it
                _graphBuilder.DescriptionsOfNodesUsedInCurrentRun.Add(_graphBuilder.CurrentNode.ArcOptions);
            }

            //System.Console.WriteLine("Leaving CreateALoopByConnectingCurrentNodeWithItsNeighbor()\n");
        }

        /*
         * NodesAgentMightBeLocatedAtInMasterGraph compiles a list of nodes present in the graph builder's master graph that match the 
         * characteristics of the node the agent is currently located at in the graph builder's current run graph. The returned list is
         * sorted, with the highest scoring matches appearing first.
         */

        public List<string> NodesAgentMightBeLocatedAtInMasterGraph()
        {
            //System.Console.WriteLine("\nIn NodesAgentMightBeLocatedAtInMasterGraph()");

            List<string> listOfAllPossibleNodesAgentMayBeLocatedAt = new List<string>();

            System.Console.WriteLine("The current node's description matches that of the following nodes in the master graph: ");

            foreach (Node node in _graphBuilder.MasterGraph.NodesSet)
            {
                if (_graphBuilder.CurrentNode.ArcOptions.Equals(node.ArcOptions) && !node.IsTarget) // node can't be target b/c it was just checked in FindTargetHelper()
                {
                    System.Console.Write(node.Name + " - ");

                    listOfAllPossibleNodesAgentMayBeLocatedAt.Add(node.Name);
                }
            }

            System.Console.Write("End of list\n");

            int counter = 0;

            while (counter < listOfAllPossibleNodesAgentMayBeLocatedAt.Count)
            {
                int score = NodesAgentMightBeLocatedAtInMasterGraphHelper(listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(counter), _pathFromStartToCurrentPosition, 0);

                System.Console.WriteLine("A score of " + score + " was returned for " + listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(counter));

                if (score > 0)
                {
                    listOfAllPossibleNodesAgentMayBeLocatedAt[counter] = score + "-" + listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(counter);

                    System.Console.WriteLine("Item is now " + listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(counter));

                    counter++;
                }
                else
                {
                    listOfAllPossibleNodesAgentMayBeLocatedAt.RemoveAt(counter);

                    System.Console.WriteLine("Item was deleted for having a score of 0");
                }

            }

            listOfAllPossibleNodesAgentMayBeLocatedAt.Sort();

            listOfAllPossibleNodesAgentMayBeLocatedAt.Reverse();

            System.Console.Write("Ordered list looks like... ");

            for (int i = 0; i < listOfAllPossibleNodesAgentMayBeLocatedAt.Count; i++)
            {
                int delineator = listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(i).IndexOf("-");

                listOfAllPossibleNodesAgentMayBeLocatedAt[i] = listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(i).Substring(delineator + 1);

                System.Console.Write(listOfAllPossibleNodesAgentMayBeLocatedAt.ElementAt(i) + " - ");
            }

            System.Console.Write("End of list\n");

            //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraph()\n");

            return listOfAllPossibleNodesAgentMayBeLocatedAt;
        }

        /*
         * NodesAgentMightBeLocatedAtInMasterGraphHelper compares the characteristics of a given node in the graph builder's master 
         * graph with the characterists of the last node recorded in the agent's current path. If they match, the method uses the 
         * current path to determine the direction in which an adjacent, matching node in the graph builder's master graph should be 
         * located. It calls itself recursively with the name of that adjecent node and a copy of the current path with the last node 
         * and direction travelled deleted. It continues until the the characteristics of the nodes do not match (returning a score of 0
         * for the orginal node), or either the master graph or the current path run out of nodes to compare (returning a score of
         * however many nodes were matched for the original node).
         * 
         */

        public int NodesAgentMightBeLocatedAtInMasterGraphHelper(string nodeNameFromMasterGraph, List<string> pathBackToStart, int numberOfMatchingNodesFound)
        {
            //System.Console.WriteLine("\nIn NodesAgentMightBeLocatedAtInMasterGraphHelper()");

            List<string> copyOfPath = new List<string>(pathBackToStart);

            Node nodeFromCurrentGraph = _graphBuilder.CurrentGraph.GetNode(copyOfPath.ElementAt(copyOfPath.Count - 1));

            copyOfPath.RemoveAt(copyOfPath.Count - 1);

            Node nodeFromMasterGraph = _graphBuilder.MasterGraph.GetNode(nodeNameFromMasterGraph);

            if (copyOfPath.Count == 0)
            {
                if (nodeFromMasterGraph.ArcOptions.Equals(nodeFromCurrentGraph.ArcOptions))
                {
                    System.Console.WriteLine("Have reached start of path; Returning that agent POSSIBLY knows where it is");

                    //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraphHelper()\n");

                    return (numberOfMatchingNodesFound + 1);
                }
                else
                {
                    System.Console.WriteLine("They do NOT match; Returning that agent does NOT know where it is");

                    //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraphHelper()\n");

                    return 0;
                }
            }

            string lastDirectionTravelledReversed = Helper.GiveOppositeDirection(copyOfPath.ElementAt(copyOfPath.Count - 1));

            copyOfPath.RemoveAt(copyOfPath.Count - 1);

            System.Console.WriteLine("Checking if " +
                                    nodeFromMasterGraph.Name + " from the MG has the same arc options as " +
                                    nodeFromCurrentGraph.Name + " from the CG");

            if (nodeFromMasterGraph.ArcOptions.Equals(nodeFromCurrentGraph.ArcOptions))
            {
                System.Console.WriteLine("They match; Checking if MG node " + nodeFromMasterGraph.Name + "'s " + lastDirectionTravelledReversed + " arc option has been explored");

                int newNumberOfMatchingNodesFound = (numberOfMatchingNodesFound + 1);

                if (nodeFromMasterGraph.WasSpecifiedArcOptionExplored(lastDirectionTravelledReversed))
                {
                    System.Console.WriteLine("It was; seeing if it is possible to backtrack MG using CG path");

                    string nextNodeToCheck = nodeFromMasterGraph.NeighboringNodeToSpecifiedDirection(lastDirectionTravelledReversed).Name;

                    //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraphHelper()\n");

                    return NodesAgentMightBeLocatedAtInMasterGraphHelper(nextNodeToCheck, copyOfPath, newNumberOfMatchingNodesFound);
                }
                else
                {
                    System.Console.WriteLine("It was not; Returning that agent POSSIBLY knows where it is");

                    //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraphHelper()\n");

                    return newNumberOfMatchingNodesFound;
                }
            }
            else
            {
                System.Console.WriteLine("They do NOT match; Returning that agent does NOT know where it is");
            }

            //System.Console.WriteLine("Leaving NodesAgentMightBeLocatedAtInMasterGraphHelper()\n");

            return 0;
        }

        /*
         * FindShortestPathToTargetFromSpecifiedNode uses breadth first search pattern to find the shortest path from a given node to the
         * target node in the master graph built by the graph builder class. Once determined, it returns the first direction that must
         * be selected in order to take that path. Becasue it is impossible for the given node name to be that of the target node, 
         * and because the cost of each arc is equal, this method can checks if a reach node is the target BEFORE dequeueing it from
         * the list of current options, in order to perform its task faster. If either of these facts where not true, the method would
         * instead need to check if the node was the target only after it had been dequeued.
         */

        public string FindShortestPathToTargetFromSpecifiedNode(string nodeName)
        {
            //System.Console.WriteLine("\nIn FindShortestPathToTargetFromSpecifiedNode()");

            Queue<string> nodesToTry = new Queue<string>();

            List<string> namesOfNodesBeingOrAlreadyLookedAt = new List<string>();

            Dictionary<string, List<string>> pathsToTargetFromEveryNode = new Dictionary<string, List<string>>();

            nodesToTry.Enqueue(nodeName);

            namesOfNodesBeingOrAlreadyLookedAt.Add(nodeName);

            pathsToTargetFromEveryNode[nodeName] = new List<string>() { nodeName };

            while (nodesToTry.Count != 0)
            {
                /* FOR DEBUGGING PURPOSES
                
                System.Console.WriteLine("Queue includes the following nodes: ");

                foreach (string nameOfNode in nodesToTry)
                {
                    System.Console.Write(nameOfNode + " - ");
                }

                System.Console.Write("END of queue contents\n");

                System.Console.WriteLine("Dictionary includes the following paths: ");

                foreach (KeyValuePair<string, List<string>> kvp in pathsToTargetFromEveryNode)
                {
                    Console.Write(kvp.Key + ": ");

                    foreach (string element in kvp.Value)
                    {
                        Console.Write(element + " - ");
                    }
                    Console.Write("END of path\n");
                }
                
                */

                string nameOfNodeBeingLookedAt = nodesToTry.Dequeue();

                System.Console.WriteLine("Node " + nameOfNodeBeingLookedAt + " dequeued; Looking for its neighbors");

                /* THIS IS WHERE CHECKING IF THE NODE WAS THE TARGET WOULD TYPICALLY TAKE PLACE IN A BFS
                
                System.Console.WriteLine("Checking if node " + nameOfNodeBeingLookedAt + " is the target node");
                 
                if (_graphBuilder.MasterGraph.GetNode(nameOfNodeBeingLookedAt).IsTarget)
                {
                    System.Console.WriteLine(nameOfNodeBeingLookedAt + " is the target node");

                    List<string> shortestPath = pathsToTargetFromEveryNode[nameOfNodeBeingLookedAt];

                    string directionToGo = shortestPath.ElementAt(1);

                    System.Console.WriteLine("The shortest path from to the target is...");

                    foreach (string element in shortestPath)
                    {
                        System.Console.Write(element + " - ");
                    }

                    System.Console.Write(" Target Reached\nThe agent will be told it should go " + directionToGo + "\n");

                    //System.Console.WriteLine("Leaving FindShortestPathToTargetFromSpecifiedNode()\n");

                    return directionToGo;
                }
                
                System.Console.WriteLine(nameOfNodeBeingLookedAt + " is NOT the target node");

                */

                List<string> possibleDirections = new List<string>();

                Node nodeBeingLookedAt = _graphBuilder.MasterGraph.GetNode(nameOfNodeBeingLookedAt);

                if (nodeBeingLookedAt.NorthArcOptionExists && nodeBeingLookedAt.NorthArcOptionIsExplored)
                {
                    possibleDirections.Add("north");
                }

                if (nodeBeingLookedAt.EastArcOptionExists && nodeBeingLookedAt.EastArcOptionIsExplored)
                {
                    possibleDirections.Add("east");
                }

                if (nodeBeingLookedAt.SouthArcOptionExists && nodeBeingLookedAt.SouthArcOptionIsExplored)
                {
                    possibleDirections.Add("south");
                }

                if (nodeBeingLookedAt.WestArcOptionExists && nodeBeingLookedAt.WestArcOptionIsExplored)
                {
                    possibleDirections.Add("west");
                }

                foreach (string direction in possibleDirections)
                {
                    Node neighborthingNode = _graphBuilder.MasterGraph.GetNode(nameOfNodeBeingLookedAt).NeighboringNodeToSpecifiedDirection(direction);

                    string neighboringNodeName = neighborthingNode.Name;

                    System.Console.WriteLine(neighboringNodeName + " is a neighbor of " + nameOfNodeBeingLookedAt);

                    if (!namesOfNodesBeingOrAlreadyLookedAt.Contains(neighboringNodeName))
                    {
                        List<string> pathToNode = new List<string>(pathsToTargetFromEveryNode[nameOfNodeBeingLookedAt]);

                        pathToNode.Add(direction);

                        pathToNode.Add(neighboringNodeName);

                        pathsToTargetFromEveryNode[neighboringNodeName] = pathToNode;

                        // NEIGHBOR IS CHECKED HERE IF IT IS THE TARGET AND THE IF SO, THE SEARCH CAN STOP BEFORE IT IS EVEN QUEUED

                        if (_graphBuilder.MasterGraph.GetNode(neighboringNodeName).IsTarget)
                        {
                            System.Console.WriteLine(neighboringNodeName + " is the target node");

                            List<string> shortestPath = pathsToTargetFromEveryNode[neighboringNodeName];

                            string directionToGo = shortestPath.ElementAt(1);

                            System.Console.WriteLine("The shortest path from to the target is...");

                            foreach (string element in shortestPath)
                            {
                                System.Console.Write(element + " - ");
                            }

                            System.Console.Write(" Target Reached\nThe agent will be told it should go " + directionToGo + "\n");

                            //System.Console.WriteLine("Leaving FindShortestPathToTargetFromSpecifiedNode()\n");

                            return directionToGo;
                        }

                        System.Console.WriteLine(neighboringNodeName + " is NOT the target node");

                        namesOfNodesBeingOrAlreadyLookedAt.Add(neighboringNodeName);

                        nodesToTry.Enqueue(neighboringNodeName);

                        System.Console.WriteLine("Node " + neighboringNodeName + " added to the queue and list of already or being looked at nodes");
                    }
                    else
                    {
                        System.Console.WriteLine("... but " + neighboringNodeName + " is already known of");
                    }
                }
            }

            //System.Console.WriteLine("Leaving FindShortestPathToTargetFromSpecifiedNode()\n");

            return "error"; // This should never be returned
        }
    }
}
