using System.Dynamic;

namespace CSharpHelpers.Models
{
    /// <summary>
    /// A model of a file saved in a file system.
    /// </summary>
    public class SavedFile(FileInfo fileInfo, string newFileName, string targetExtension, string fileAuthor, DirectoryInfo? fileSaveDirectory = null, DateTime? fileUpdateDate = null)
    {
        #region Properties
        public FileInfo FileInfo { get; } = fileInfo;
        public DirectoryInfo? FileSaveDirectory { get; } = fileSaveDirectory ?? fileInfo.Directory;
        public string NewFileName { get; } = newFileName;
        public string TargetExtension { get; } = targetExtension;
        public DateTime FileUpdateDate { get; } = fileUpdateDate ?? DateTime.Now;
        public string FileAuthor { get; } = fileAuthor;
        #endregion

        #region Funtionality
        public string GetNewfilenameWithTargetExtension() =>   
            string.Concat(NewFileName, TargetExtension);    

        public bool IsSourceAndTargetFileExtensionsEqual => 
            string.Equals(fileInfo.Extension, targetExtension);

        #endregion
        


    }
}