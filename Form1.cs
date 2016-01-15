using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace SightByte
{
    public partial class Form1 : Form
    {
        Knob knobby;
        Hotkey_Configuration keyConfig;
        int _gammaLvl;
        public static int _MAXGAMMA = 256;
        public bool editing;

        public Keys keyPreset0;
        public Form1.KeyModifier preset0Mod;
        public Keys keyPreset1;
        public Form1.KeyModifier preset1Mod;
        public static int preset1Value;
        public Keys keyPreset2;
        public Form1.KeyModifier preset2Mod;
        public static int preset2Value;
        public Keys keyPreset3;
        public Form1.KeyModifier preset3Mod;
        public static int preset3Value;

        public int gammaLvl { get { return _gammaLvl; } set { _gammaLvl = value; } }

        public static RAMP ramp = new RAMP();
        public static RAMP r = new RAMP();

        public static RAMP preset1 = new RAMP();
        public static RAMP preset2 = new RAMP();
        public static RAMP preset3 = new RAMP();

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            trackBar1.Value = _MAXGAMMA / 2;

            StoreGamma();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();
                if(!editing)
                {
                    switch (id)
                    {
                        case (0):
                            {
                                trackBar1.Value = _MAXGAMMA / 2;
                                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref r);
                                break;
                            }
                        case (1):
                            {
                                trackBar1.Value = preset1Value;
                                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset1);
                                break;
                            }
                        case (2):
                            {
                                trackBar1.Value = preset2Value;
                                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset2);
                                break;
                            }
                        case (3):
                            {
                                trackBar1.Value = preset3Value;
                                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset3);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        public static void SetGamma(int gamma)
        {
            if (gamma <= 256 && gamma >= 1)
            {
                RAMP ramp = new RAMP();
                ramp.Red = new ushort[256];
                ramp.Green = new ushort[256];
                ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    int iArrayValue = i * (gamma + 128);

                    if (iArrayValue > 65535)
                        iArrayValue = 65535;
                    ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;
                }
                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            }
        }

        public static int GetGamma()
        {
            return GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
        }

        public static void StoreGamma()
        {
            GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref r);

            GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset1);
            preset1Value = _MAXGAMMA / 2;
            GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset2);
            preset2Value = _MAXGAMMA / 2;
            GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset3);
            preset3Value = _MAXGAMMA / 2;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {  
                Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(knobby == null || !knobby.IsHandleCreated)
                knobby = new Knob(this);
            knobby.Show();
            knobby.TopMost = true;
        }

        // Revert
        private void button3_Click(object sender, EventArgs e)
        {
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref r);
            trackBar1.Value = _MAXGAMMA / 2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            gammaLvl = trackBar1.Value;
            SetGamma(trackBar1.Value);
            toolTip1.SetToolTip(trackBar1, trackBar1.Value.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref r);
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            UnregisterHotKey(this.Handle, 3);
        }

        public void setGammaValue(int value)
        {
            SetGamma(value);
        }

        //Preset 1
        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = preset1Value;
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset1);
        }

        //Preset 2
        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = preset2Value;
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset2);
        }

        //Preset 3
        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = preset3Value;
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset3);
        }

        //Save preset
        private void button5_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                saveLabel.ForeColor = Color.Black;
                preset1Value = trackBar1.Value;
                saveLabel.Text = "Preset 1 Saved!";
                GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset1);
                timer1.Start();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                saveLabel.ForeColor = Color.Black;
                preset2Value = trackBar1.Value;
                saveLabel.Text = "Preset 2 Saved!";
                GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset2);
                timer1.Start();
            }
            if (comboBox1.SelectedIndex == 2)
            {
                saveLabel.ForeColor = Color.Black;
                preset3Value = trackBar1.Value;
                saveLabel.Text = "Preset 3 Saved!";
                GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref preset3);
                timer1.Start();
            }
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=NV98CPXKF2MP2&lc=US&item_name=Gray%20Glove%20Studio&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
        }

        private void setHotkeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (keyConfig == null || !keyConfig.IsHandleCreated)
                keyConfig = new Hotkey_Configuration(this);
            keyConfig.ShowDialog();
            keyConfig.TopMost = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutApp aApp = new AboutApp();
            aApp.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int fadingSpeed = 10;
            saveLabel.ForeColor = Color.FromArgb(saveLabel.ForeColor.R + fadingSpeed, saveLabel.ForeColor.G + fadingSpeed, saveLabel.ForeColor.B + fadingSpeed);

            if (saveLabel.ForeColor.R >= this.BackColor.R)
            {
                timer1.Stop();
                saveLabel.ForeColor = this.BackColor;
            }
        }

        public void changeHotKey(int value, KeyEventArgs e, KeyModifier mod)
        {
            UnregisterHotKey(this.Handle, value);
            if (value == 0)
                keyPreset0 = e.KeyCode;
            if (value == 1)
                keyPreset1 = e.KeyCode;
            if (value == 2)
                keyPreset2 = e.KeyCode;
            if (value == 3)
                keyPreset3 = e.KeyCode;
            RegisterHotKey(this.Handle, value, (int)mod, e.KeyCode.GetHashCode());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int id = 0;
            keyPreset0 = Keys.NumPad0;
            preset0Mod = (int)KeyModifier.None;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.None, Keys.NumPad0.GetHashCode());

            id = 1;
            keyPreset1 = Keys.NumPad1;
            preset1Mod = (int)KeyModifier.None;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.None, Keys.NumPad1.GetHashCode());

            id = 2;
            keyPreset2 = Keys.NumPad2;
            preset2Mod = (int)KeyModifier.None;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.None, Keys.NumPad2.GetHashCode());

            id = 3;
            keyPreset3 = Keys.NumPad3;
            preset3Mod = (int)KeyModifier.None;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.None, Keys.NumPad3.GetHashCode());
        }
    }
}
