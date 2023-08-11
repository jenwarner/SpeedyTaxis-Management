using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeedyTaxis
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
        }
        int sec = 0;
        private void splashForm_Load(object sender, EventArgs e)
        {
            timer1.Start();

            

            }

            private void timer1_Tick(object sender, EventArgs e)
        {
           // loadingLbl.Text = "";
            sec += 1;
            for (int dots = 0; dots <= 2; ++dots)
            {
                loadingLbl.Text += (string.Format(". "));
                System.Threading.Thread.Sleep(500); // half a sec
                //loadingLbl.Text = "";
            }

            if (sec == 3)
                {
                    timer1.Stop();
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.ShowDialog();
                }
            
            
        }
    }
}
