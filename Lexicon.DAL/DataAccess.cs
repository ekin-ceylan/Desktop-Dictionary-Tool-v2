using System.Collections.Generic;
using Lexicon.Core.Abstractions;
using Lexicon.Core.Entity;
namespace Lexicon.DAL
{
    public class DataAccess
    {
        private readonly IWord _word;

        public DataAccess(IWord word)
        {
            _word = word;
        }

        public List<Word> GetDescriptions()
        {
            return _word.GetWords();
        }

        public string GetDescriptionsAsRichText()
        {
            
        }

        public string GetSound()
        {

        }


    }
}
