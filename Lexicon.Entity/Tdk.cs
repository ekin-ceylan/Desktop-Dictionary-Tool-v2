using HtmlAgilityPack;
using Lexicon.Core.EntityAbstractions;

namespace Lexicon.Entity
{
    public class Tdk : IWord
    {
        public Tdk(string word)
        {
            Address = @"http://tdk.gov.tr/index.php?option=com_gts&arama=gts&kelime=" + word + @"&uid=26607&guid=TDK.GTS.56c47c055e0e83.89258879";
        }

        public string Name { get; set; }
        public string[] Description { get; set; }
        public string Type { get; set; }
        public string Sound { get; set; }
        public string[] SampleSentence { get; set; }
        public string Address { get; private set; }
        public HtmlDocument HtmlDocument { get; set; }
        public string GetWords(HtmlDocument htmlDocument)
        {
            throw new System.NotImplementedException();
        }

        public string GetSound(HtmlDocument htmlDocument, string keyWord)
        {
            throw new System.NotImplementedException();
        }
    }
}
