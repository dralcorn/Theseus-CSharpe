using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
     * The Intermediate interface is implemented by both the GraphNavigatorIntermediate and MazeNavigatorIntermediate in order to
     * ensure that both can be used by either the ManualAgent or AutonomousAgent classes.
     */

    interface Intermediate
    {
        /*
         * CanGoNorth returns true if the agent can navigate north, false if it cannot.
         */

        bool CanGoNorth();

        /*
         * CanGoEast returns true if the agent can navigate east, false if it cannot.
         */

        bool CanGoEast();

        /*
         * CanGoSouth returns true if the agent can navigate south, false if it cannot.
         */

        bool CanGoSouth();

        /*
         * CanGoWest returns true if the agent can navigate west, false if it cannot.
         */

        bool CanGoWest();

        /*
         * GoNorth is used by the agent class to tell the intermediate to navigate north.
         */

        void GoNorth();

        /*
         * GoEast is used by the agent class to tell the intermediate to navigate east.
         */

        void GoEast();

        /*
         * GoSouth is used by the agent class to tell the intermediate to navigate south.
         */

        void GoSouth();

        /*
         * GoWest is used by the agent class to tell the intermediate to navigate west.
         */

        void GoWest();

        /*
         * TargetReached returns true if the target of the maze/graph being traversed by the agent has been reached, false
         * if it has not.
         */

        bool TargetReached();

        /*
         * StartNewRun prepares the intermediate for a new run by aiding in setting a new (in concept) starting node 
         * (although the node is reaaly just a current node that is different than the target location/node reached 
         * in the last run of the maze).
         */

        void StartNewRun();

        /*
         * Orient is used to orient the agent. In a graph/virtual setting, this method is not necessary. In a maze/physical 
         * setting this method may call a compass sensor or use a pattern of directional movements to orient the agent so that 
         * the directions north, south, east, and west are known.
         */

        void Orient();
    }
}
