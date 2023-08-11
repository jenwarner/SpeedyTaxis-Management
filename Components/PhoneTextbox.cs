using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Components
{
    public partial class PhoneTextbox: RichTextBox
    {
        public PhoneTextbox()
        {
            InitializeComponent();
        }
        // declare variables
        const int numberLength = 10;
        private string _numberInput;

        public string NumberInput
        {
            get { return _numberInput; }

            set { _numberInput = value;
            GetSetNumber();
            }
        }

        private void GetSetNumber()
        {
            if ((this.Text == NumberInput) || (this.Text == string.Empty))
            {
                this.Text = NumberInput;
            }
        } 

        private void PhoneTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // accept only numbers into textbox
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // if more than 11 numbers are entered into the textbox, those numbers will be shown in red.

            if (this.Text.Length > numberLength) { 
                this.SelectionColor = Color.Red; 
            } else {
                this.SelectionColor = Color.Black;
            }
       }
        

        
    }
}
