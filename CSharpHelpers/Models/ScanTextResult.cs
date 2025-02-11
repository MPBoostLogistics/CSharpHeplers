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
        #endregion

        #region Functionality
        public void Set(string newFileName) => NewFileName = newFileName.Trim();
        public void Set(DateOnly newFileDate) => NewFileDate = newFileDate;
        #endregion
    }
}