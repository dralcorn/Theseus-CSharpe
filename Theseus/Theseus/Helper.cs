using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theseus
{
    /*
      * The Helper class is a static class populated with utility methods used by the methods of other classes.
      */

    static class Helper
    {
        /*
         * GiveOppositeDirection returns the logically opposite direction of a given direction. It is used by the 
         * GraphBuilber.RecordAndConnect, GraphBuilser.UpdateMasterGraphHelper, and Graph.AddArc functions 
         */

        public static string GiveOppositeDirection(string direction)
        {
            //System.Console.WriteLine("\nIn GiveOppositeDirection()");

            if (direction.Equals("north"))
            {
                //System.Console.WriteLine("Leaving GiveOppositeDirection()\n");

                return "south";
            }

            if (direction.Equals("east"))
            {
                //System.Console.WriteLine("Leaving GiveOppositeDirection()\n");

                return "west";
            }

            if (direction.Equals("south"))
            {
                //System.Console.WriteLine("Leaving GiveOppositeDirection()\n");

                return "north";
            }

            if (direction.Equals("west"))
            {
                //System.Console.WriteLine("Leaving GiveOppositeDirection()\n");

                return "east";
            }

            System.Console.WriteLine("Error; Invalid input");

            //System.Console.WriteLine("Leaving GiveOppositeDirection()\n");

            return direction;
        }

        /*
         * RandomNumberBetweenRange returns a number between a provided range. The lower bound is included in the numbers that may
         * be returned, while the upperbound is not. GraphNavigatorIntermediate.StartNewRun, AutonomousAgent.FindTarget and 
         * AutonomousAgent.FindTargetHelper use this method.
         */

        public static int RandomNumberBetweenRange(int lowerBound_included, int upperBound_exluded)
        {
            //System.Console.WriteLine("\nIn RandomNumberBetweenRange()");

            Random random = new Random();

            int randomNumber = random.Next(lowerBound_included, upperBound_exluded);

            //System.Console.WriteLine("Leaving RandomNumberBetweenRange()\n");

            return randomNumber;
        }

        /*
         * ConvertNodeArcOptionsStringToListOfStrings takes the short string that denotes a node's arc options (e.g. "NW")
         * and converts that string to a list of strings that denote the node's arc options (e.g. "north", "west"). The list
         * is then returned.
         */

        public static List<string> ConvertNodeArcOptionsStringToListOfStrings(string arcOptions)
        {
            //System.Console.WriteLine("\nIn ConvertNodeArcOptionsStringToListOfStrings()");

            List<string> listOfArcOptions = new List<string>();

            if (arcOptions.Contains("N"))
            {
                listOfArcOptions.Add("north");
            }

            if (arcOptions.Contains("E"))
            {
                listOfArcOptions.Add("east");
            }

            if (arcOptions.Contains("S"))
            {
                listOfArcOptions.Add("south");
            }

            if (arcOptions.Contains("W"))
            {
                listOfArcOptions.Add("west");
            }

            //System.Console.WriteLine("Leaving ConvertNodeArcOptionsStringToListOfStrings()\n");

            return listOfArcOptions;
        }
    }
}
