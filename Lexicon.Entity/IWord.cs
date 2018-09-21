using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon.Entity
{
    interface IWord
    {
        string Name { get; set; }
        string[] Description { get; set; }
        string Type { get; set; }
        string Sound { get; set; }
        string[] SampleSentence { get; set; }
        string Address { get;}
        HtmlDocument HtmlDocument { get; set; }


        string GetWords(HtmlDocument htmlDocument);
        string GetSound(HtmlDocument htmlDocument, string keyWord);

    }
}
