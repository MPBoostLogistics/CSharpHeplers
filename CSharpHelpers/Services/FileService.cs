using System.Diagnostics;
using System.Text.RegularExpressions;
using CSharpHelpers.Models;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace CSharpHelpers.Services
{
    public class FileService : BaseService
    {
        #region Variables and constants
        public const string FILEEXTENSION_JPG = ".jpg";
        public const string FILEEXTENSION_JPEG = ".jpeg";
        public const string FILEEXTENSION_PDF = ".pdf";
        public const string GUID_REGEX_PATTERN = @"[0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12}";
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
        /// Gets or creates a DirectoryInfo object for a given directory path string.
        /// </summary>
        /// <param name="dirPath">Directory path string.</param>
        /// <param name="directoryInfo">'DirectoryInfo' object or null.</param>
        /// <param name="isCreateNewDirectory">'Сreate a new directory if it don't exist.</param>
        /// <returns>Operation execution flag.</returns>
        public static bool GetDirectoryInfo(string dirPath, out DirectoryInfo? directoryInfo, bool isCreateNewDirectory = false) 
        {
            var dirPathString = dirPath;

            if(dirPath.Contains('\'')) 
                dirPathString = dirPath.Replace("'", "");

            switch(Directory.Exists(dirPathString)) 
            {
                case true:
                    directoryInfo = new DirectoryInfo(dirPathString);
                    break;
                case false:
                    directoryInfo = isCreateNewDirectory switch
                    {
                        true => Directory.CreateDirectory(dirPathString),
                        _ => null,
                    };
                    break;
            }

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
                fileInfos = [.. fileInfos.Where(fi => extensions.Contains(fi.Extension))];
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

         public static Task<string> CreateAndSavePdfDocument(SavedFile savedFile) 
        {
            return Task.Run(() => {

                var outputDirPath = savedFile.GetFullFileTargetSaveDirectory();

                if(GetDirectoryInfo(outputDirPath, out DirectoryInfo? directoryInfo, true) && 
                    directoryInfo is not null) 
                {
                    var savePath = $"{directoryInfo.FullName}/{savedFile.NewFileName}{savedFile.TargetExtension}";
                    PdfDocument pdf = new(new PdfWriter(savePath));
                    
                    using Document? document = new(pdf);

                    //Update encoded image
                    byte[] data = Convert.FromBase64String(savedFile.SourceFileInfo.FullName);
                    ImageData imageData = ImageDataFactory.Create(data) ?? throw new Exception("imageData is null");
                    Image? image = new(imageData);
                    
                    document.Add(image);
                    document.Close();

                    return savePath;
                } 
                else 
                {
                    return string.Empty;
                }
            });
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

        /// <summary>
        /// Renames a file according to given 'Save File' model.
        /// </summary>
        /// <param name="savedFile">'Save File' model.</param>
        /// <returns>Operation execution flag.</returns>
        public static bool RenameOrRemoveFile(SavedFile savedFile)
        {
            var targetDirPath = savedFile.FileTargetSaveDirectory?.FullName; 

            if (string.IsNullOrEmpty(targetDirPath)) return false; 

            string? newFileName = savedFile.GetNewfilenameWithTargetExtension();
            string? newFilePath = Path.Combine(targetDirPath, newFileName);

            File.Move(savedFile.SourceFileInfo.FullName, newFilePath);
            File.SetCreationTime(newFilePath, savedFile.FileUpdateDate);
            File.SetLastWriteTime(newFilePath, savedFile.FileUpdateDate);

            // TODO: Update file author 

            return File.Exists(newFilePath) && 
                File.GetCreationTime(newFilePath) == savedFile.FileUpdateDate;
        }

        #endregion
    }
}