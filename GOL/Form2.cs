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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {

        }

        public int setInterval {
            get {
                return (int)numericUpDown1.Value;
            }
            set {
                numericUpDown1.Value = value;
            }

        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
