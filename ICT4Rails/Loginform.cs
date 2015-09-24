using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICT4Rails
{
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
            Remisesysteem remise = new Remisesysteem();
            remise.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
