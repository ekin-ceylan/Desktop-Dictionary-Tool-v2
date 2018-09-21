using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lexicon.Infrastructure
{
    public static class Descriptions
    {
        internal static readonly Dictionary<string, object> DictionaryFont = new Dictionary<string, object>
        {
            {"bold", FontStyle.Bold},
            {"italic", FontStyle.Italic},
            {"regular", FontStyle.Regular},
            {"underline", FontStyle.Underline},
            {"center", HorizontalAlignment.Center},
            {"left", HorizontalAlignment.Left},
            {"right", HorizontalAlignment.Right}
        };

        public static readonly Dictionary<string, string> DictionaryLang = new Dictionary<string, string>
        {
            {"English", "en"},
            {"İngilizce", "en"},
            {"Türkçe", "tr"},
            {"Turkish", "tr"}
        };
    }
}
