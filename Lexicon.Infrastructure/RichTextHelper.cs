using System.Drawing;
using System.Windows.Forms;

namespace Lexicon.Infrastructure
{
    public class RichTextHelper
    {
        public string RichTextWarning(string text)
        {
            RichTextBox rtb = new RichTextBox {SelectionAlignment = HorizontalAlignment.Center};
            rtb.AppendText(text);
            return rtb.Rtf;
        }

        public string AppendRichText(ref string richText, string text, float size, string alignment, string fontStyle)
        {
            RichTextBox rtb = new RichTextBox
            {
                Rtf = richText,
                SelectionFont = new Font("Microsoft Sans Serif", size, (FontStyle)Descriptions.DictionaryFont[fontStyle]),
                SelectionAlignment = (HorizontalAlignment)Descriptions.DictionaryFont[alignment]
            };
            rtb.AppendText(text);
            return richText = rtb.Rtf;
        }


        public void AppendRichTextRegular(ref RichTextBox richTextBox, string text)
        {
            richTextBox.SelectionFont = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Regular);
            richTextBox.AppendText(text);
        }

        public void AppendRichTextBold(ref RichTextBox richTextBox, string text)
        {
            richTextBox.SelectionFont = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Bold);
            richTextBox.AppendText(text);
        }

        public void AppendRichTextItalic(ref RichTextBox richTextBox, string text)
        {
            richTextBox.SelectionFont = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Italic);
            richTextBox.AppendText(text);
        }

        public void AppendRichTextCenter(ref RichTextBox richTextBox, string text)
        {
            richTextBox.SelectionFont = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Italic);
            richTextBox.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox.AppendText(text);
        }

        public void AppendRichTextLeft(ref RichTextBox richTextBox, string text)
        {
            richTextBox.SelectionFont = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Regular);
            richTextBox.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox.AppendText(text);
        }

    }
}
