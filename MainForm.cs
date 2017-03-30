using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace translate_test
{
    public partial class MainForm : Form
    { 
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_MouseEnter(object sender, EventArgs e)
        {
            label5.Visible = true;

        }

        private void checkBox1_MouseLeave(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(Format(textBox1.Text));
            foreach(object o in listBox1.Items)
            {
                f.AddLanguage((string)o);
            }

            string[] strings = textBox6.Text.Split(new char[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strings)
                f.Strings.Add(s);

            f.StringDelimiter = Format(textBox4.Text);
            f.GroupDelimiter = Format(textBox3.Text);
            f.Suffix = Format(textBox2.Text);
            f.OutputFilepath = textBox5.Text;
            f.GroupByLanguage = checkBox1.Checked;
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if(d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string t = (string.IsNullOrEmpty(textBox5.Text) ? "" : textBox5.Text.Substring(textBox5.Text.LastIndexOf('\\') + 1));
                textBox5.Text = d.SelectedPath + "\\" + t;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Text Files (.txt)|*.txt";
            d.Multiselect = false;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using(System.IO.StreamReader sr = new System.IO.StreamReader(d.FileName))
                {
                    textBox6.Text = sr.ReadToEnd();
                }
            }
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                if (!listBox1.Items.Contains(((CheckedListBox)sender).Items[e.Index]))
                    listBox1.Items.Add(((CheckedListBox)sender).Items[e.Index]);
            }
            else
            {
                listBox1.Items.Remove(((CheckedListBox)sender).Items[e.Index]);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private string Format(string s)
        {
            return s.Replace("\\r\\n", "\r\n").Replace("\\t", "\t").Replace("\\\\", "\\").Replace("\\\"", "\"");
        }
    }
}
