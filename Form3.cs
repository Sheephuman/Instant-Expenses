using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;


namespace WindowsFormsApplication1
{
    public partial class SettingForm : Form
    {

        #region タイトルバー非表示時に移動可にする
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();


        #endregion


     [SecurityPermission(SecurityAction.Demand,
    Flags = SecurityPermissionFlag.UnmanagedCode)]
        
        
        
        
        public SettingForm()
        {
            InitializeComponent();

       

            //タイトルバーを消す



            this.Text = "";
            // this.ControlBox = false;
            //this.BackColor = Color.DarkCyan;
        }

     #region フォームを閉じさせない処理
     protected override void WndProc(ref Message m)
     {
         const int WM_SYSCOMMAND = 0x112;
         const long SC_CLOSE = 0xF060L;

         if (m.Msg == WM_SYSCOMMAND &&
             (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
         {

             this.WindowState = FormWindowState.Minimized;
             return;
         }

         base.WndProc(ref m);
     }
     #endregion 

     public SettingForm STPropaty
        {
            set
            {
                this.STPropaty.Topradio.Checked = true;

            }
            get
            {

                return this.STPropaty;

            }




        }

     
        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            this.Opacity = (double)trackBar1.Value / 100;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

           

            this.Opacity = 1;
            trackBar1.Minimum = 20;


            trackBar1.Maximum = 100;


            trackBar1.Value = 100;


            trackBar1.TickFrequency = 5;


            trackBar1.SmallChange = 1;
            trackBar1.LargeChange = 30;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {

                this.ControlBox = false;

            }

            else { this.ControlBox = true; }

        }

    

        private void SettingForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //マウスのキャプチャを解除
                ReleaseCapture();
                //タイトルバーでマウスの左ボタンが押されたことにする
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }



        }
    }
}
