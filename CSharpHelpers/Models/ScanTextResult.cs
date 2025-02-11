namespace CSharpHelpers.Models
{
    public class ScanTextResult(string filePath, string text, double scanTime, bool isConvertToPdfRequired)
    {
        #region Properties
        
        public string FilePath { get; private set; } = filePath;
        public string Text { get; private set; } = text;
        public double ScanTime { get; private set; } = scanTime;
        public bool IsConvertToPdfRequired { get; private set; } = isConvertToPdfRequired;
        public string? NewFileName { get; private set;} = null;
        public DateOnly? NewFileDate { get; private set;} = null;
        public string ScanManager { get; private set;} = string.Empty;
        public string? PathToSubdirForSaving =>
            NewFileDate.HasValue ?  
            $"/{NewFileDate.Value.Year}/{NewFileDate.Value.Month} {NewFileDate.Value.Year}" : 
            null;
   
        #endregion

        #region Functionality
        public void SetNewFilename(string newFileName) => NewFileName = newFileName.Trim();
        public void SetFileCreatioDate(DateOnly newFileDate) => NewFileDate = newFileDate;
        public void SetScanManager(string scanManager) => ScanManager = scanManager;
        #endregion
    }
}