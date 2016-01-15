using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SightByte
{
    public partial class Knob : Form
    {
        Form1 baseForm;

        public Knob(Form1 baseForm)
        {
            this.baseForm = baseForm;
            InitializeComponent();
        }

        private void Knob_Load(object sender, EventArgs e)
        {
            Location = new Point(Control.MousePosition.X, Control.MousePosition.Y - (this.Size.Height + (Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height)));
            trackBar1.Value = baseForm.trackBar1.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            baseForm.trackBar1.Value = trackBar1.Value;
            baseForm.setGammaValue(trackBar1.Value);
            toolTip1.SetToolTip(trackBar1, trackBar1.Value.ToString());
        }

        private void Knob_Deactivate(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
