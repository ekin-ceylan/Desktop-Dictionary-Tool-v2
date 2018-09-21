using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using Lexicon.Core.Abstractions;
using Lexicon.Infrastructure;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Lexicon.Core.Entity
{
    public class Tureng : IWord
    {
        private readonly RichTextHelper _rth = new RichTextHelper();
        private readonly StringHelper _stringHelper = new StringHelper();
        private HtmlDocument HtmlDocument { get; set; }

        public string Word { get; set; }

        private string GetAddress()
        {
            return @"http://www.tureng.com/search/" + Word;
        }


        public string GetDescriptionAsRichText()
        {
            if (HtmlDocument == null)
                HtmlDocument = new HtmlWeb().Load(GetAddress());

            return GetDescriptionAsText(HtmlDocument);
        }
        private string GetDescriptionAsRichText(HtmlDocument htmlDocument)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//*[@id='englishResultsTable']");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            RichTextBox rtb = new RichTextBox();

            if (htmlNodeCollection == null)
                return _rth.RichTextWarning("\nNo result");

            string lang1 = "";

            for (int i = 0; i < htmlNodeCollection.Count; i++)
            {
                dictionary.Clear();
                HtmlNodeCollection trCollection = htmlNodeCollection[i].SelectNodes("tr");
                HtmlNodeCollection nd = trCollection[0].SelectNodes("th");

                if (lang1 == _stringHelper.ClearText(nd[2].InnerText) || i == 2)
                    continue;

                lang1 = _stringHelper.ClearText(nd[2].InnerText);
                string lang2 = _stringHelper.ClearText(nd[3].InnerText);

                if (i > 0)
                    rtb.AppendText("\n\n");

                _rth.AppendRichTextItalic(ref rtb, "              " + lang1 + " - " + lang2);


                foreach (HtmlNode tableRow in trCollection)
                {
                    HtmlNode temptd = tableRow.SelectSingleNode("*[@lang='" + Descriptions.DictionaryLang[lang2] + "']");

                    if (temptd == null)
                        continue;

                    string str = temptd.ChildNodes[0].InnerText;
                    string key = tableRow.SelectSingleNode("*[@class='hidden-xs']").InnerText;

                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key] += (", " + str);
                        rtb.AppendText(", " + str);
                    }
                    else
                    {
                        dictionary.Add(key, str);
                        _rth.AppendRichTextBold(ref rtb, "\n" + key.Replace('İ', 'I') + ": ");
                        _rth.AppendRichTextRegular(ref rtb, str);
                    }
                }
            }
            return _stringHelper.ClearText(rtb.Rtf);
        }



        public string GetDescriptionAsText()
        {
            if (HtmlDocument == null)
                HtmlDocument = new HtmlWeb().Load(GetAddress());

            return GetDescriptionAsText(HtmlDocument);
        }
        private string GetDescriptionAsText(HtmlDocument htmlDocument)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//*[@id='hor-minimalist-a']");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            StringBuilder rtb = new StringBuilder();

            if (htmlNodeCollection == null)
                return "\nNoResult";

            for (int i = 0; i < htmlNodeCollection.Count; i++)
            {
                dictionary.Clear();
                HtmlNodeCollection trCollection = htmlNodeCollection[i].SelectNodes("tr");
                HtmlNode nd = htmlNodeCollection[i].SelectSingleNode("thead").SelectSingleNode("tr").SelectSingleNode("th");
                string word = nd.SelectSingleNode("b").InnerText.Trim();

                if (i > 0)
                    rtb.AppendLine();

                rtb.Append(word);

                foreach (HtmlNode tableRow in trCollection)
                {
                    string str = tableRow.SelectSingleNode("td").InnerText.Trim();

                    if (str == "")
                        continue;

                    rtb.Append(" " + str);

                }
            }
            return _stringHelper.ClearText(rtb.ToString());
        }


        public string GetSound()
        {
            if (HtmlDocument == null)
                HtmlDocument = new HtmlWeb().Load(GetAddress());

            return GetSound(HtmlDocument);
        }
        private string GetSound(HtmlDocument htmlDocument)
        {
            HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='turengVoiceENTRENus']");
            return htmlNode == null ? null : htmlNode.SelectSingleNode("source").GetAttributeValue("src", null);
        }
        

        public bool IsWordExist()
        {
            if (HtmlDocument == null)
                HtmlDocument = new HtmlWeb().Load(GetAddress());

            return IsWordExist(HtmlDocument);
        }
        private bool IsWordExist(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@id='englishResultsTable']") != null;
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
