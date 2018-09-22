using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
//using HtmlAgilityPack;
using Lexicon.Core.Entity;
using Lexicon.Dictionary.Properties;
//using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Lexicon.Infrastructure;
using Bus = Lexicon.Business.Business;

namespace Lexicon.Dictionary
{
    public partial class DesktopDictionary : Form
    {
        public DesktopDictionary()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //threadStart = Search;
            Region = Region.FromHrgn(WinFormHelper.CreateRoundRectRgn(0, 0, Width = _width[1], Height = _height[1], 10, 10));

            _clm = (ToolStripMenuItem)ctxItemColor.DropDownItems[Settings.Default.Color] ?? ctxItemColorLime;
            _tsm = (ToolStripMenuItem)ctxItemTransparent.DropDownItems[Settings.Default.Transparency] ?? ctxItemTransparent0;
            _dcb = btnEn;
            txtWord.Text = Settings.Default.Welcome ?? "welcome";

            _clm.Checked = true;
            BackgroundImage = _clm.BackgroundImage;

            _tsm.Checked = true;
            Opacity = Convert.ToDouble(_tsm.Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });

            WinFormHelper.Appname = Assembly.GetExecutingAssembly().FullName.Remove(Assembly.GetExecutingAssembly().FullName.IndexOf(","));
            _startupPathName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), WinFormHelper.Appname + ".lnk");

            ctxItemStartup.Checked = WinFormHelper.CreateShortcut(_startupPathName, Settings.Default.RunAtStartup);
            ctxItemAlwaysTop.Checked = (TopMost = Settings.Default.AlwaysOnTop);
        }

        private Bus _business;


        private ToolStripMenuItem _tsm;
        private ToolStripMenuItem _clm;
        private Button _dcb;
        private bool _isTr;
        private readonly int[] _height = { 291, 55 };
        private readonly int[] _width = { 180, 148 };
        private readonly string _version = "1.2";
        private bool _setFired;
        private readonly string _startupPathName;
        private bool _isProsessing;
        //private readonly Thread _thread;
        //private ThreadStart threadStart;


        private const int CsDropshadow = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CsDropshadow;
                return cp;
            }
        }

        private void ShowTextArea(string text)
        {
            txtTranslation.Rtf = text;

            if (Height == _height[1])
            {
                Region = Region.FromHrgn(WinFormHelper.CreateRoundRectRgn(0, 0, Width = _width[0], Height = _height[0], 10, 10));
                Location = new Point(Location.X - (Width - _width[1]), Location.Y);
            }

            Activate();
            txtWord.Focus();
            txtWord.SelectAll();
            txtTranslation.Focus();
            Opacity = 1;
            UpdateStyles();
            //txtTranslation.ForceRefresh();
        }

        private void Search()
        {
            Word word = new Word
            {
                RichText = _business.GetDescriptionsAsRichText(),
                Sound = _business.GetSound(),
                Text = _business.GetDescriptionsAsText()
            };
            ShowTextArea(word.RichText);
            _isProsessing = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!_isProsessing && txtWord.Text != "")
            {
                _isProsessing = true;
                _business = new Bus(new TdkGts { Word = txtWord.Text });
                Thread thread = new Thread(Search) { IsBackground = true };
                thread.Start();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WinFormHelper.ReleaseCapture();
                WinFormHelper.SendMessage(Handle, WinFormHelper.WmNclbuttondown, WinFormHelper.HtCaption, 0);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ctxMenu.Visible = true;
                ctxMenu.Location = MousePosition;
            }
        }

        private void txtWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnSearch_Click(this, new EventArgs());
                e.Handled = true;
            }
        }

        private void ctxItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ctxItemAlwaysTop_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            tsm.Checked = !tsm.Checked;
            TopMost = !TopMost;
        }

        private void ctxMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void ctxItemTransparent_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _tsm.Checked = false;
            _tsm = (ToolStripMenuItem)e.ClickedItem;
            _tsm.Checked = true;
            Opacity = Convert.ToDouble(_tsm.Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });
        }

        private void ctxItemAbout_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = new RichTextBox { SelectionAlignment = HorizontalAlignment.Center };
            //var version = System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed ?
            //    System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion :
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            rtb.AppendText("\n\nSimple desktop dictionary tool for Windows");
            rtb.AppendText("\nVersion: " + _version + " (2015)");
            rtb.AppendText("\n\nDeveloped by Ekin Ceylan");
            rtb.AppendText("\nekinceylan@gmail.com");
            ShowTextArea(rtb.Rtf);
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {
            Opacity = 1;
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            int x = MousePosition.X - Location.X;
            int y = MousePosition.Y - Location.Y;
            if (Height == _height[1] && (x < 0 || x + 1 >= Width || y < 0 || y + 1 >= Height))
                Opacity = Convert.ToDouble(_tsm.Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Location = new Point(Location.X + (Width - _width[1]), Location.Y);
            Region = Region.FromHrgn(WinFormHelper.CreateRoundRectRgn(0, 0, Width = _width[1], Height = _height[1], 10, 10));
            Opacity = Convert.ToDouble(_tsm.Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });
            txtTranslation.Clear();
        }

        private void txtTranslation_MouseHover(object sender, EventArgs e)
        {
            txtTranslation.Focus();
        }

        private void txtTranslation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            txtWord.Focus();
            txtWord.Text = e.KeyChar.ToString();
            txtWord.SelectionStart = txtWord.Text.Length;
        }

        private void txtTranslation_ClientSizeChanged(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
            panel2.Visible = !panel2.Visible;
        }

        private void txtTranslation_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _setFired = true;
            if (txtTranslation.SelectedText.Trim() != "")
            {
                txtWord.Text = txtTranslation.SelectedText.Trim();
                btnSearch_Click(null, new EventArgs());
            }
        }

        private void txtTranslation_MouseUp(object sender, MouseEventArgs e)
        {
            if (txtTranslation.SelectedText.Length > 0 && !_setFired)
            {
                ctxSearch.Visible = true;
                ctxSearch.Location = MousePosition;
            }
            _setFired = false;
        }

        private void ctxItemSearch_Click(object sender, EventArgs e)
        {
            txtWord.Text = txtTranslation.SelectedText.Trim();
            btnSearch_Click(null, new EventArgs());
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtTranslation.SelectedText.Trim());
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            MediaPlayer.MediaPlayer player = new MediaPlayer.MediaPlayer { FileName = (string)btnSound.Tag };
            player.Play();
            txtTranslation.Focus();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            txtWord.Focus();
        }

        private void ctxItemColor_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _clm.Checked = false;
            _clm = (ToolStripMenuItem)e.ClickedItem;
            _clm.Checked = true;
            //txtTranslation.BackColor = Color.FromName(_clm.Text);
            BackgroundImage = _clm.BackgroundImage;
            //BackColor = Color.FromName(_clm.Text);
        }

        private void ctxItemColor_MouseHover(object sender, EventArgs e)
        {
            //txtTranslation.BackColor = Color.FromName(((ToolStripMenuItem)sender).Text);
            BackgroundImage = ((ToolStripMenuItem)sender).BackgroundImage;
        }

        private void colorToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            BackgroundImage = _clm.BackgroundImage;
        }

        private void ctxItemTransparent0_MouseHover(object sender, EventArgs e)
        {
            Opacity = Convert.ToDouble(((ToolStripMenuItem)sender).Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });
        }

        private void ctxItemTransparent_DropDownClosed(object sender, EventArgs e)
        {
            Opacity = Convert.ToDouble(_tsm.Tag, new NumberFormatInfo { NumberDecimalSeparator = "." });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Transparency = _tsm.Name;
            Settings.Default.Color = _clm.Name;
            Settings.Default.LocationX = Location.X + Width - _width[1];
            Settings.Default.LocationY = Location.Y;
            Settings.Default.Welcome = "";
            Settings.Default.RunAtStartup = ctxItemStartup.Checked;
            Settings.Default.AlwaysOnTop = ctxItemAlwaysTop.Checked;
            Settings.Default.Save();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point(Settings.Default.LocationX, Settings.Default.LocationY);
        }

        private void ctxItemStartup_Click(object sender, EventArgs e)
        {
            ctxItemStartup.Checked = WinFormHelper.CreateShortcut(_startupPathName, !ctxItemStartup.Checked);

        }

        private void txtWord_DragDrop(object sender, DragEventArgs e)
        {
            txtWord.Text = e.Data.GetData(DataFormats.Text).ToString();
            btnSearch_Click(null, new EventArgs());
        }

        private void txtWord_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.Text) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void btnTr_Click(object sender, EventArgs e)
        {
            _dcb.FlatAppearance.BorderSize = 0;
            _dcb = ((Button)sender);
            _dcb.FlatAppearance.BorderSize = 1;
            _isTr = Convert.ToBoolean(_dcb.Tag);
        }
    }
}
