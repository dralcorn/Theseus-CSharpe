using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
     * An instance of the ManualAgent class provides a user with the means of navigating a maze/graph by listing all available moves,
     * recording a users choice, and implementing that move. The ManualAgent gives instructions and recieves all information from
     * an instance of an intermediate class. 
     */

    class ManualAgent
    {
        /*************************
         *  VARIABLES
         *************************/

        Intermediate _intermediate;

        GraphBuilder _graphBuilder;

        /*************************
         *  CONSTRUCTOR(S)
         *************************/

        /*
         * ManualAgent constructor sets a passed instance of an intermediate class as the source of everything the agent will 
         * know of the maze/graph it is traversing and the destination for all of the agent's movement instructions. 
         */

        public ManualAgent(Intermediate intermediate)
        {
            //System.Console.WriteLine("\nIn ManualAgent()");

            this._intermediate = intermediate;

            _graphBuilder = new GraphBuilder();

            //System.Console.WriteLine("Leaving ManualAgent()");
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
                if (_intermediate.TargetReached())
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
                }

                ListNavigationOptions();

                ExecuteNavigationOption(GetNavigationOption());
            }

            //System.Console.WriteLine("Leaving Traverse()\n");
        }

        /*
         * ListNavigationOptions provides the user with a list of traversable arc options present at the current location 
         * (i.e. node or intersection) of the agent. It lists the options according to what it told by the intermediate, 
         * not the maze/graph directly.
         */

        public void ListNavigationOptions()
        {
            //System.Console.WriteLine("\nIn ListNavigationOptions()");

            System.Console.WriteLine("Options include...");

            bool northOption = _intermediate.CanGoNorth();

            bool eastOption = _intermediate.CanGoEast();

            bool southOption = _intermediate.CanGoSouth();

            bool westOption = _intermediate.CanGoWest();


            if (northOption)
            {
                System.Console.WriteLine("north");
            }

            if (eastOption)
            {
                System.Console.WriteLine("east");
            }

            if (southOption)
            {
                System.Console.WriteLine("south");
            }

            if (westOption)
            {
                System.Console.WriteLine("west");
            }

            _graphBuilder.RecordAndConnectNode(northOption, eastOption, southOption, westOption);

            //System.Console.WriteLine("\nLeaving ListNavigationOptions()\n");
        }

        /*
         * GetNavigationOptions takes and passes along the user's arc option choice. 
         */

        public string GetNavigationOption()
        {
            //System.Console.WriteLine("\nIn GetNavigationOption()");

            string chosenOption;

            while (true)
            {

                chosenOption = Console.ReadLine();

                if ((chosenOption.Equals("north") && _intermediate.CanGoNorth()) ||
                    (chosenOption.Equals("east") && _intermediate.CanGoEast()) ||
                    (chosenOption.Equals("south") && _intermediate.CanGoSouth()) ||
                    (chosenOption.Equals("west") && _intermediate.CanGoWest()))
                {
                    System.Console.WriteLine("Option " + chosenOption + " selected");

                    break;
                }

                System.Console.WriteLine("Input not recognized; Try again");
            }

            //System.Console.WriteLine("\nLeaving GetNavigationOption()\n");

            return chosenOption;
        }

        /*
         * ExecuteNavigationOption tells the intermediate the ar option choice that was selected in GetNavigationOption.
         */

        public void ExecuteNavigationOption(string chosenOption)
        {
            //System.Console.WriteLine("\nIn ExecuteNavigationOption()");

            if (chosenOption.Equals("north"))
            {
                _intermediate.GoNorth();

                _graphBuilder.GoNorth();
            }

            if (chosenOption.Equals("east"))
            {
                _intermediate.GoEast();

                _graphBuilder.GoEast();
            }

            if (chosenOption.Equals("south"))
            {
                _intermediate.GoSouth();

                _graphBuilder.GoSouth();
            }

            if (chosenOption.Equals("west"))
            {
                _intermediate.GoWest();

                _graphBuilder.GoWest();
            }

            //System.Console.WriteLine("\nLeaving ExecuteNavigationOption()\n");
        }

    }
}
