using System.Globalization;
using System.Text.RegularExpressions;

namespace PdfFileRenameFromContent.Application
{
    public class DateExtractor
    {
        private static readonly string[] StandardDateRegexPatterns = new[]
        {
            "^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)[0-9]{2}$",
            @"^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$",
            @"(0[1-9]|[12][0-9]|3[01])[\.-](0[1-9]|1[012])[\.-](19|20|)\d\d",
        };

        private static readonly string[] MonthNameDateRegexPatterns = new[]
        {
            @"((0[1-9]|[12][0-9]|3[01])([ ]{1}|\.{1}|\.[ ]{1})(Jan(uar)?|Feb(ruar)?|Mär(z)?|Apr(il)?|Mai|Jun(i)?|Jul(i)?|Aug(ust)?|Sep(tember)?|Okt(ober)?|Nov(ember)?|Dez(ember)?)\s+\d{4})",
            @"((0[1-9]|[12][0-9]|3[01])([ ]{1}|\.{1}|\.[ ]{1})(?<Month>((Jan(uar)?)|(Feb(ruar)?)|(Mär(z)?)|(Apr(il)?)|(Mai)|(Juni?)|(Juli?)|(Aug(ust)?)|(Sep(t(ember)?)?)|(Okt(ober)?)|(Nov(ember)?)|(Dez(ember)?)))[ ](19|20)[0-9]{2})$",
            @"(([1-9]|[12][0-9]|3[01])([ ]{1}|\.{1}|\.[ ]{1})(Jan(uar)?|Feb(ruar)?|Mär(z)?|Apr(il)?|Mai|Jun(i)?|Jul(i)?|Aug(ust)?|Sep(tember)?|Okt(ober)?|Nov(ember)?|Dez(ember)?)\s+\d{4})"
        };

        private static readonly string[] MonthNameWithoutDayDateRegexPatterns = new[]
        {
            @"((Jan(uar)?|Feb(ruar)?|Mär(z)?|Apr(il)?|Mai|Jun(i)?|Jul(i)?|Aug(ust)?|Sep(tember)?|Okt(ober)?|Nov(ember)?|Dez(ember)?)\s+\d{4})"
        };

        public DateTime GetDate(string content)
        {
            var foundDateParts = GetDateStringByRegexPattern(content, StandardDateRegexPatterns, StandardDateSplitter)
                .Union(GetDateStringByRegexPattern(content, MonthNameDateRegexPatterns, ds => MonthNameDateSplitter(ds?.Replace("\n", string.Empty).Replace(",", "."))))
                .Union(GetDateStringByRegexPattern(content, MonthNameWithoutDayDateRegexPatterns, MonthNameWithoutDayDateSplitter))
                .Select(ConvertToDateTime).ToList();

            if (foundDateParts.Count == 0)
            {
                return DateTime.MinValue;
            }

            var maxDateOfPast = foundDateParts.Where(d => d < DateTime.Now).Max();
            return maxDateOfPast;
        }

        public DateTime GetDate(string content, Func<int, string> getPageFunc)
        {
            var date = GetDate(content);


            int pageNum = 2;
            while (date == DateTime.MinValue && pageNum < 5)
            {
                var contentOfPageX = getPageFunc(pageNum++);
                date = GetDate(contentOfPageX);
            }

            return date;
        }

        private static string[] MonthNameDateSplitter(string dateString)
        {
            string[] dateParts;
            var separator = new string[] { ". ", " " };
            dateParts = dateString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            dateParts[1] = DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month.ToString();
            return dateParts;
        }

        private static string[] StandardDateSplitter(string dateString)
        {
            return dateString.Split(new[] { '.', '-' });
        }

        private static string[] MonthNameWithoutDayDateSplitter(string dateString)
        {
            var separator = new string[] { ". ", " ", "\u00A0" }; // \u00A0 = non-breaking space
            var incompleteDateParts = dateString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var dateParts = new[] { "1", incompleteDateParts[0], incompleteDateParts[1] };
            if (dateParts[1].Length == 3)
            {
                dateParts[1] = DateTime.ParseExact(dateParts[1], "MMM", CultureInfo.CurrentCulture).Month.ToString();
            }
            else
            {
                dateParts[1] = DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month.ToString();
            }

            return dateParts;
        }

        private static DateTime ConvertToDateTime(string[] dateParts)
        {
            var day = Convert.ToInt32(dateParts[0]);
            var month = Convert.ToInt32(dateParts[1]);
            var year = Convert.ToInt32(dateParts[2]);
            if (year < 50)
            {
                year += 2000;
            }

            if (year >= 50 && year < 100)
            {
                year += 1900;
            }

            var dateValue = new DateTime(year, month, day);
            return dateValue;
        }

        private static ICollection<string[]> GetDateStringByRegexPattern(string content, string[] dateRegexPatterns, Func<string, string[]> dateSplittingFunction)
        {
            var x = dateRegexPatterns.Select(p => new Regex(p).Matches(content));
            var matchedDates = x.SelectMany(p => p.Cast<Match>().ToList()).ToList();
            var dates = matchedDates.Select(m => dateSplittingFunction(m.Value)).ToList();
            return dates;
        }

        private static string[] GetDateStringByRegexPatternOld(string content, string[] dateRegexPatterns, Func<string, string[]> dateSplittingFunction)
        {
            var bestMatchingPattern = dateRegexPatterns.FirstOrDefault(p => new Regex(p).Match(content)?.Success ?? false);

            if (bestMatchingPattern == null)
            {
                return null;
            }

            Regex rgx = new Regex(bestMatchingPattern);
            //Match mat = rgx.Match(content);
            Match mat = MatchWithoutBirthdate(rgx, content);
            string dateString = mat?.ToString();

            string[] dateParts = null;
            if (!string.IsNullOrEmpty(dateString))
            {
                dateParts = dateSplittingFunction(dateString);
            }

            return dateParts;
        }

        private static Match MatchWithoutBirthdate(Regex rgx, string content)
        {
            var matches = rgx.Matches(content);
            foreach (Match match in matches)
            {
                if (match.ToString() != "06.10.1975")
                {
                    return match;
                }
            }

            return null;
        }
    }
}