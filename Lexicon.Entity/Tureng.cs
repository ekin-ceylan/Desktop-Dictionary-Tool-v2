using Lexicon.Core.EntityAbstractions;

namespace Lexicon.Entity
{
    public class Tureng : Word, IWordType, ISound
    {
        public string Type { get; set; }
        public string Sound { get; set; }
    }
}
