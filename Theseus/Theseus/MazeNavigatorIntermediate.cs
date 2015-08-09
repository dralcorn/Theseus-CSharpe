using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace Theseus
{
    /*
        * An instance of the MazeNavigatorIntermediate class is used by an instance of the ManualAgent or AutonomousAgent 
        * classes to travese a physical maze using a Lego EV3 line-follower robot. It interprets what options for 
        * movemment are available given the robots's current position in the maze  and relays that information in a 
        * simplified format to the agent. 
        * 
        * When the agent chooses a move, the MazeNavidatorIntermediate implements that move.
        *
        * The MazeNavidatorIntermediate relays very simple signals/instructions between the agent class and the robot's 
        * sensors and motors.  
        * 
        * The robot show start each run on the arc leading to the node which will be its starting node, not on the node 
        * itself.
        */

    class MazeNavigatorIntermediate : Intermediate
    {
        /*************************
         *  VARIABLES
         *************************/
        Brick _brick;

        int _forwardPower = 50; // 50% of full power forwards

        int _backwardPower = -50; // 50% of full power in reverse

        uint _timeInWhichPowerWillBeApplied = 300; // 300 miliseconds

        double tp = 25; // target power (25%)

        double kp; // power constant

        double ki; // integral constant

        double kd; // derivative constant

        double integral;

        double derivative;
        
        double currentError;

        double previousError;

        double offset;

        double turn;

        double rightMotorPower;

        double leftMotorPower;

        double lightColorSIValue;

        double darkColorSIValue;

        double whiteSIValue;

        string _orientation;

        bool goingEastOrSouth = false;

        bool targetReached = false;

        bool rightArcOptionExists = false;

        bool leftArcOptionExists = false;

        bool forwardArcOptionExists = false;

        bool traversingArc; 

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
         * MazeNavigatorIntermediate constructor takes a SI value for a dark color, a SI value for a light color, a SI 
         * value for white, and the initial 
         * orientation ("north", "east", "south", or "west"). The light and dark color values will be used to move 
         * straight along the length of the arcs, and the orientation is used to determine how the the robot should move
         * ("pivot right 90 degrees", "pivot left 90 degrees", "forwards", or "pivot 180 degrees") in order choose a
         * particular arc option. 
         */

        public MazeNavigatorIntermediate(double darkValue, double lightValue, double whiteValue, string startingOrientation)
        {
            //System.Console.WriteLine("\nIn MaveNavigatorIntermediate()");

            darkColorSIValue = darkValue;

            lightColorSIValue = lightValue;

            whiteSIValue = whiteValue;

            _orientation = startingOrientation;

            offset = ((lightColorSIValue + darkColorSIValue) / 2);

            double dt = .02; // delta time for each loop; guessed .02 seconds per loop

            double pc = .5; // time it takes the robot to ocisllate from right to left to right again; guessed .5 seconds 

            kp = (0 - tp) / ((-1 * (lightColorSIValue - offset)) - 0) * .6;

            ki = (kp * 2 * dt) / pc;

            kd = (kp * pc) / 8 * dt;

            integral = 0;

            previousError = 0;

            derivative = 0;

            SetUpBrick();

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving MaveNavigatorIntermediate()\n");
        }

        /*************************
        *  METHODS
        *************************/

        /*
         * SetUpBrick prepares the Lego brick to send and reveive signals. 
         */

        private async void SetUpBrick()
        {
            //System.Console.WriteLine("\nIn SetUpBrick()");

            _brick = new Brick(new BluetoothCommunication("COM4"));

            _brick.BrickChanged += _brick_BrickChanged;

            await _brick.ConnectAsync();
            
            await _brick.DirectCommand.PlayToneAsync(100, 1000, 300);
            
            await _brick.DirectCommand.SetMotorPolarity(OutputPort.B | OutputPort.C, Polarity.Forward);
            
            await _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
            
            _brick.Ports[InputPort.One].SetMode(ColorMode.Reflective);

            //System.Console.WriteLine("Leaving SetUpBrick()\n");
        }

        /*
         * _brick_BrickChanged is an event handler that will fire when ever the brick's color sensor reveives new data. 
         */

        void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            //System.Console.WriteLine("\nIn MaveNavigatorIntermediate()");

            double lightSensorValue = e.Ports[InputPort.One].SIValue;

            if (traversingArc)
            {
                if (lightSensorValue < darkColorSIValue) // black in current 
                {
                    // Possible node found

                    traversingArc = false;
                }
                else if (lightSensorValue > lightColorSIValue) 
                {
                    // Target found

                    targetReached = true;

                    traversingArc = false;

                    MakeTone(1000);

                    Task.Delay(500).Wait();

                    MakeTone(1000);

                    Task.Delay(500).Wait();

                    MakeTone(1000);

                    Task.Delay(500).Wait();
                }
                else
                {
                    TraverseArc(lightSensorValue);
                }
            }

            //System.Console.WriteLine("Leaving MaveNavigatorIntermediate()\n");
        }

        /*
         * TraverseArc makes the robot to move forward along the line/arc it is currently on while remaining centered. 
         */

        private async void TraverseArc(double sensorValue)
        {
            //System.Console.WriteLine("\nIn TraverseArc()");

            currentError = sensorValue - offset;

            integral = (2 / 3) * integral + currentError;

            derivative = currentError - previousError;

            turn = (kp * currentError) + (ki * integral) + (kd * derivative);

            previousError = currentError;

            if (!goingEastOrSouth)
            {
                rightMotorPower = tp + turn;

                leftMotorPower = tp - turn;
            }
            else
            {
                rightMotorPower = tp - turn;

                leftMotorPower = tp + turn;
            }

            if (rightMotorPower > 100)
            {
                rightMotorPower = 100;
            }
            else if (rightMotorPower < -100)
            {
                rightMotorPower = -100;
            }

            if (leftMotorPower > 100)
            {
                leftMotorPower = 100;
            }
            else if (leftMotorPower < -100)
            {
                leftMotorPower = -100;
            }

            //System.Console.WriteLine("R = " + rightMotorPower + ", L = " + leftMotorPower);

            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, Convert.ToInt32(rightMotorPower), 150, false);

            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, Convert.ToInt32(leftMotorPower), 150, false);

            await _brick.BatchCommand.SendCommandAsync();

            //System.Console.WriteLine("Leaving TraverseArc()\n");
        }

        /*
         * CanGoNorth makes the robot perform a set sequence of movements to check for a north arc option, returning 
         * true if the light sensor reads that an arc is present and connected and false if one is not. 
         */

        bool Intermediate.CanGoNorth()
        {
            //System.Console.WriteLine("\nIn CanGoNorth()");

            bool canGoNorth = false;

            if (_orientation.Equals("north"))
            {
                CheckForwardArcOption().Wait();

                canGoNorth = forwardArcOptionExists;
            }
            else if (_orientation.Equals("east"))
            {
                CheckLeftArcOption().Wait();

                canGoNorth = leftArcOptionExists;
            }
            else if (_orientation.Equals("south"))
            {
                canGoNorth = true;
            }
            else // __orientation.Equals("west")
            {
                CheckRightArcOption().Wait();

                canGoNorth = rightArcOptionExists;
            }

            //System.Console.WriteLine("Leaving CanGoNorth()\n");

            return canGoNorth;
        }

        /*
        * CanGoEast makes the robot perform a set sequence of movements to check for a east arc option, returning 
        * true if the light sensor reads that an arc is present and connected and false if one is not. 
        */

        bool Intermediate.CanGoEast()
        {
            //System.Console.WriteLine("\nIn CanGoEast()");

            bool canGoEast = false;

            if (_orientation.Equals("north"))
            {
                CheckRightArcOption().Wait();

                canGoEast = rightArcOptionExists;
            }
            else if (_orientation.Equals("east"))
            {
                CheckForwardArcOption().Wait();

                canGoEast = forwardArcOptionExists;
            }
            else if (_orientation.Equals("south"))
            {
                CheckLeftArcOption().Wait();

                canGoEast = leftArcOptionExists;
            }
            else // __orientation.Equals("west")
            {
                canGoEast = true;
            }

            //System.Console.WriteLine("Leaving CanGoEast()\n");

            return canGoEast;
        }

        /*
       * CanGoSouth makes the robot perform a set sequence of movements to check for a south arc option, returning 
       * true if the light sensor reads that an arc is present and connected and false if one is not. 
       */

        bool Intermediate.CanGoSouth()
        {
            //System.Console.WriteLine("\nIn CanGoSouth()");

            bool canGoSouth = false;

            if (_orientation.Equals("north"))
            {
                canGoSouth = true;
            }
            else if (_orientation.Equals("east"))
            {
                CheckRightArcOption().Wait();

                canGoSouth = rightArcOptionExists;
            }
            else if (_orientation.Equals("south"))
            {
                CheckForwardArcOption().Wait();

                canGoSouth = forwardArcOptionExists;
            }
            else // __orientation.Equals("west")
            {
                CheckLeftArcOption().Wait();

                canGoSouth = leftArcOptionExists;
            }

            //System.Console.WriteLine("Leaving CanGoSouth()\n");

            return canGoSouth;
        }

        /*
       * CanGoWest makes the robot perform a set sequence of movements to check for a west arc option, returning 
       * true if the light sensor reads that an arc is present and connected and false if one is not. 
       */

        bool Intermediate.CanGoWest()
        {
            //System.Console.WriteLine("\nIn CanGoWest()");

            bool canGoWest = false;

            if (_orientation.Equals("north"))
            {
                CheckLeftArcOption().Wait();

                canGoWest = leftArcOptionExists;
            }
            else if (_orientation.Equals("east"))
            {
                canGoWest = true;
            }
            else if (_orientation.Equals("south"))
            {
                CheckRightArcOption().Wait();

                canGoWest = rightArcOptionExists;
            }
            else // __orientation.Equals("west")
            {
                CheckForwardArcOption().Wait();

                canGoWest = forwardArcOptionExists;
            }

            //System.Console.WriteLine("Leaving CanGoWest()\n");

            return canGoWest;
        }

       /*
       * GoNorth makes the robot perform a set sequence of movements in order to travel north from its current position
       * at a node.
       */

        void Intermediate.GoNorth()
        {
            //System.Console.WriteLine("\nIn GoNorth()");

            if (_orientation.Equals("north"))
            {
                MoveForward().Wait();

                Task.Delay(500).Wait();

                MoveForward().Wait();

                Task.Delay(500).Wait();
            }
            else if (_orientation.Equals("east"))
            {
                TurnLeft().Wait();
            }
            else if (_orientation.Equals("south"))
            {
                TurnAround().Wait();
            }
            else // __orientation.Equals("west")
            {
                TurnRight().Wait();
            }

            _orientation = "north";

            goingEastOrSouth = false;

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving GoNorth()\n");
        }

        /*
      * GoEast makes the robot perform a set sequence of movements in order to travel east from its current position
      * at a node.
      */

        void Intermediate.GoEast()
        {
            //System.Console.WriteLine("\nIn GoEast()");

            if (_orientation.Equals("north"))
            {
                TurnRight().Wait();
            }
            else if (_orientation.Equals("east"))
            {
                MoveForward().Wait();

                Task.Delay(500).Wait();

                MoveForward().Wait();

                Task.Delay(500).Wait();
            }
            else if (_orientation.Equals("south"))
            {
                TurnLeft().Wait();
            }
            else // __orientation.Equals("west")
            {
                TurnAround().Wait();
            }

            _orientation = "east";

            goingEastOrSouth = true;

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving GoEast()\n");
        }

        /*
        * GoSouth makes the robot perform a set sequence of movements in order to travel south from its current position
        * at a node.
        */

        void Intermediate.GoSouth()
        {
            //System.Console.WriteLine("\nIn GoSouth()");

            if (_orientation.Equals("north"))
            {
                TurnAround().Wait();
            }
            else if (_orientation.Equals("east"))
            {
                TurnRight().Wait();
            }
            else if (_orientation.Equals("south"))
            {
                MoveForward().Wait();

                Task.Delay(500).Wait();

                MoveForward().Wait();

                Task.Delay(500).Wait();
            }
            else // __orientation.Equals("west")
            {
                TurnLeft().Wait();
            }

            _orientation = "south";

            goingEastOrSouth = true;

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving GoSouth()\n");
        }

        /*
        * GoWest makes the robot perform a set sequence of movements in order to travel west from its current position
        * at a node.
        */

        void Intermediate.GoWest()
        {
            //System.Console.WriteLine("\nIn GoWest()");

            if (_orientation.Equals("north"))
            {
                TurnLeft().Wait();
            }
            else if (_orientation.Equals("east"))
            {
                TurnAround().Wait();
            }
            else if (_orientation.Equals("south"))
            {
                TurnRight().Wait();
            }
            else // __orientation.Equals("west")
            {
                MoveForward().Wait();

                Task.Delay(500).Wait();

                MoveForward().Wait();

                Task.Delay(500).Wait();
            }

            _orientation = "west";

            goingEastOrSouth = false;

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving GoWest()\n");
        }

        /*
        * MoveForward makes the robot's motors move the robot forward.
        * at a node.
        */

        private async Task MoveForward()
        {
            //System.Console.WriteLine("\nIn MoveForward()");
            
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _forwardPower, _timeInWhichPowerWillBeApplied, false);

            //System.Console.WriteLine("Leaving MoveForward()\n");
        }

        /*
        * MoveBackward makes the robot's motors move the robot backward.
        * at a node.
        */

        private async Task MoveBackward()
        {
            //System.Console.WriteLine("\nIn MoveBackward()");

            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _backwardPower, _timeInWhichPowerWillBeApplied, false);

            //System.Console.WriteLine("Leaving MoveBackward()\n");
        }

        /*
        * PivotLeft makes the robot's motors pivot the robot slightly to the left.
        */

        private async Task PivotLeft()
        {
            //System.Console.WriteLine("\nIn PivotLeft()");
            
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _backwardPower, _timeInWhichPowerWillBeApplied, false);

            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _forwardPower, _timeInWhichPowerWillBeApplied, false);
            
            await _brick.BatchCommand.SendCommandAsync();

            //System.Console.WriteLine("Leaving PivotLeft()\n");
        }

        /*
        * PivotLeft makes the robot's motors pivot the robot slightly to the right.
        */

        private async Task PivotRight()
        {
            //System.Console.WriteLine("\nIn PivotRight()");

            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _backwardPower, _timeInWhichPowerWillBeApplied, false);
            
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _forwardPower, _timeInWhichPowerWillBeApplied, false);
            
            await _brick.BatchCommand.SendCommandAsync();

            //System.Console.WriteLine("Leaving PivotRight()\n");
        }

        /*
        * TurnLeft makes the robot's motors move the robot 90 degrees to the left.
        */

        private async Task TurnLeft()
        {
            //System.Console.WriteLine("\nIn TurnLeft()");
            
            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await PivotLeft();

            await Task.Delay(500);

            await PivotLeft();

            await Task.Delay(500);

            await PivotLeft();

            await Task.Delay(500);

            while (_brick.Ports[InputPort.One].SIValue < darkColorSIValue || _brick.Ports[InputPort.One].SIValue > lightColorSIValue)
            {
                await PivotLeft();

                await Task.Delay(500);
            }

            //System.Console.WriteLine("Leaving TurnLeft()\n");
        }

        /*
        * TurnRight makes the robot's motors move the robot 90 degrees to the right.
        */

        private async Task TurnRight()
        {
            //System.Console.WriteLine("\nIn TurnRight()");
            
            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            while (_brick.Ports[InputPort.One].SIValue < darkColorSIValue || _brick.Ports[InputPort.One].SIValue > lightColorSIValue)
            {
                await PivotRight();

                await Task.Delay(500);
            }

            //System.Console.WriteLine("Leaving TurnRight()\n");
        }

        /*
        * TurnAround makes the robot's motors move the robot 180 degrees to the right, reversing the robots orientation.
        */

        private async Task TurnAround()
        {
            //System.Console.WriteLine("\nIn TurnAround()");
            
            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await MoveForward();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            await PivotRight();

            await Task.Delay(500);

            while (_brick.Ports[InputPort.One].SIValue < darkColorSIValue || _brick.Ports[InputPort.One].SIValue > lightColorSIValue)
            {
                await PivotRight();

                await Task.Delay(500);
            }

            //System.Console.WriteLine("Leaving TurnAround()\n");
        }

        /*
        * UpdateOrientation updates the robot's recorded orientation based on the direction it just turned adn the robot's 
        * previously recorded direction. 
        */

        private void UpdateOrientation(string directionTurned)
        {
            //System.Console.WriteLine("\nIn UpdateOrientation()");
            
            if (directionTurned.Equals("left")) 
            {
                if (_orientation.Equals("north")) 
                {
                    _orientation = "west";
                }
                else if (_orientation.Equals("east")) 
                {
                    _orientation = "north";
                }
                else if (_orientation.Equals("south")) 
                {
                    _orientation = "east";
                }
                else // __orientation.Equals("west")
                {
                    _orientation = "south";
                }
            }
            else if (directionTurned.Equals("right")) 
            {
                if (_orientation.Equals("north"))
                {
                    _orientation = "east";
                }
                else if (_orientation.Equals("east"))
                {
                    _orientation = "south";
                }
                else if (_orientation.Equals("south"))
                {
                    _orientation = "west";
                }
                else // __orientation.Equals("west")
                {
                    _orientation = "north";
                }
            }
            else // turned around a full 180 degrees
            {
                if (_orientation.Equals("north"))
                {
                    _orientation = "south";
                }
                else if (_orientation.Equals("east"))
                {
                    _orientation = "west";
                }
                else if (_orientation.Equals("south"))
                {
                    _orientation = "north";
                }
                else // __orientation.Equals("west")
                {
                    _orientation = "east";
                }
            }

            //System.Console.WriteLine("Leaving UpdateOrientation()\n");
        }

        /*
        * MakeTone makes the brick play a tone. It takes a ushort between 1000 (for higher pitch) and 300 (for lower pitch).
        */

        private async void MakeTone(ushort freq)
        {
            //System.Console.WriteLine("\nIn MakeTone()");

            await _brick.DirectCommand.PlayToneAsync(100, freq, 300);

            //System.Console.WriteLine("Leaving MakeTone()\n");
        }

        /*
        * CheckLeftArcOption moves the robot in way that it can use its color sensor to look for an arc to the left of the
         * current node. 
        */

        private async Task CheckLeftArcOption()
        {
            //System.Console.WriteLine("\nIn CheckLeftArcOption()");

            await PivotLeft();
            await Task.Delay(500);

            await MoveForward();
            await Task.Delay(500);

            await PivotLeft();
            await Task.Delay(500);

            if (_brick.Ports[InputPort.One].SIValue < whiteSIValue)
            {
                MakeTone(1000);

                leftArcOptionExists = true;
            }
            else
            {
                MakeTone(500);

                leftArcOptionExists = false;
            }
            await Task.Delay(500);

            await PivotRight();
            await Task.Delay(500);

            await MoveBackward();
            await Task.Delay(500);

            await PivotRight();
            await Task.Delay(500);

            //System.Console.WriteLine("Leaving CheckLeftArcOption()\n");
        }

        /*
        * CheckForwardArcOption moves the robot in way that it can use its color sensor to look for an arc to forward of the
         * current node. 
        */

        private async Task CheckForwardArcOption()
        {
            //System.Console.WriteLine("\nIn CheckForwardArcOption()");

            await MoveForward();
            await Task.Delay(500);

            await MoveForward();
            await Task.Delay(500);

            if (_brick.Ports[InputPort.One].SIValue < whiteSIValue)
            {
                MakeTone(1000);

                forwardArcOptionExists = true;
            }
            else
            {
                MakeTone(500);

                forwardArcOptionExists = false;
            }
            await Task.Delay(500);

            await MoveBackward();
            await Task.Delay(500);

            await MoveBackward();
            await Task.Delay(500);

            //System.Console.WriteLine("Leaving CheckForwardArcOption()\n");
        }

        /*
       * CheckRightArcOption moves the robot in way that it can use its color sensor to look for an arc to the right of the
        * current node. 
       */

        private async Task CheckRightArcOption()
        {
            //System.Console.WriteLine("\nIn CheckRightArcOption()");

            await PivotRight();
            await Task.Delay(500);

            await MoveForward();
            await Task.Delay(500);

            await PivotRight();
            await Task.Delay(500);

            if (_brick.Ports[InputPort.One].SIValue < whiteSIValue)
            {
                MakeTone(1000);

                rightArcOptionExists = true;
            }
            else
            {
                MakeTone(500);

                rightArcOptionExists = false;
            }
            await Task.Delay(500);

            await PivotLeft();
            await Task.Delay(500);

            await MoveBackward();
            await Task.Delay(500);

            await PivotLeft();
            await Task.Delay(500);

            //System.Console.WriteLine("Leaving CheckRightArcOption()\n");
        }

     /*
      * TargetReached returns a bool to the agent, letting it know if the target has been reached or not. 
      */

        bool Intermediate.TargetReached()
        {
            //System.Console.WriteLine("\nIn TargetReached()");

            return targetReached;

            //System.Console.WriteLine("Leaving TargetReached()\n");
        }

        /*
         * StartNewRun returns a bool to the agent, letting it know if the target has been reached or not. 
         */

        void Intermediate.StartNewRun()
        {
            //System.Console.WriteLine("\nIn StartNewRun()");

            System.Console.WriteLine("Move the robot to an new location in the maze, then enter the orientation of the robot.");

            string newOrientation  = System.Console.ReadLine();

            while (!(newOrientation.Equals("north") ||
                   newOrientation.Equals("east") ||
                   newOrientation.Equals("south") ||
                   newOrientation.Equals("west")))
            {
                System.Console.WriteLine("Input not recognized. Please enter the orientation of the robot");
                
                newOrientation = System.Console.ReadLine();
            }

            _orientation = newOrientation;

            if (_orientation.Equals("east") || _orientation.Equals("south")) 
            {
                goingEastOrSouth = true;
            }
            else 
            {
                goingEastOrSouth = false;
            }

            integral = 0;

            previousError = 0;

            derivative = 0;

            targetReached = false;

            _brick.DirectCommand.ClearAllDevices();

            //SetUpBrick();

            traversingArc = true;

            while (traversingArc == true)
            {
                Task.Delay(500).Wait();
            }

            //System.Console.WriteLine("Leaving StartNewRun()\n");
        }

        /*
         * Orient updates the robots direction. It would be used if the robot had a compass sensor, 
         * which it currently does not. 
         */

        void Intermediate.Orient() {

            //System.Console.WriteLine("\nIn StartNewRun()");

            //System.Console.WriteLine("Leaving StartNewRun()\n");
        }
    }
}
