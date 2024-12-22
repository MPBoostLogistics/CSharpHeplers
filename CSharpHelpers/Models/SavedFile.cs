namespace CSharpHelpers.Models
{
    /// <summary>
    /// A model of a file saved in a file system.
    /// </summary>
    public class SavedFile(FileInfo fileInfo, string newFileName, string fileAuthor, DirectoryInfo? fileSaveDirectory = null, DateTime? fileUpdateDate = null)
    {
        public FileInfo FileInfo { get; } = fileInfo;
        public DirectoryInfo? FileSaveDirectory { get; } = fileSaveDirectory;
        public string NewFileName { get; } = newFileName;
        public DateTime FileUpdateDate { get; } = fileUpdateDate ?? DateTime.Now;
        public string FileAuthor { get; } = fileAuthor;
    }
}