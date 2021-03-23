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
    public partial class Form3 : Form
    {
        int size;
        int [,] tbm;
        public Form3(int a, int[,] tb)
        {
            InitializeComponent();
            this.size = a;
            this.tbm = tb;
        }
        public void creat_table()
        {
            int x = 0; int y = 10; int n = 0;
            TextBox[,] tb = new TextBox[size, size];
            Label[] lb = new Label[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!(i == 0 && j > 0) && !(i > 0 && j == 0))
                    {
                        if (!(i == 0 && j == 0))
                        {
                            tb[i, j] = new System.Windows.Forms.TextBox();
                            tb[i, j].Location = new System.Drawing.Point(10 + x * 27, y);
                            tb[i, j].Name = "textBox" + i.ToString();
                            tb[i, j].Size = new System.Drawing.Size(25, 25);
                            tb[i, j].Text = tbm[n, 2].ToString();
                            tb[i, j].ReadOnly = true;
                            tb[i, j].TabIndex = i;
                            Controls.Add(tb[i, j]);
                            n++;
                        }
                    }
                    else
                    {
                        if (i > 0 && j == 0)
                        {
                            lb[i] = new Label();
                            lb[i].Location = new Point(10 + x * 30, y);
                            lb[i].Name = "label2" + i.ToString();
                            lb[i].Text = (i - 1).ToString();
                            lb[i].MaximumSize = new Size(25, 25);
                            Controls.Add(lb[i]);
                        }
                        if (i == 0 && j > 0)
                        {
                            lb[j] = new Label();
                            lb[j].Location = new Point(10 + x * 27, y);
                            lb[j].Name = "label1" + j.ToString();
                            lb[j].Text = (j - 1).ToString();
                            lb[j].MaximumSize = new Size(25, 25);
                            Controls.Add(lb[j]);
                        }
                    }
                    x++;
                }
                x = 0; y += 23;
            }
        }
    }
}
