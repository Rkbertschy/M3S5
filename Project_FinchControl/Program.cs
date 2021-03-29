using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

using System.IO;


namespace Project_FinchControl
{
  
    /// <summary>
    /// User Commands
    /// </summary>
    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }
      
       
        
    
    //
    // Title: Finch Controler
    // Description: A program to control the finch in diffrent ways
    // Application Type: Console
    // Author: Ryan Bertschy
    // Dated Created: 2/20/2021
    // Last Modified: 3/28/2021  
    //


    class Program
    {
        /// <summary>
        /// Main Program Order
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {




            DisplaySetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        #region Theme



        /// <summary>
        /// Display Theme 
        /// </summary>
        static void DisplaySetTheme()
        {
            (ConsoleColor forgroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;


            //Set Theme

            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.forgroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("\tSet App Theme");

            Console.WriteLine($"\t\tCurrent text color: {Console.ForegroundColor}");
            Console.WriteLine($"\t\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.WriteLine("\t\tWould you like to change the theme colors? [Yes or No]" );
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.forgroundColor = GetColorFromUser("text");
                    themeColors.backgroundColor = GetColorFromUser("background");

                    //Set New Theme
                    Console.ForegroundColor = themeColors.forgroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set New Theme for App");
                    Console.WriteLine($"\t\tNew Text Color: {Console.ForegroundColor}");
                    Console.WriteLine($"\t\tNew Background Color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\t\tDo you wish to change the theme again?");
                    if (Console.ReadLine().ToLower() == "no" )
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.forgroundColor, themeColors.backgroundColor);
                    }

                } while (!themeChosen);

            }
            DisplayContinuePrompt();

        }



        /// <summary>
        /// Write Theme Colors
        /// </summary>
        /// <param name="forground"></param>
        /// <param name="background"></param>
        static void WriteThemeData(ConsoleColor forground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";

            File.WriteAllText(dataPath, forground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }



        /// <summary>
        /// Read Theme Color
        /// </summary>
        /// <returns></returns>
        static (ConsoleColor forgroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor forgroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out forgroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (forgroundColor, backgroundColor);

        }


        /// <summary>
        /// Get Color From User
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        static ConsoleColor GetColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;


            Console.Clear();
            DisplayScreenHeader("\tChoose New Theme Colors");
            

            do
            {
                Console.WriteLine($"\t\tEnter a Color for the {property}:");
                Console.WriteLine("\tCOLOR LIST [Black, DarkBlue, DarkGreen, " +
                        "DarkRed, DarkMagenta, DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta, Yellow, and White]");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);
                
                

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** Please enter a valid console color, [ Black, DarkBlue, DarkGreen, " +
                        "DarkRed, DarkMagenta, DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta, Yellow, and White] ******\n");

                }
                else
                {
                    validConsoleColor = true;
                }



            } while (!validConsoleColor);

            return consoleColor;

        }

              

        #endregion


        /// <summary>
        /// Home Screen  
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // home screen menu
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // Home Screen choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DisplayDataRecoderMenuScreen(finchRobot);
                        break;

                    case "d":
                        DisplayAlarmSystemMenuScreen(finchRobot);
                        break;

                    case "e":
                        DisplayUserProgrammingMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }


        #region user Programming 

        /// <summary>
        /// User Programming Menu
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayUserProgrammingMenuScreen(Finch finchRobot)
        {
            string menuChoice;
            bool quitMenu = false;

            // Tuple for Command Parameters
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("\t\tUser Programming");

                // Menu choices

                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Command");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Perform Commands");
                Console.WriteLine("\tq) Back");
                Console.WriteLine("\t\t Please Enter Choice ");
                menuChoice = Console.ReadLine().ToLower();


                // Process Menu Choices

                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandPerameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(finchRobot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter from the list of menu choices.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        
        
        
        
        
        
        
        
        
        
        
        
        
        }

        /// <summary>
        /// Execute Commands
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        static void UserProgrammingDisplayExecuteFinchCommands(
            Finch finchRobot, 
            List<Command> commands, 
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("\tExecute Finch Commands");

            Console.WriteLine("\tThe Finch Robot is ready to execute the commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature().ToString("n2")}\n";
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:

                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");


            }







        }

        /// <summary>
        /// Display Commands
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("\tFinch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("\tUser Programming");
        }

        /// <summary>
        /// Get User Commands
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("\tFinch Robot Commands");

            //List of Commands

            int commandCount = 1;

            Console.WriteLine("\tLost of Availiable Commands");
            Console.WriteLine();
            Console.Write("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"< {commandName.ToLower()} >");
                if (commandCount % 5 == 0) Console.Write("<\n\t>");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.WriteLine("\tEnter Command:");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\t\tPLEASE ENTER A COMMAND FROM LIST ABOVE.");
                }
           
           
            }

            Console.WriteLine($"\t{command}");


            DisplayMenuPrompt("\tUser Programming");

        }



        /// <summary>
        /// Get command Parameters
        /// </summary>
        /// <returns>Tuple for Command Parameters</returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandPerameters()
        {
            DisplayScreenHeader("\tCommand Perameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            GetValidSpeed("\tEnter Motor Speed [1 - 255]:", 1, 255, out commandParameters.motorSpeed);
            GetValidBrightness("\tEnter LED Brightness [1 - 255]:", 1, 255, out commandParameters.ledBrightness);
            GetValidTime("\tEnter wait time in Seconds:", 0, 10, out commandParameters.waitSeconds);

            Console.WriteLine();
            Console.WriteLine($"\t\tMotor Speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\t\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\t\tWait Time: {commandParameters.waitSeconds}");

            DisplayMenuPrompt("\tUser Programming");

            return commandParameters;
        }

        static int GetValidBrightness(string v1, int v2, int v3, out int ledBrightness)
        {
            do
            {
                DisplayScreenHeader("\tLED Brightness");

                Console.WriteLine();
                Console.WriteLine("\tPlease Enter LED Brightness (1-255):");
                int.TryParse(Console.ReadLine(), out ledBrightness);

                if (ledBrightness < 1 || ledBrightness > 255)
                {
                    Console.WriteLine("\t" +
                        "Please enter an integer between 1 and 255.");
                    DisplayContinuePrompt();
                }
            } while (ledBrightness < 1 || ledBrightness > 255);

            return ledBrightness;
        }

        static double GetValidTime(string v1, int v2, int v3, out double waitSeconds)
        {
            do
            {
                DisplayScreenHeader("\tWait Time");

                Console.WriteLine();
                Console.WriteLine("\tPlease Enter Wait Time (seconds) (1-15):");
                double.TryParse(Console.ReadLine(), out waitSeconds);

                if (waitSeconds < 1 || waitSeconds > 15)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer between 1 and 15.");
                    DisplayContinuePrompt();
                }
            } while (waitSeconds < 1 || waitSeconds > 15);

            return waitSeconds;
        }

        static int GetValidSpeed(string v1, int v2, int v3, out int motorSpeed)
        {
            do
            {
                DisplayScreenHeader("\tMotor Speed");

                Console.WriteLine();
                Console.WriteLine("\tPlease Enter Motor Speed (1-255):");
                int.TryParse(Console.ReadLine(), out motorSpeed);

                if (motorSpeed < 1 || motorSpeed > 255)
                {
                    Console.WriteLine("\t" +
                        "Please enter an integer between 1 and 255.");
                    DisplayContinuePrompt();
                }
            } while (motorSpeed < 1 || motorSpeed > 255);

            return motorSpeed;
        }

        #endregion

        #region Alarm System

        /// <summary>
        /// Alarm System
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayAlarmSystemMenuScreen(Finch finchRobot)
        {
            {
                Console.CursorVisible = true;

                bool quitMenu = false;
                string menuChoice;

                string sensorsToMonitor = "";
                string rangeType = "";
                int minMaxThresholdValue = 0;
                int timeToMonitor = 0;


                do
                {
                    DisplayScreenHeader("Alarm System Menu");

                    //
                    // user Data Recorder screen
                    //
                    Console.WriteLine("\ta) Set Sensors to Monitor");
                    Console.WriteLine("\tb) Set Range Type ");
                    Console.WriteLine("\tc) Set Min/Max Threshold Value");
                    Console.WriteLine("\td) Set Time to Monitor");
                    Console.WriteLine("\te) Set Alarm");
                    Console.WriteLine("\tq) Back");
                    Console.Write("\t\tEnter Choice:");
                    menuChoice = Console.ReadLine().ToLower();

                    //
                    // user Data Recorder choice
                    //
                    switch (menuChoice)
                    {
                        case "a":
                            sensorsToMonitor = AlarmSystemDisplaySetSenorsToMonitor();
                            break;

                        case "b":
                            rangeType = AlarmSystemDisplaySetRangeType();
                            break;

                        case "c":
                            minMaxThresholdValue = AlarmSystemSetMinMaxThresholdValue(rangeType, finchRobot);
                            break;

                        case "d":
                            timeToMonitor = AlarmSystemSetTimeToMonitor();
                            break;

                        case "e":
                            AlarmSystemSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                            break;

                        case "q":
                            quitMenu = true;
                            break;


                        default:
                            Console.WriteLine();
                            Console.WriteLine("\t\tPlease enter a letter for the menu choice.");
                            DisplayContinuePrompt();
                            break;
                    }

                } while (!quitMenu);
            }
        }
        /// <summary>
        /// Set Alarm
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="timeToMonitor"></param>
        static void AlarmSystemSetAlarm(
            Finch finchRobot,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightvalue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\t\tSensors to monitor {sensorsToMonitor}");
            Console.WriteLine($"\t\tRange type: {0}", rangeType);
            Console.WriteLine($"\t\tMin/Max threshold value {minMaxThresholdValue}");
            Console.WriteLine($"\t\tTime to monitor {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("\t\tPress any key to start system monitoring");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightvalue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightvalue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightvalue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "Min":
                        if (currentLightvalue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "Max":
                        if (currentLightvalue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                secondsElapsed++;

            }

            if (thresholdExceeded)
            {
                Console.WriteLine($"\t\tThe {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the light sensor value of {currentLightvalue}.");
            }
            else
            {
                Console.WriteLine($"\t\tThe {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
            }


            DisplayMenuPrompt("Alarm System");
        }


        /// <summary>
        /// Threshold Value
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static int AlarmSystemSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;
            bool validResponse = false;
           
            do
            {
                DisplayScreenHeader("Min/Max Threshold Value");

                Console.WriteLine($"\t\tLeft light sensor ambient value: {finchRobot.getLeftLightSensor()}");
                Console.WriteLine($"\t\tRight light sensor ambient value: {finchRobot.getRightLightSensor()}");
                Console.WriteLine();

                Console.Write($"\t\tEnter the {rangeType} light sensor value");
                int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

                if (minMaxThresholdValue <= 0)
                {
                    Console.WriteLine("\t\tPlesae enter a postive number [1, 2, 3,...]");
                }
                else
                {
                    validResponse = true;
                }

                DisplayMenuPrompt("Alarm System");

            } while (!validResponse);


            return minMaxThresholdValue;
        }


        /// <summary>
        /// Time to Monitor
        /// </summary>
        /// <returns></returns>
        static int AlarmSystemSetTimeToMonitor()
        {
            int timeToMonitor;
            bool validResponse = false;

            do
            {
                DisplayScreenHeader("Time to Monitor");

                Console.Write($"\t\tTime in seconds:");
                Console.WriteLine();
                int.TryParse(Console.ReadLine(), out timeToMonitor);
                Console.WriteLine();

                if (timeToMonitor <= 0)
                {
                    Console.WriteLine("\t\tPlease enter a positive number [1, 2, 3,...]");
                }
                else
                {
                    validResponse = true;
                }

                DisplayMenuPrompt("Alarm System");

            } while (!validResponse);


            return timeToMonitor;
        }

        /// <summary>
        /// Sensors to Monitor
        /// </summary>
        /// <returns></returns>
        static string AlarmSystemDisplaySetSenorsToMonitor()
        {
            string sensorsToMonitor;
            string userResponse;
            bool validResponse = false;

            do
            {
                DisplayScreenHeader("Sensors to Monitor");

                Console.Write("\t\tSensors to Monitor [Left, Right, Both]:");
                Console.WriteLine();
                userResponse = Console.ReadLine().ToLower();
                sensorsToMonitor = Console.ReadLine();
                if (userResponse != "left" && userResponse != "right" && userResponse != "both")
                {
                    Console.WriteLine("\t\tPlease enter [Left, Right, or Both]");
                    Console.WriteLine();
                }
                else
                {
                    validResponse = true;
                }


            } while (!validResponse);

            DisplayMenuPrompt("Alarm System");

            return sensorsToMonitor;
        }

        /// <summary>
        /// Range Type
        /// </summary>
        /// <returns></returns>
        static string AlarmSystemDisplaySetRangeType()
        {
            string rangeType;
            string userResponse;
            bool validResponse = false;

            do
            {
                DisplayScreenHeader("Range Type");

                Console.Write("\t\tRange Type [Min, Max]:");
                Console.WriteLine();
                userResponse = Console.ReadLine().ToLower();
                rangeType = Console.ReadLine();

                if (userResponse != "min" && userResponse != "max")
                {
                    Console.WriteLine("\t\tPlease enter [Min or Max]");
                }
                else
                {
                    validResponse = true;
                }

                DisplayMenuPrompt("Alarm System");

            } while (!validResponse);


            return rangeType;
        }

        #endregion

        #region Data Recorder

        /// <summary>
        /// Data recorder menu
        /// </summary>
        static void DisplayDataRecoderMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            int numberOfDataPoints = 0;
            double frequencyOfDataPointsInSecounds = 0;
            double[] temperatures = null;


            do
            {
                DisplayScreenHeader("Data Recoder Menu");

                //
                // user Data Recorder screen
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Display the data set");
                Console.WriteLine("\tq) Back");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // user Data Recorder choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        frequencyOfDataPointsInSecounds = DataRecorderDisplayGetFrequencyOfDataPoints();
                        break;

                    case "c":
                        if (numberOfDataPoints == 0 || frequencyOfDataPointsInSecounds == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t\tPlease preform the actions in A and B first.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            temperatures = DateRecorderDisplayGetDataSet(numberOfDataPoints, frequencyOfDataPointsInSecounds, finchRobot);
                        }
                        break;

                    case "d":
                        DataRecorderDisplayGetDataSet(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        /// <summary>
        /// Data Recorder, Display Data Table
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayDataTable(double[] temperatures)
        {
            Console.WriteLine(
                "\t\tReading #".PadLeft(20) +
                "Temerature".PadLeft(20)
                );

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(25) +
                    temperatures[index].ToString("n2").PadLeft(25)
                    );

            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Data Recorder, Display Data Set
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayGetDataSet(double[] temperatures)
        {
            DisplayScreenHeader("\t\tData set");

            DataRecorderDisplayDataTable(temperatures);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Data Recorder, Get The Data Points
        /// </summary>
        /// <param name="finchRobot"></param>
        static double[] DateRecorderDisplayGetDataSet(int numberOfDataPoints, double frequencyOfDataPointsInSecounds, Finch finchRobot)
        {
            double[] temeratures = new double[numberOfDataPoints];



            DisplayScreenHeader("\t\tGet Data Set");

            Console.WriteLine($"\t\tNumber of Data Points: {numberOfDataPoints}");
            Console.WriteLine($"\t\tFrequency of Data Points: {frequencyOfDataPointsInSecounds}");
            Console.WriteLine();
            Console.WriteLine("\t\tThe Finch Robot is ready to recorder the temperatures. Please press any key to start.");
            Console.ReadKey();


            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temeratures[index] = finchRobot.getTemperature();
                Console.WriteLine($"\t Reading {index + 1}: {temeratures[index].ToString("n2")}");
                int waitInSecounds = (int)(frequencyOfDataPointsInSecounds * 1000);
                finchRobot.wait(waitInSecounds);
            }

            DisplayContinuePrompt();

            return temeratures;
        }

        /// <summary>
        /// Data Recorder, Get The Frequency Of Data Points
        /// </summary>
        /// <param name="finchRobot"></param>
        static double DataRecorderDisplayGetFrequencyOfDataPoints()
        {
            string userResponse;
            double frequencyOfDataPoints;
            bool validResponse;

            DisplayScreenHeader("\t\tThe Frequency of the Data Points");

            do
            {
                Console.Write("\t\tEnter the Frequency of the Data Points.  ");
                userResponse = Console.ReadLine();

                validResponse = double.TryParse(userResponse, out frequencyOfDataPoints);

                if (frequencyOfDataPoints > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("\t\tPlease enter a number greater then 0.  ");
                    Console.WriteLine();
                }


            } while (!validResponse);


            Console.WriteLine();
            Console.WriteLine($"\t\tThe Frequency of the Data Points: {frequencyOfDataPoints}");


            DisplayContinuePrompt();

            return frequencyOfDataPoints;
        }


        /// <summary>
        /// Data Recorder, Get Number Of Data Points
        /// </summary>
        /// <param name="finchRobot"></param>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            string userResponse;
            int numberOfDataPoints;
            bool validResponse;

            DisplayScreenHeader("\t\tNumber of Data Points");

            do
            {
                Console.Write("\t\tEnter the number of Data Points.  ");
                userResponse = Console.ReadLine();

                validResponse = int.TryParse(userResponse, out numberOfDataPoints);

                if (!validResponse)
                {
                    Console.WriteLine("\t\tPLease enter an integer  ");
                    Console.WriteLine();
                }


            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\t\t There are {numberOfDataPoints} Data Points.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            return numberOfDataPoints;
        }

        #endregion

        #region TALENT SHOW

        /// <summary>
        /// talent show menu
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // user talent show screen
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance ");
                Console.WriteLine("\tc) Song");
                Console.WriteLine("\td) Interpretive Dance");
                Console.WriteLine("\tq) Back");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // user talent show choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        DisplayDance(finchRobot);
                        break;

                    case "c":
                        DisplaySong(finchRobot);
                        break;

                    case "d":
                        DisplayInterpretiveDance(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }
        /// <summary>
        /// Interpretive Dance
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void DisplayInterpretiveDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 200; lightSoundLevel++)
            {
                finchRobot.setLED(0, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);


            }

            finchRobot.setMotors(250, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 250);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(200);

            for (int i = 200; i < 255; i++)
            {
                finchRobot.setMotors(i, -i);
                finchRobot.setLED(0, i, i);


                finchRobot.setMotors(0, 0);
                finchRobot.setLED(0, 0, 0);

            }


            DisplayMenuPrompt("Talent Show Menu");
        }




        /// <summary>
        /// Talent Show, Song
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplaySong(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Song");

            Console.WriteLine("\t\tThe Finch will now Sing!");
            DisplayContinuePrompt();

            //verse 1


            finchRobot.noteOn(392);
            finchRobot.noteOff();
            finchRobot.wait(500);
            finchRobot.noteOn(392);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(500);
            finchRobot.noteOn(392);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(500);
            finchRobot.noteOn(392);
            finchRobot.noteOn(440);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(750);

            //verse 2
            finchRobot.noteOn(329);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(392);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(392);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(750);

            //verse 3
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(750);

            //verse 4
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1318);
            finchRobot.wait(550);
            finchRobot.noteOn(1396);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1174);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(750);

            // cor
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(600);

            //verse 5
            finchRobot.noteOn(392);
            finchRobot.wait(500);
            finchRobot.noteOn(392);
            finchRobot.wait(250);
            finchRobot.noteOn(392);
            finchRobot.wait(500);
            finchRobot.noteOn(440);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOn(440);
            finchRobot.noteOn(392);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //verse 6
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1318);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //verse 7
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //verse 8
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1318);
            finchRobot.wait(500);
            finchRobot.noteOn(1396);
            finchRobot.wait(500);
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1174);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //cor
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1567);
            finchRobot.wait(250);
            finchRobot.noteOn(1760);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            finchRobot.noteOn(1567);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1046);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            finchRobot.noteOn(440);
            finchRobot.wait(250);
            finchRobot.noteOn(392);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //verse 9
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1318);
            finchRobot.wait(500);
            finchRobot.noteOn(1396);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1318);
            finchRobot.noteOn(1174);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);

            //cor
            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.wait(250);
            finchRobot.noteOn(1174);
            finchRobot.noteOn(1046);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(250);
        }


        /// <summary>
        /// Talent Show, Dance
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\t\tThe Finch will now dance!");
            DisplayContinuePrompt();

            finchRobot.setMotors(0, 150);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(100);
            finchRobot.setMotors(0, -150);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(100);
            finchRobot.setMotors(150, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(100);
            finchRobot.setMotors(-150, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(100);
            finchRobot.setMotors(150, 150);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(100);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(750);
            finchRobot.setMotors(0, 0);

        }

        /// <summary>
        /// Talent Show, Light and Sound
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);

            }

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// disconnect screen
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// connect screen
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected)
            {
                Console.WriteLine("\t\tRobot Connected.");
                for (int i = 0; i < 255; i++)
                {
                    finchRobot.setLED(i, 0, i);

                }
                for (int i = 255; i > 0; i--)
                {
                    finchRobot.setLED(i, 0, i);

                }
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine("\t\tThere was a problem connecting");
            }

            DisplayMenuPrompt("Main Menu");

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t\t Welcome to the Finch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}

    

