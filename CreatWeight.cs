using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graf
{
    public partial class CreatWeight : Form
    {
        public CreatWeight()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = (Form1)this.Owner;
            Single a;
            bool X = Single.TryParse(textBox1.Text, System.Globalization.NumberStyles.Number,
            System.Globalization.NumberFormatInfo.CurrentInfo, out a);
            if (X == false || a <= 0 || a - Math.Truncate(a) != 0)
            {
                MessageBox.Show("Введите целое положительное число!", "Fatal ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (a > int.MaxValue)
            {
                MessageBox.Show("Ваша дуга не может быть такой тяжелой!", "Fatal ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            f.weight((int)a);
            this.Close();
        }   // задаю вес дуги
    }
}
