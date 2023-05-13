using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Menus
    {
        Validation v = new Validation();
        internal void initMenu()
        {
            Console.WriteLine("********************");
            Console.WriteLine("Enter a number:");
            Console.WriteLine("(1) Add Coding Session");
            Console.WriteLine("(2) Update Coding Session");
            Console.WriteLine("(3) View Logged Coding Sessions");
            Console.WriteLine("(4) Delete Coding Session");
            Console.WriteLine("(5) Track Coding Session Via Stopwatch");
            Console.WriteLine("(0) Exit Application");
            Console.WriteLine("********************");

            string input = Console.ReadLine();
            int choice;

            if (!v.ValidateInput(input))
                initMenu();
            choice = int.Parse(input);

            switch (choice)
            {
                case 1:
                    NewSession();
                    break;
                case 2:
                    UpdateSession();
                    break;
                case 3:
                    ViewSessions();
                    break;
                case 4:
                    DeleteSession();
                    break;
                case 5:
                    NewSessionStopwatch();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Not a valid number! Try again."); initMenu();
                    break;
            }
        }
        internal void NewSession()
        {
            CodingSession session = new CodingSession();

            Console.WriteLine("");
            Console.WriteLine("Enter a start time for your coding session in one of the following formats:");
            Console.WriteLine("05/12/23 4:14 PM (DD/MM/YY HH:MM AM/PM)");
            string input = Console.ReadLine();
            if (!v.ValidateInput(input))
                initMenu();
            else
                session.StartTime = input;
            DateTime start = DateTime.Parse(input);
            Console.WriteLine("");
            Console.WriteLine("Enter an end time for your coding session in the following format:");
            Console.WriteLine("05/12 4:21 PM (DD/MM/YYYY HH:MM AM/PM)");
            input = Console.ReadLine();
            if (!v.ValidateInput(input))
                initMenu();
            else
                session.EndTime = input;
            DateTime end = DateTime.Parse(input);
            session.Duration = (end - start).ToString(@"hh\:mm\:ss");
            Console.WriteLine($"Duration of coding session:{session.Duration}");

            SqliteDataAccess.SaveSession(session);
            Console.WriteLine("Session has been saved! Returning to main menu...");
            Console.WriteLine("");

            initMenu();
        }
        internal void UpdateSession()
        {
            List<CodingSession> list = SqliteDataAccess.LoadSessions();


            foreach (CodingSession session in list)
            {
                Console.WriteLine($"Start time: {DateTime.Parse(session.StartTime)} - End time: {DateTime.Parse(session.EndTime)} ||| Duration: {session.Duration}");
            }
            Console.WriteLine("");
            Console.WriteLine("Enter the ID for the session you would like to update:");
            string input = Console.ReadLine();
            if (!v.ValidateInput(input))
                UpdateSession();
            int choice = int.Parse(input);
            CodingSession oldSession = list.Find(x => x.Id == choice);

            if (!v.ValidateInput(choice, oldSession))
                UpdateSession();


            Console.WriteLine("What would you like to update");
            Console.WriteLine("(1) Start time");
            Console.WriteLine("(2) End Time");
            input = Console.ReadLine();

            if (!v.ValidateInput(input))
                UpdateSession();
            choice = int.Parse(input);

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter updated start time:");
                    input = Console.ReadLine();
                    if (!v.ValidateInput(input))
                        UpdateSession();
                    oldSession.StartTime = input;
                    break;
                case 2:
                    Console.WriteLine("Enter updated end time:");
                    input = Console.ReadLine();
                    if (!v.ValidateInput(input))
                        UpdateSession();
                    oldSession.EndTime = input;
                    break;
                default:
                    Console.WriteLine("Not a valid number! Try again."); UpdateSession();
                    break;
            }
            oldSession.Duration = (DateTime.Parse(oldSession.EndTime) - DateTime.Parse(oldSession.StartTime)).ToString(@"hh\:mm\:ss");
            SqliteDataAccess.UpdateSessions(oldSession);
            Console.WriteLine("Your session has been updated! Returning to main menu...");
            initMenu();


        }
        internal void ViewSessions()
        {
            List<CodingSession> list = SqliteDataAccess.LoadSessions();

            TableView tv = new TableView();
            tv.DisplayTable(list);

            Console.WriteLine("");

            initMenu();
        }
        internal void DeleteSession()
        {
            List<CodingSession> list = SqliteDataAccess.LoadSessions();

            foreach (CodingSession session in list)
            {
                Console.WriteLine($"({session.Id}) {DateTime.Parse(session.StartTime)} - {DateTime.Parse(session.EndTime)} ||| {session.Duration}");
            }
            Console.WriteLine("");
            Console.WriteLine("Enter the ID for the session you want to delete:");
            string input = Console.ReadLine();
            if (!v.ValidateInput(input))
                DeleteSession();
            int sessionID = int.Parse(input);

            CodingSession sessionToDelete = list.Find(x => x.Id == sessionID);
            Console.WriteLine("Are you sure you want to delete this session? Yes/No:");
            string answer = Console.ReadLine().ToLower();
            switch (answer)
            {
                case "no":
                    initMenu();
                    break;
                case "yes":
                    SqliteDataAccess.DeleteSessions(sessionToDelete);
                    break;
                default:
                    Console.WriteLine("Invalid entry! Try again!");
                    DeleteSession();
                    break;
            }

            Console.WriteLine($"The session has been deleted from the database. Returning to main menu...");
            Console.WriteLine("");
            initMenu();
        }
        internal void NewSessionStopwatch()
        {
            CodingSession session = new CodingSession();
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Type 'start' to start stopwatch! Then type 'stop' to stop it!");
            string input = Console.ReadLine();
            if (input.ToLower() == "start")
            {
                session.StartTime = DateTime.Now.ToString();
                stopwatch.Start();
            }
            else
            {
                Console.WriteLine("Invalid entry, returning to main menu.");
                initMenu();
            }
            bool continueTimer = false;
            while (!continueTimer)
            {
                input = Console.ReadLine();
                if (input.ToLower() == "stop")
                {
                    session.EndTime = DateTime.Now.ToString();
                    stopwatch.Stop();
                    continueTimer = true;
                }
                else
                {
                    Console.WriteLine("Type 'stop' to end the stopwatch!");
                }
            }

            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            session.Duration = elapsedTime;
            Console.WriteLine($"The duration of your coding session was: {elapsedTime}");
            SqliteDataAccess.SaveSession(session);
            initMenu();
        }

    }
}
