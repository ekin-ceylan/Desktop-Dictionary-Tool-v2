using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using Lexicon.Core.Abstractions;
using Lexicon.Infrastructure;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Lexicon.Core.Entity
{
    public class TdkGts : IWord
    {
        private readonly RichTextHelper _rth = new RichTextHelper();
        private readonly StringHelper _stringHelper = new StringHelper();
        private HtmlDocument HtmlDocument { get; set; }

        public string Word { get; set; }

        private string GetAddress()
        {
            return @"http://tdk.gov.tr/index.php?option=com_gts&arama=gts&kelime=" + Word.Trim() + @"&uid=26607&guid=TDK.GTS.56c47c055e0e83.89258879";
        }

        public string GetDescriptionAsRichText()
        {
            if (HtmlDocument == null)
            {
                HtmlDocument = new HtmlWeb().Load(GetAddress());
            }

            return GetDescriptionAsRichText(HtmlDocument);
        }

        private string GetDescriptionAsRichText(HtmlDocument htmlDocument)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//*[@id='hor-minimalist-a']");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            RichTextBox rtb = new RichTextBox();

            if (htmlNodeCollection == null)
            {
                return _rth.RichTextWarning("\nNoResult");
            }

            for (int i = 0; i < htmlNodeCollection.Count; i++)
            {
                dictionary.Clear();
                HtmlNodeCollection trCollection = htmlNodeCollection[i].SelectNodes("tr");
                HtmlNode nd = htmlNodeCollection[i].SelectSingleNode("thead").SelectSingleNode("tr").SelectSingleNode("th");
                string word = nd.SelectSingleNode("b").InnerText.Trim();
                //string wordType = nd.SelectSingleNode("i").SelectSingleNode("b").InnerText.Trim();
                //HtmlNodeCollection nd = trCollection[0].SelectNodes("th");

                if (i > 0)
                {
                    rtb.AppendText("\n\n");
                }

                _rth.AppendRichTextBold(ref rtb, word);

                foreach (HtmlNode tableRow in trCollection)
                {
                    string str = tableRow.SelectSingleNode("td").InnerText.Trim();
                    string strh = tableRow.SelectSingleNode("td").InnerHtml;

                    if (str == "")
                    {
                        continue;
                    }

                    _rth.AppendRichTextRegular(ref rtb, " " + str);
                }
            }

            return _stringHelper.ClearText(rtb.Rtf);
        }

        public string GetDescriptionAsText()
        {
            if (HtmlDocument == null)
            {
                HtmlDocument = new HtmlWeb().Load(GetAddress());
            }

            return GetDescriptionAsText(HtmlDocument);
        }

        private string GetDescriptionAsText(HtmlDocument htmlDocument)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//*[@id='hor-minimalist-a']");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            StringBuilder rtb = new StringBuilder();

            if (htmlNodeCollection == null)
            {
                return Word + " için sonuç bulunamadı.";
            }

            for (int i = 0; i < htmlNodeCollection.Count; i++)
            {
                dictionary.Clear();
                HtmlNodeCollection trCollection = htmlNodeCollection[i].SelectNodes("tr");
                HtmlNode nd = htmlNodeCollection[i].SelectSingleNode("thead").SelectSingleNode("tr").SelectSingleNode("th");
                string word = nd.SelectSingleNode("b").InnerText.Trim();

                if (i > 0)
                {
                    rtb.AppendLine();
                    rtb.AppendLine();
                }

                rtb.Append(word);

                foreach (HtmlNode tableRow in trCollection)
                {
                    string str = tableRow.SelectSingleNode("td").InnerText.Trim();

                    if (str == "")
                    {
                        continue;
                    }

                    rtb.Append(" " + str);
                }
            }

            return _stringHelper.ClearText(rtb.ToString());
        }

        public string GetSound()
        {
            HtmlDocument htmlDocument = new HtmlWeb().Load(GetAddress());
            return GetSound(htmlDocument);
        }

        private string GetSound(HtmlDocument htmlDocument)
        {
            return null;
        }

        public bool IsWordExist()
        {
            if (HtmlDocument == null)
            {
                HtmlDocument = new HtmlWeb().Load(GetAddress());
            }

            return IsWordExist(HtmlDocument);
        }

        private bool IsWordExist(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@id='hor-minimalist-a']") != null;
        }

        public Word GetTranslation()
        {
            HtmlDocument htmlDocument = new HtmlWeb().Load(GetAddress());

            return new Word
            {
                RichText = GetDescriptionAsRichText(htmlDocument),
                Sound = GetSound(htmlDocument)
            };
        }
    }
}
