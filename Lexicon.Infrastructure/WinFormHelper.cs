using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Lexicon.Infrastructure
{
    public static class WinFormHelper
    {
        public static string Appname;
        

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnectedToInternet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }


        #region formShape
        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        #endregion


        public static bool CreateShortcut(string shortcutPathName, bool create)
        {
            if (create)
            {
                try
                {
                    string shortcutTarget = Path.Combine(Application.StartupPath, Appname + ".exe");
                    WshShell myShell = new WshShell();
                    WshShortcut myShortcut = (WshShortcut)myShell.CreateShortcut(shortcutPathName);
                    myShortcut.TargetPath = shortcutTarget; //The exe file this shortcut executes when double clicked 
                    myShortcut.IconLocation = shortcutTarget + ",0"; //Sets the icon of the shortcut to the exe`s icon 
                    myShortcut.WorkingDirectory = Application.StartupPath; //The working directory for the exe 
                    myShortcut.Arguments = ""; //The arguments used when executing the exe 
                    myShortcut.Save(); //Creates the shortcut 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    if (File.Exists(shortcutPathName))
                        File.Delete(shortcutPathName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return File.Exists(shortcutPathName);

        }


    }
}
