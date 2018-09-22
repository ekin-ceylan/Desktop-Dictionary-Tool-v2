using System;
using System.Net;
using Lexicon.Core.Abstractions;
using Lexicon.Infrastructure;

namespace Lexicon.Business
{
    public class Business
    {
        public IWord Word;
        private readonly RichTextHelper _rth = new RichTextHelper();

        public Business(IWord word)
        {
            Word = word;
        }

        public string GetSound()
        {
            try
            {
                return Word.GetSound();
            }
            catch (WebException)
            {
                return _rth.RichTextWarning("\nUnable to reach the website");
            }
            catch (Exception ex)
            {
                return _rth.RichTextWarning(ex.Message);
            }
        }

        public string GetDescriptionsAsRichText()
        {
            try
            {
                return Word.GetDescriptionAsRichText();
            }
            catch (WebException)
            {
                return _rth.RichTextWarning("\nUnable to reach the website");
            }
            catch (Exception ex)
            {
                return _rth.RichTextWarning(ex.Message);
            }
        }

        public string GetDescriptionsAsText()
        {
            try
            {
                return Word.GetDescriptionAsText();
            }
            catch (WebException)
            {
                return "\nUnable to reach the website";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
