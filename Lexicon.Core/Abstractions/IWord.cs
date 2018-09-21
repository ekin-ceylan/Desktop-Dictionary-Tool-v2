using System.Collections.Generic;
using HtmlAgilityPack;
using Lexicon.Core.Entity;

namespace Lexicon.Core.Abstractions
{
    public interface IWord
    {
        string Word { get; set; }
        string GetSound();
        string GetDescriptionAsRichText();
        string GetDescriptionAsText();
        bool IsWordExist();
        Word GetTranslation();
    }
}
