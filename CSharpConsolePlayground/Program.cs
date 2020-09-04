using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpConsolePlayground
{
    class Program
    {
        // Decleration of variables - Voting regions and time consuming simulations for each region by miliseconds
        public static Dictionary<string, int> Regions = new Dictionary<string, int>()
             {
                { "Marmara", 2000},
                { "Akdeniz", 1000},
                { "Anadolu", 1500}
             };
        public static async Task Main(string[] args)
        {
            // Starting of Async Voting Simulation
            await AsyncTasks();
            // Breaking line between the results
            Console.WriteLine();
            // Starting of Sync Votin Simulation
            SyncTasks();
        }
        // Sync Voting Simulation
        // Timer started for the comparing of task performance
        // List variable created to keep all results from functions
        // Each functions called by synchnorized and recorded in VoteCounts variable
        // vote result shown at the end
        private static void SyncTasks()
        {
            Stopwatch Chronomemeter = StartVoting("Sync");
            List<double> VoteCounts = new List<double>();
            foreach (var item in Regions)
            {
                VoteCounts.Add(VotesSync(item.Value, item.Key));
            }
            CompleteVoting(Chronomemeter, VoteCounts, "Sync");
        }
        // Async Voting Simulation
        // Timer started for the comparing of task performance
        // List<Task> variable created to keep all the task
        // Each task called by asynchnorized and recorded in VoteCounts variable
        // vote result shown at the end
        private static async Task AsyncTasks()
        {
            Stopwatch Chronomemeter = StartVoting("Async");
            List<Task<double>> VoteCounts = new List<Task<double>>();
            foreach (var item in Regions)
            {
                VoteCounts.Add(VotesAsync(item.Value, item.Key));
            }
            var results = await Task.WhenAll(VoteCounts);
            CompleteVoting(Chronomemeter, results.ToList(), "Async");
        }
        // Voting start function with stopwatch object return
        private static Stopwatch StartVoting(string SyncType)
        {
            Stopwatch Chronomemeter = new Stopwatch();
            Chronomemeter.Start();
            Console.WriteLine($"- - - - {SyncType} Voting started ! - - - -");
            return Chronomemeter;
        }
        // Completing of voting and writing the results
        private static void CompleteVoting(Stopwatch Chronomemeter, List<double> results, string SyncType)
        {
            Console.WriteLine($"- - - - {SyncType} Voting completed ! - - - -");
            Chronomemeter.Stop();
            var TotalTime = Chronomemeter.ElapsedMilliseconds;
            Console.WriteLine($"Voting Results\t\t\t\t: {results.Sum()}");
            Console.WriteLine($"Completed in\t\t\t\t: {TotalTime} miliseconds");
        }
        // Async vote function
        public static async Task<double> VotesAsync(int DurationPeriod, string RegionName)
        {
            await Task.Delay(DurationPeriod);
            return VoteCalculateAndWrite(RegionName);
        }
        // Sync vote function
        public static double VotesSync(int DurationPeriod, string RegionName)
        {
            Thread.Sleep(DurationPeriod);
            return VoteCalculateAndWrite(RegionName);
        }
        // Random vote result for vote functions
        private static double VoteCalculateAndWrite(string RegionName)
        {
            Random RandomVote = new Random();
            var VoteResult = RandomVote.Next(1, 11);
            Console.WriteLine($"Voting Result from {RegionName}\t\t: {VoteResult}");
            return VoteResult;
        }
    }
}
