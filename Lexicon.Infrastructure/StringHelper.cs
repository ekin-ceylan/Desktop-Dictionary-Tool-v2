namespace Lexicon.Infrastructure
{
    public class StringHelper
    {
        public string ClearText(string dirty)
        {
            return dirty.Replace("&Ouml;", "Ö").Replace("&ouml;", "ö")
                        .Replace("&#214;", "Ö").Replace("&#246;", "ö")
                        .Replace("&Uuml;", "Ü").Replace("&uuml;", "ü")
                        .Replace("&#220;", "Ü").Replace("&#252;", "ü")
                        .Replace("&nbsp;", " ").Replace("&#160;", " ")
                        .Replace("&rsquo;", "'")
                        .Replace("&Ccedil;", "Ç").Replace("&ccedil;", "ç")
                        .Replace("&#199;", "Ç").Replace("&#231;", "ç")
                        .Replace("&#350;", "Ş").Replace("&#351;", "ş")
                        .Replace("&#39;", "'").Trim();
        }
    }
}
