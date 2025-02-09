using System.Globalization;
using CSharpHelpers.Extensions;

namespace CSharpHelpers.Models
{
    public class CreationDocumentDate 
    {
        #region Variables and constants
        private readonly CultureInfo _dateOnlyCultureInfo;
        #endregion

        #region Properties 
        public DateOnly Date { get; set; }
        public string QuarterAndYearString { get; }
        #endregion
        
        public CreationDocumentDate(CultureInfo dateOnlyCultureInfo, string dateOnlyString)
        {
            _dateOnlyCultureInfo = dateOnlyCultureInfo;
            if(DateOnly.TryParse(dateOnlyString, _dateOnlyCultureInfo, out var date)) 
            {
                Date = date;
                QuarterAndYearString = Date.GetQuarterAndYearString();
            } 
            else 
            {
                throw new FormatException($"String in format '{dateOnlyString}' can't be converted to date.");
            }
        }
    }

    public interface IScanTextResult 
    {
        public CreationDocumentDate CreationDate { get; set; }
    }


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