namespace CSharpHelpers.Models
{
    public class ScanTextResult(string filePath, string text, double scanTime, bool isConvertToPdfRequired)
    {
        #region Properties
        
        public string SourceFilePath { get; private set; } = filePath;
        public string? TargetFilePath { get; private set; } = string.Empty;
        public string Text { get; private set; } = text;
        public double ScanTime { get; private set; } = scanTime;
        public bool IsConvertToPdfRequired { get; private set; } = isConvertToPdfRequired;
        public string? NewFileName { get; private set;} = null;
        public DateTime? NewFileDate { get; private set;} = null;
        public string ScanManager { get; private set;} = string.Empty;
        public string? PathToSubdirForSaving =>
            NewFileDate.HasValue ?  
            $"/{NewFileDate.Value.Year}/{NewFileDate.Value.Month} {NewFileDate.Value.Year}/" : 
            null;
   
        #endregion

        #region Functionality
        public void SetNewFilename(string newFileName) => NewFileName = newFileName.Trim();
        public void SetFileCreatioDate(DateTime newFileDate) => NewFileDate = newFileDate;
        public void SetScanManager(string scanManager) => ScanManager = scanManager;
        public void SetTargetFilePath (string targetFilePath) => TargetFilePath = targetFilePath;

        #endregion
    }
}