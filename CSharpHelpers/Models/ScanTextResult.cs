namespace CSharpHelpers.Models
{
    public class ScanTextResult(string filePath, string text, double scanTime, bool isConvertToPdfRequired)
    {
        #region Properties
        
        public string FilePath { get; private set; } = filePath;
        public string Text { get; private set; } = text;
        public double ScanTime { get; private set; } = scanTime;
        public bool IsConvertToPdfRequired { get; private set; } = isConvertToPdfRequired;

        #endregion

    }
}