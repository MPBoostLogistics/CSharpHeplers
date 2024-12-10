using System.Diagnostics;

namespace CSharpHelpers.Services
{
    public class FileService
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
        /// <param name="extensions">Collection of file extensions to search for (optional).</param>
        /// <param name="fileInfos">File search results.</param>
        public static bool SearchFilesInDirectory(string dirPath, string[]? extensions, out FileInfo[]? fileInfos) 
        {
            fileInfos = null;

            if(GetDirectoryInfo(dirPath, out DirectoryInfo? directoryInfo)
                && directoryInfo is not null) 
            {
                fileInfos = directoryInfo.GetFiles();

                if(fileInfos is null || fileInfos.Length == 0) 
                {
                    Trace.WriteLine($"Directory at path\n\"{dirPath}\"\ndon`t contain any files.");
                } 
                else 
                {
                    if(extensions is not null && extensions.Length > 0) 
                    {
                        fileInfos = fileInfos
                                        .Where(fi => extensions.Contains(fi.Extension))
                                        .ToArray();
                    }
                }
            } 
            else 
            {
                Trace.WriteLine($"Directory at path\n\"{dirPath}\"\ndoes not exist.");
            }

            return fileInfos is not null && fileInfos.Length > 0;
        }

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