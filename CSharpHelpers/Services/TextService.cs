using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpHelpers.Models;

namespace CSharpHelpers.Services
{
    public class TextService : BaseService
    {
        // private static readonly CultureInfo cultureInfo = new("ru-RU");

        public static async Task<List<(ScanTextResult, string[]? matchesResult)>> FindMatchesInTextsByRegex(List<ScanTextResult> scanTextResults, Regex regex, int allowedNumberMatches) 
        {
            var results = scanTextResults;
            Trace.WriteLine("\nStart searching for matches in text using a regular expression...");

            // Add tasks
            List<Task<(ScanTextResult, string[]? matchesResult)>> findMatchesInTextsByRegexTasks = [];
            foreach (var result in results)
            {
                findMatchesInTextsByRegexTasks.Add(FindMatchesInTextsByRegex(result, regex, allowedNumberMatches));
            }

            // Start tasks
            var findMatchesInTextsByRegexTasksArray = findMatchesInTextsByRegexTasks.ToArray();
            List<(ScanTextResult, string[]? matchesResults)> findMatchesInTextsByRegexTasksResults = [];
            foreach (var task in findMatchesInTextsByRegexTasksArray)
            {
                await task;
                findMatchesInTextsByRegexTasksResults.Add(task.Result);
            }
            Task.WaitAll(findMatchesInTextsByRegexTasksArray);
            
            return findMatchesInTextsByRegexTasksResults;
        }

        private static async Task<(ScanTextResult, string[]? matchesResults)> FindMatchesInTextsByRegex(ScanTextResult scanTextResult, Regex regex, int allowedNumberMatches) 
        {
            var result = scanTextResult;
            string[]? matchesResult = null;

            MatchCollection? matches = regex.Matches(result.Text); 
            if(matches is null || matches.Count == 0) 
            {
                Trace.WriteLine("No matches found in text.");
            }

            if(matches!.Count > allowedNumberMatches) 
            {
                Trace.WriteLine($"Number of matches ({matches.Count}) is greater than expected ({allowedNumberMatches})\n" + 
                                $"Please check document named '{result.SourceFilePath}' and try again.\n");
            } 
            else 
            {
                matchesResult = new string [matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    if(matches[i].Success) 
                    {
                        matchesResult[i] = matches[i].Value;
                    }
                }
            }

            await Task.Delay(10);               // Dummy
            return (result, matchesResult);
        }       
    }
}