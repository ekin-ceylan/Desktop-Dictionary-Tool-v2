using System;
using System.Windows.Forms;
using Lexicon.Core.Entity;
using Bus = Lexicon.Business.Business;

namespace Lexicon.WinForm
{
    public partial class Lexicon : Form
    {
        public Lexicon()
        {
            InitializeComponent();
        }

        private Bus _business;

        void Search()
        {
            Word word = new Word
            {
                RichText = _business.GetDescriptionsAsRichText(),
                Sound = _business.GetSound(),
                Text = _business.GetDescriptionsAsText()
            };
            rtbWord.Rtf = word.RichText;
            txtDesc.Text = word.Text;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _business = new Bus(new TdkGts { Word = txtWord.Text });
            Search();
        }

    }
}
