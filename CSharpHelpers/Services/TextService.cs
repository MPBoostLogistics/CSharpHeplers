using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using CSharpHelpers.Models;

namespace CSharpHelpers.Services
{
    public class TextService : BaseService
    {
        private static readonly CultureInfo cultureInfo = new("ru-RU");

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
    
        public static bool FindDateInText(string text, Regex fileDateRegex, out DateTime? dateTime) 
        {
            dateTime = null;
            var match = fileDateRegex.Match(text);
            if(match.Success) 
            {
                dateTime = DateTime.Parse(match.Value.Trim(), cultureInfo);
            } 
            return dateTime is not null;
        }

        private static void UpdateScanTextResult(in DirectoryInfo? targetDirectoryInfo, in string scanManagerName, 
            in Regex fileDateRegex, ref ScanTextResult currentResult, string[] matches)
        {
            if (matches.FirstOrDefault() != null || !string.IsNullOrEmpty(matches.FirstOrDefault()))
            {
                var newFileName = matches.First();
     
                // Write target filePath
                if(targetDirectoryInfo is not null)
                    currentResult.SetTargetFilePath(targetDirectoryInfo.FullName);

                // Write scan manager name
                currentResult.SetScanManager(scanManagerName);

                // Write new file name
                currentResult.SetNewFilename(newFileName);

                if (FindDateInText(newFileName, fileDateRegex, out DateTime? createDateTime) &&
                    createDateTime is not null)
                {
                    // Write the document creation date
                    currentResult.SetFileCreatioDate((DateTime)createDateTime);
                }
            }
        }
    }
}