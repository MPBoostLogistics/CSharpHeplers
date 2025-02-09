namespace CSharpHelpers.Extensions
{
    public static class CSharpExtensions
    {
        public static int GetQuarter(this DateOnly date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else if (date.Month >= 10 && date.Month <= 12)
                return 4;
            else 
                return 1;
        }

        public static (int quarter, int year) GetQuarterAndYear(this DateOnly date) => 
            (GetQuarter(date), date.Year);

        public static string GetQuarterAndYearString(this DateOnly date) 
            => $"{GetQuarter(date)} квартал {date.Year} года";


    }
}