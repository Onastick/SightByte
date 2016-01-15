using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SightByte
{
    partial class Hotkey_Configuration : Form
    {
        Form1 baseForm;
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Hotkey_Configuration(Form1 baseForm)
        {
            this.baseForm = baseForm;
            InitializeComponent();
            if(baseForm.preset1Mod != Form1.KeyModifier.None)
                textBox1.Text = baseForm.preset1Mod.ToString() + " " + baseForm.keyPreset1.ToString();
            else
                textBox1.Text = baseForm.keyPreset1.ToString();

            if (baseForm.preset2Mod != Form1.KeyModifier.None)
                textBox2.Text = baseForm.preset2Mod.ToString() + " " + baseForm.keyPreset2.ToString();
            else
                textBox2.Text = baseForm.keyPreset2.ToString();

            if (baseForm.preset3Mod != Form1.KeyModifier.None)
                textBox3.Text = baseForm.preset3Mod.ToString() + " " + baseForm.keyPreset3.ToString();
            else
                textBox3.Text = baseForm.keyPreset3.ToString();

            if (baseForm.preset0Mod != Form1.KeyModifier.None)
                textBox4.Text = baseForm.preset0Mod.ToString() + " " + baseForm.keyPreset0.ToString();
            else
                textBox4.Text = baseForm.keyPreset0.ToString();

        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        Form1.KeyModifier modifierListener(object sender, KeyEventArgs e)
        {
            Form1.KeyModifier modifier;
            switch (Control.ModifierKeys)
            {
                case (Keys.Shift):
                    {
                        modifier = Form1.KeyModifier.Shift;
                        break;
                    }
                case (Keys.Alt):
                    {
                        modifier = Form1.KeyModifier.Alt;
                        break;
                    }
                case (Keys.Control):
                    {
                        modifier = Form1.KeyModifier.Control;
                        break;
                    }
                case (Keys.LWin):
                    {
                        modifier = Form1.KeyModifier.WinKey;
                        break;
                    }
                case (Keys.RWin):
                    {
                        modifier = Form1.KeyModifier.WinKey;
                        break;
                    }
                default:
                    {
                        modifier = Form1.KeyModifier.None;
                        break;
                    }
            }

            return modifier;
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            baseForm.editing = false;
        }

        private void Hotkey_Configuration_Load(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            baseForm.editing = true;
            textBox1.Text = "";
            label1.Text = "Enter Hotkey";
        }

        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            baseForm.editing = true;
            textBox2.Text = "";
            label2.Text = "Enter Hotkey";
        }

        private void textBox3_MouseUp(object sender, MouseEventArgs e)
        {
            baseForm.editing = true;
            textBox3.Text = "";
            label3.Text = "Enter Hotkey";
        }

        private void textBox4_MouseUp(object sender, MouseEventArgs e)
        {
            baseForm.editing = true;
            textBox4.Text = "";
            label5.Text = "Enter Hotkey";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Form1.KeyModifier modifier = modifierListener(sender, e);
            label1.Text = "Preset 1";
            if ((int)modifier != 0)
                textBox1.Text = modifier.ToString() + " + " + e.KeyCode.ToString();
            else
                textBox1.Text = e.KeyCode.ToString();

            if (textBox1.Text.Count() == 0)
                textBox1.Text = "Invalid Key";

            baseForm.changeHotKey(1, e, modifier);
            label1.Focus();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            Form1.KeyModifier modifier = modifierListener(sender, e);
            label2.Text = "Preset 2";
            if ((int)modifier != 0)
                textBox2.Text = modifier.ToString() + " + " + e.KeyCode.ToString();
            else
                textBox2.Text = e.KeyCode.ToString();

            if (textBox2.Text.Count() == 0)
                textBox2.Text = "Invalid Key";

            baseForm.changeHotKey(2, e, modifier);
            label2.Focus();
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            Form1.KeyModifier modifier = modifierListener(sender, e);
            label3.Text = "Preset 3";
            if ((int)modifier != 0)
                textBox3.Text = modifier.ToString() + " + " + e.KeyCode.ToString();
            else
                textBox3.Text = e.KeyCode.ToString();

            if (textBox3.Text.Count() == 0)
                textBox3.Text = "Invalid Key";

            baseForm.changeHotKey(3, e, modifier);
            label3.Focus();
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            Form1.KeyModifier modifier = modifierListener(sender, e);
            label5.Text = "Revert";
            if ((int)modifier != 0)
                textBox4.Text = modifier.ToString() + " + " + e.KeyCode.ToString();
            else
                textBox4.Text = e.KeyCode.ToString();

            if (textBox4.Text.Count() == 0)
                textBox4.Text = "Invalid Key";

            baseForm.changeHotKey(0, e, modifier);
            label5.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void Hotkey_Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            baseForm.editing = false;
        }
    }
}
