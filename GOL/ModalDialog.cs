using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{
    public partial class ModalDialog : Form
    {
        public ModalDialog()
        {
            InitializeComponent();
        }

        private void ModalDialog_Load(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public int setX
        {
            get 
            {
                return (int)numericUpDown1.Value;
            }
            set 
            {
                numericUpDown1.Value = value;
            }

        }
        public int setY {
            get {
                return (int)numericUpDownNumber.Value;
            }
            set {
                numericUpDownNumber.Value = value;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownNumber_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
