using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CSharpHelpers.Services
{
    public class FileService
    {
        #region Variables and constants
        public const string FILEEXTENSION_JPG = ".jpg";
        public const string FILEEXTENSION_JPEG = ".jpeg";
        public const string FILEEXTENSION_PDF = ".pdf";
        #endregion

        #region Properties 
        // ...    
        #endregion

        #region Constructors
        // ... 
        #endregion

        #region Functionality

        /// <summary>
        /// Returns DirectoryInfo object of current project.
        /// </summary>
        /// <param name="directoryInfo">DirectoryInfo object found (optional).</param>
        /// <returns>Operation execution flag.</returns>
        /// <exception cref="DirectoryNotFoundException">Exception if directory not found.</exception>
        public static bool GetProjectDirectory(out DirectoryInfo? directoryInfo) 
        {
            DirectoryInfo? getParentworkingDirectory = Directory.GetParent(Environment.CurrentDirectory) ?? throw new DirectoryNotFoundException();
            var projectDirectoryPath = getParentworkingDirectory.Parent!.Parent!.FullName;

            return GetDirectoryInfo(projectDirectoryPath, out directoryInfo);
        }

        /// <summary>
        /// Get a 'DirectoryInfo' object given a directory path string.
        /// </summary>
        /// <param name="dirPath">Directory path string.</param>
        /// <param name="directoryInfo">'DirectoryInfo' object or null.</param>
        /// <returns>Search execution flag.</returns>
        public static bool GetDirectoryInfo(string dirPath, out DirectoryInfo? directoryInfo) 
        {
            var dirPathString = dirPath;

            if(dirPath.Contains('\'')) 
                dirPathString = dirPath.Replace("'", "");
            
            directoryInfo =  Directory.Exists(dirPathString) ? new DirectoryInfo(dirPathString) : null;

            return directoryInfo is not null;
        }

        /// <summary>
        /// Search files in a directory by extensions. 
        /// </summary>
        /// <param name="dirPath">Directory path string.</param>
        /// <param name="fileInfos">File search results.</param>
        /// <param name="extensions">Collection of file extensions to search for (optional).</param>
        public static bool SearchFilesInDirectory(string dirPath, out FileInfo[]? fileInfos, string[]? extensions = null) 
        {
            fileInfos = null;

            var dirSuccess = GetDirectoryInfo(dirPath, out DirectoryInfo? directoryInfo);
            if(dirSuccess is false || directoryInfo is null) 
            {
                Trace.WriteLine($"Unable to find dir at path '{dirPath}'");
                return false;
            }

            fileInfos = directoryInfo.GetFiles();
            if(fileInfos is null || fileInfos.Length == 0) 
            {
                Trace.WriteLine($"There are no files atpath '{dirPath}'");
                return false;
            }

            if(extensions is not null && extensions.Length != 0) 
            {
                fileInfos = fileInfos
                                .Where(fi => extensions.Contains(fi.Extension))
                                .ToArray();
            }

            return fileInfos is not null && fileInfos.Length > 0;
        }

        /// <summary>
        /// Search files in a directory by name pattern. 
        /// </summary>
        /// <param name="fileInfos">Files to search.</param>
        /// <param name="fileNameRegex">Regular expression for file search.</param>
        /// <returns>Lists of matching and non-matching files.</returns>
        public static (List<FileInfo> matchList, List<FileInfo> missMatchList) SearchFilesInDirectory(in FileInfo[] fileInfos, Regex fileNameRegex) 
        {
            List<FileInfo> matchList = [], missMatchList = [];

            foreach (var fileInfo in fileInfos) 
            {
                var match = fileNameRegex.Match(fileInfo.Name);
                if(!match.Success)
                    missMatchList.Add(fileInfo);
                else 
                    matchList.Add(fileInfo);
            }

            return (matchList, missMatchList);  
        }

/*

 internal static (List<FileInfo> matchList, List<FileInfo> missMatchList) GetFilesByNameMatchingPattern(in List<FileInfo> fileInfos, Regex fileNameRegex) 
        {
            List<FileInfo> matchList = [], missMatchList = [];
            
            foreach (var fileInfo in fileInfos) 
            {
                var match = fileNameRegex.Match(fileInfo.Name);
                if(!match.Success) 
                    missMatchList.Add(fileInfo);
                else 
                    matchList.Add(fileInfo);
            }

            return (matchList, missMatchList);  
        }

*/



        /// <summary>
        /// Deletes file at path.
        /// </summary>
        /// <param name="filePath">Path to file..</param>
        /// <returns>Delete execution flag.</returns>
        public static bool DeleteFileAtPath(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Trace.WriteLine($"File at path\n\"{filePath}\"\nsuccessfully deleted.");
            }
            else 
            {
                Trace.WriteLine($"File at path\n\"{filePath}\"\nnot found.");
            }
                
            return !File.Exists(filePath);
        }

        #endregion
    }
}