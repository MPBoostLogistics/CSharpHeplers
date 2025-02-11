namespace CSharpHelpers.Models
{
    public readonly struct SavedFileSettings (bool isOverwriteExistingFile, bool isDeleteSourceFile)
    {
        public bool IsOverwriteExistingFile { get; } = isOverwriteExistingFile;
        public bool IsDeleteSourceFile { get ; } = isDeleteSourceFile;
    }

    /// <summary>
    /// A model of a file saved in a file system.
    /// </summary>
    public class SavedFile
    {
        #region Properties
        public FileInfo SourceFileInfo { get; }
        public DirectoryInfo? FileTargetSaveDirectory { get; }
        public string NewFileName { get; }
        public string TargetExtension { get; }
        public DateTime FileUpdateDate { get; }
        public string FileAuthor { get; }
        public SavedFileSettings SavedFileSettings { get; }
        #endregion

        #region Constructors 
        public SavedFile(FileInfo sourceFileInfo, string newFileName, string targetExtension, string fileAuthor, SavedFileSettings savedFileSettings, DirectoryInfo? fileSaveDirectory = null, DateTime? fileUpdateDate = null) 
        {
            SourceFileInfo = sourceFileInfo;
            FileTargetSaveDirectory = fileSaveDirectory ?? sourceFileInfo.Directory;
            NewFileName = newFileName;
            TargetExtension = targetExtension;
            FileUpdateDate = fileUpdateDate ?? SourceFileInfo.CreationTime;
            FileAuthor = fileAuthor;
            SavedFileSettings = savedFileSettings;
        }

        public SavedFile(ScanTextResult scanTextResult, string targetExtension, SavedFileSettings savedFileSettings) 
        {
            SourceFileInfo = new FileInfo(scanTextResult.SourceFilePath);

            if(!string.IsNullOrEmpty(scanTextResult.TargetFilePath))
                FileTargetSaveDirectory = new DirectoryInfo(scanTextResult.TargetFilePath);

            NewFileName = scanTextResult.NewFileName ?? SourceFileInfo.Name;
            TargetExtension = targetExtension;
            FileUpdateDate = scanTextResult.NewFileDate ?? SourceFileInfo.CreationTime;
            FileAuthor = scanTextResult.ScanManager;
            SavedFileSettings = savedFileSettings;
        }
        #endregion

        #region Funtionality
        public string GetNewfilenameWithTargetExtension() => $"{NewFileName}{TargetExtension}";

        public string GetFullFileTargetSaveDirectory() => 
            FileTargetSaveDirectory is null ? 
            string.Empty : 
            Path.Combine(FileTargetSaveDirectory.FullName,$"{FileUpdateDate.Year}",$"{FileUpdateDate.Month} {FileUpdateDate.Year}");

        #endregion
    }
}