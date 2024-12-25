using System.Reflection;
using System.Text;

namespace CSharpHelpers.Services 
{
    public class ConsoleAppService : BaseService
    {
        #region Variables and constants
        // ...
        #endregion

        #region Properties 
        // ...    
        #endregion

        #region Constructors
        // ...
        #endregion

        #region Functionality
        /// <summary>
        /// Gets string data from user from console.
        /// </summary>
        /// <param name="message">Explanation for user.</param>
        /// <returns>Received string data.</returns>
        public static string GetUserStringFromConsole(string message) 
        {
            string? result;
            do
            {
                Console.Clear();
                Console.WriteLine(message);
                result = Console.ReadLine();
            } while(string.IsNullOrEmpty(result));

            return !string.IsNullOrEmpty(result) ? result.Trim() : string.Empty;
        }
        
        /// <summary>
        /// Gets keystroke from user from console.
        /// </summary>
        /// <param name="message">Explanation for user.</param>
        /// <returns>Received keystroke.</returns>
        public static ConsoleKey GetUserKeyFromConsole(string message) 
        {
            ConsoleKey? result;
            do
            {
                Console.Clear();
                Console.WriteLine(message);
                result = Console.ReadKey().Key;
            } while(result is null);

            return (ConsoleKey)result;
        }
        
        /// <summary>
        /// Get console app to exit with a message.
        /// </summary>
        /// <param name="lastMessage">Last message.</param>
        /// <param name="infoMessage">Info message.</param>
        public static void ConsoleAppExit(string lastMessage, string? infoMessage = "")
        {
            var builder = new StringBuilder();

            if(!string.IsNullOrEmpty(infoMessage))
            {
                builder.Append($"{infoMessage}\n{lastMessage}");
            } 
            else 
            {
                builder.Append($"{lastMessage}");
            }

            Console.WriteLine(builder.ToString());
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
        #endregion
    }
}