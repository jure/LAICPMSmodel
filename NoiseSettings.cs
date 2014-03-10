using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class NoiseSettings : Form
    {
        private double[] rsds;

        public NoiseSettings(double[] rsdsCalc)
        {
            InitializeComponent();
            rsds = rsdsCalc;         
        }
        private void NoiseSettings_Load(object sender, System.EventArgs e)
        {
            if (rsds != null)
            {
                double rsdsSum = 0;
                for (int i = 0; i < rsds.Count(); i++)
                {
                    textBox13.Text += String.Format("{0:0.##}", rsds[i]) + Environment.NewLine;
                    rsdsSum += rsds[i];
                }

                textBox13.Text += Environment.NewLine + "The average: " + Environment.NewLine + String.Format("{0:0.##}", rsdsSum/rsds.Count()) + ".";
            }
        }
        private void NoiseSettings_Close(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
         
            this.Close();

        }

    }
}
