using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graf
{
    public enum CHOICE { NUUL, FloydWarshell, BellmanFord, Dijkstra, Analith };
    public partial class Form1 : Form
    {
        Brush[] color = new Brush[7] { Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green,
        Brushes.LightBlue, Brushes.Aqua, Brushes.Violet};
        static int i = 0;
        int MATRIX_SIZE = 0;
        int[,] MATRIX;
        static List<int> road = new List<int>(2);
        public List<Peak> array_of_peak = new List<Peak>();
        public List<Arc> array_of_arc = new List<Arc>();
        bool Draw = false;
        bool Clear = false;
        bool Line = false;
        bool Weight = false;
        int ix;
        int size = 0;
        public bool Table = false;
        bool Open = false;
        public void weight(int a) {
            this.ix = a;
        }       // получает вес дуги
        public void draw() {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                System.Drawing.Font font = new System.Drawing.Font("Verdana", 15);
                Pen blackPen = new Pen(Color.Black, 2);
                blackPen.CustomEndCap = new AdjustableArrowCap(4.0F, 8.0F);
                for (int j = 0; j < array_of_peak.Count; j++)
                {
                    g.FillEllipse(color[array_of_peak[j].color], array_of_peak[j].location[0], array_of_peak[j].location[1], 26, 26);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.DrawString(array_of_peak[j].number_peak.ToString(), font, Brushes.Black, array_of_peak[j].location[0], array_of_peak[j].location[1]);
                }
                for (int j = 0; j < array_of_arc.Count; j++)
                {
                    string Number = (j + 1).ToString();
                    if (array_of_arc[j].peak1 == array_of_arc[j].peak2)
                    {
                        double p;
                        double newX;
                        double newY;
                        double a = 50;
                        for (int i = 0; i <= 60; i++)
                        {
                            p = a * Math.Sin(3 * i * Math.PI / 180);
                            newX = p * Math.Cos(i * Math.PI / 180) + array_of_arc[j].peak1.location[0];
                            newY = p * Math.Sin(i * Math.PI / 180) + array_of_arc[j].peak1.location[1];
                            g.FillEllipse(Brushes.Black, (int)newX + 13, (int)newY + 13, 3, 3);
                            if (i == 15)
                            {
                                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                g.DrawString(array_of_arc[j].weight.ToString(), font, color[3], (int)newX, (int)newY);
                            }
                            if (i == 30) {
                                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                g.DrawString(Number, font, color[0], (int)newX, (int)newY);
                            }
                        }
                    }
                    else
                    {
                        if (array_of_arc[j].arc) {
                            double[] v1 = new double[2]; double[] v2 = new double[2]; double[] v3 = new double[2]; double[] v4 = new double[2];
                            double[] p1 = new double[2]; double[] p2 = new double[2];
                            double k;
                            bool change = false;
                            double newp1X = array_of_arc[j].peak1.location[0]; double newp1Y = -array_of_arc[j].peak1.location[1];
                            double newp2X = array_of_arc[j].peak2.location[0]; double newp2Y = -array_of_arc[j].peak2.location[1];
                            try { k = Math.Atan((newp2Y - newp1Y) / (newp2X - newp1X)); }
                            catch { k = 90 * Math.PI / 180; }
                            double COS = Math.Cos(k);
                            double SIN = Math.Sin(k);
                            p1[0] = newp1X * COS + newp1Y * SIN;
                            p1[1] = -newp1X * SIN + newp1Y * COS;
                            p2[0] = newp2X * COS + newp2Y * SIN;
                            p2[1] = -newp2X * SIN + newp2Y * COS;
                            double x = (p2[0] + p1[0]) / 2;
                            double y = (p2[1] + p1[1]) / 2 + 20;
                            double param_a = (y - p1[1]) / (2 * x * p1[0] - Math.Pow(x, 2) - Math.Pow(p1[0], 2));
                            double param_b = -2 * param_a * x;
                            double param_c = y + param_a * Math.Pow(x, 2);
                            if (p1[0]>p2[0]) {
                                double elm = p1[0];
                                p1[0] = p2[0];
                                p2[0] = elm;
                                change = true;
                            }
                            for (double i = p1[0]; i < p2[0]; i++)
                            {
                                double Y = param_a * Math.Pow(i, 2) + param_b * i + param_c;
                                double renewX = i * COS - Y * SIN;
                                double renewY = (-1) * (i * SIN + Y * COS);
                                g.FillEllipse(Brushes.Black, (int)renewX + 13, (int)renewY + 13, 3, 3);
                                if ((int)i + 50 == (int)x) {
                                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                    g.DrawString(array_of_arc[j].weight.ToString(), font, color[3], (int)renewX, (int)renewY);
                                }
                                if ((int)i == (int)x)
                                {
                                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                    g.DrawString(Number, font, color[0], (int)renewX, (int)renewY);
                                }
                            }
                            if (!change)
                            {
                                for (double i = p2[0] - 15; i < p2[0]; i++)
                                {
                                    double Y = param_a * Math.Pow(i, 2) + param_b * i + param_c;
                                    double renewX = i * COS - Y * SIN;
                                    double renewY = (-1) * (i * SIN + Y * COS);
                                    g.FillEllipse(Brushes.DarkRed, (int)renewX + 13, (int)renewY + 13, 7, 7);
                                    g.FillEllipse(Brushes.LightPink, (int)renewX + 13, (int)renewY + 13, 4, 4);
                                }
                            }
                            else {
                                for (double i = p1[0] + 5; i < p1[0] + 20; i++)
                                {
                                    double Y = param_a * Math.Pow(i, 2) + param_b * i + param_c;
                                    double renewX = i * COS - Y * SIN;
                                    double renewY = (-1) * (i * SIN + Y * COS);
                                    g.FillEllipse(Brushes.DarkRed, (int)renewX + 13, (int)renewY + 13, 7, 7);
                                    g.FillEllipse(Brushes.LightPink, (int)renewX + 13, (int)renewY + 13, 4, 4);
                                }
                            }
                        } 
                        else {
                            int x = (array_of_arc[j].peak2.location[0] + array_of_arc[j].peak1.location[0]) / 2;
                            int y = (array_of_arc[j].peak2.location[1] + array_of_arc[j].peak1.location[1]) / 2;
                            g.DrawLine(blackPen, array_of_arc[j].peak1.location[0] + 13, array_of_arc[j].peak1.location[1] + 13,
                                array_of_arc[j].peak2.location[0] + 13, array_of_arc[j].peak2.location[1] + 13);
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            g.DrawString(Number, font, color[0], x, y);
                            g.DrawString(array_of_arc[j].weight.ToString(), font, color[3],
                                (array_of_arc[j].peak2.location[0] + x) / 2, (array_of_arc[j].peak2.location[1] + y) / 2);
                        }
                    }
                }
            }
        }           // рисует
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt";
            openFileDialog1.CheckPathExists = true;
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.OverwritePrompt = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            toolTip1.SetToolTip(textBox1, "Введите последовательность вершин графа в формате: точка1 точка2 вес дуги");
            toolTip1.IsBalloon = true;
        }
        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == true) return;
            if (e.KeyChar == (char)Keys.Back) return;
            e.Handled = true;
        }           // проверка ввода в textbox
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)     // клик по picturebox
        {
            if (Draw == true)
            {
                Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Peak point = new Peak();
                int X = e.X - 13; int Y = e.Y - 13;
                if (point.match(array_of_peak, X, Y) >= 0)
                {
                    return;
                }
                point.peak(array_of_peak.Count, X, Y, i);
                array_of_peak.Add(point);
                pictureBox1.Image = btm;
                draw();
                pictureBox1.Invalidate();
                if (i == 6) { i = -1; }
                i++;
            }       // задаем точки
            if (Clear == true)
            {
                Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Peak point = new Peak();
                Arc line = new Arc();
                int X = e.X - 13; int Y = e.Y - 13;
                int index = point.match(array_of_peak, X, Y);   // мы кликнули по точке?
                int index_road_del = line.match(array_of_arc, X, Y);    // мы кликнули по прямой?
                int loop = line.search_for_loop(array_of_arc, X, Y);    // мы кликнули по петле?
                int arc = line.search_for_arc(array_of_arc, X, Y);
                if (index >= 0)
                {
                    for (int j = 0; j < array_of_arc.Count; j++)
                    {
                        if (array_of_arc[j].peak1.number_peak == array_of_peak[index].number_peak
                            || array_of_arc[j].peak2.number_peak == array_of_peak[index].number_peak)
                        {
                            array_of_arc.RemoveAt(j);
                            j--;
                        }
                    }
                    array_of_peak.RemoveAt(index);
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                if (arc >= 0) {
                    array_of_arc.RemoveAt(arc);
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                if (loop >= 0) {
                    array_of_arc.RemoveAt(loop);
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                if (index_road_del >= 0) {
                    array_of_arc.RemoveAt(index_road_del);
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
            }       // удаляем
            if (Line == true)
            {
                Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Peak point = new Peak();
                Arc line = new Arc();
                int X = e.X - 13; int Y = e.Y - 13;
                int index = point.match(array_of_peak, X, Y);
                if (index >= 0)
                {
                    road.Add(index);
                }
                if (road.Count == 2) {
                    bool arc = false;
                    if (!line.search_for_matches(array_of_arc, array_of_peak, road, ref arc)) {
                        line.Box(array_of_peak, road, 0, arc);
                        array_of_arc.Add(line);
                        pictureBox1.Image = btm;
                        draw();
                        pictureBox1.Invalidate();

                    }
                    road.Clear();
                }
            }       // задаеем дуги
            if (Weight == true) {
                Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Peak point = new Peak();
                Arc line = new Arc();
                int X = e.X - 13; int Y = e.Y - 13;
                int index_road_del = line.match(array_of_arc, X, Y);
                int loop = line.search_for_loop(array_of_arc, X, Y);
                int arc = line.search_for_arc(array_of_arc, X, Y);
                if (arc >= 0)
                {
                    CreatWeight newForm = new CreatWeight();
                    newForm.Owner = this;
                    newForm.ShowDialog();
                    array_of_arc[arc].weight = ix;
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                if (loop >= 0)      // клик по петле
                {
                    CreatWeight newForm = new CreatWeight();
                    newForm.Owner = this;
                    newForm.ShowDialog();
                    array_of_arc[loop].weight = ix;
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                if (index_road_del >= 0)    // клик по дуге
                {
                    CreatWeight newForm = new CreatWeight();
                    newForm.Owner = this;
                    newForm.ShowDialog();
                    array_of_arc[index_road_del].weight = ix;
                    pictureBox1.Image = btm;
                    draw();
                    pictureBox1.Invalidate();
                    return;
                }
                ix = 0;
            }       // задаем вес
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!Table && !Open)
            {
                Draw = true;
                Clear = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
            else {
                Draw = false;
                Clear = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
        }       // разрешает рисовать вершины
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!Table && !Open)
            {
                Clear = true;
                Draw = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
            else
            {
                Draw = false;
                Clear = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
        }       // разрешает удалять
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!Table && !Open)
            {
                Line = true;
                Clear = false;
                Draw = false;
                Weight = false;
                road.Clear();
            }
            else
            {
                Draw = false;
                Clear = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
        }       // разрешает рисовать дуги
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (!Table && !Open)
            {
                Weight = true;
                Draw = false;
                Clear = false;
                Line = false;
                road.Clear();
            }
            else
            {
                Draw = false;
                Clear = false;
                Line = false;
                Weight = false;
                road.Clear();
            }
        }       // разрешает задавать вес
        public TextBox[,] tb;
        private void button5_Click(object sender, EventArgs e)
        {
            try {
                size = Convert.ToInt32(textBox1.Text);
                if (size > 1 && size < 26)
                {
                    Form2 newf = new Form2(size + 1);
                    newf.Owner = this;
                    tb = newf.creat_table();
                    newf.ShowDialog();
                    if (Table)
                    {
                        Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        Graphics.FromImage(btm).Clear(Color.White);
                        array_of_arc.Clear();
                        array_of_peak.Clear();
                        pictureBox1.Image = btm;
                        draw();
                        pictureBox1.Invalidate();
                        for (int i = 0; i < size; i++)
                        {
                            Peak point = new Peak();
                            point.peak(array_of_peak.Count, 0, 0, 0);
                            array_of_peak.Add(point);
                        }
                        for (int i = 1; i <= size; i++)
                        {
                            for (int j = 1; j <= size; j++)
                            {
                                Arc line = new Arc();
                                try
                                {
                                    Single a;
                                    List<int> index = new List<int>(2);
                                    bool weight = Single.TryParse(tb[i, j].Text, System.Globalization.NumberStyles.Number, System.Globalization.NumberFormatInfo.CurrentInfo, out a);
                                    if (a <= int.MaxValue)
                                    {
                                        index.Add(i - 1); index.Add(j - 1);
                                        line.Box(array_of_peak, index, (int)a, false);
                                        array_of_arc.Add(line);
                                    }
                                    else {
                                        MessageBox.Show("Ваша дуга не может быть такой тяжелой!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        array_of_arc.Clear();
                                        array_of_peak.Clear();
                                        Table = false;
                                        return;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите целое положительное число больше 1 и меньше 26!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch {
                MessageBox.Show("Введите целое положительное число больше 1 и меньше 26!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }           // задать матрицу достижимости и взять из нее данные
        private void button2_Click(object sender, EventArgs e)
        {
            array_of_arc.Clear();
            array_of_peak.Clear();
            road.Clear();
            size = 0;
            Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(btm).Clear(Color.White);
            pictureBox1.Image = btm;
            draw();
            pictureBox1.Invalidate();
            textBox2.Text = "";
            textBox1.Text = "";
            Open = false;
            Table = false;
            Draw = false;
            Line = false;
            Clear = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            Choice = CHOICE.NUUL;
        }           // чистить все
        CHOICE Choice = CHOICE.NUUL;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Choice = CHOICE.FloydWarshell;
        }       // выбор алгоритма
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Choice = CHOICE.BellmanFord;
        }       // выбор алгоритма
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Choice = CHOICE.Dijkstra;
        }       // выбор алгоритма
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Choice = CHOICE.Analith;
        }       // выбор алгоритма
        private void button1_Click(object sender, EventArgs e)
        {
            if (array_of_peak.Count > 1 && Choice != CHOICE.NUUL)
            {
                List<int> parameters = new List<int>();
                Arc Line = new Arc();
                int[] distance = new int[array_of_peak.Count];
                List<Peak> array_of_peak1 = new List<Peak>(array_of_peak);
                List<Arc> array_of_arc1 = new List<Arc>(array_of_arc);
                distance = Line.Work(array_of_arc1, array_of_peak1, Choice, ref parameters);
                if (Choice != CHOICE.Analith)
                {
                    textBox2.Text = "Результат работы алгоритма: " + Choice.ToString() + "\r\n";
                    for (int i = 1; i < array_of_peak.Count; i++)
                    {
                        textBox2.Text += "точка 0 - точка " + array_of_peak[i].number_peak + ": " + (distance[i] == Int32.MaxValue || distance[i] <= 0 ? "Нет пути" : distance[i].ToString()) + "\r\n";
                    }
                    textBox2.Text += "Время работы алгоритма в милесекундах: " + parameters[0] + "\r\n";
                    textBox2.Text += "Количество сравнений: " + parameters[1] + "\r\n";
                    textBox2.Text += "Количество пререстановок: " + parameters[2] + "\r\n";

                }
                else {
                    textBox2.Text = "Результат работы алгоритмов:\r\n";
                    for (int i = 1; i < array_of_peak.Count; i++)
                    {
                        textBox2.Text += "точка 0 - точка " + array_of_peak[i].number_peak + ": " + (distance[i] == Int32.MaxValue || distance[i] <= 0 ? "Нет пути" : distance[i].ToString()) + "\r\n";
                    }
                    CHOICE key = CHOICE.NUUL;
                    for (int i = 0; i < 9; i += 3) {
                        key++;
                        textBox2.Text += "Алгоритм  " + key.ToString() + "\r\n";
                        textBox2.Text += "Время работы алгоритма в миллисекундах: " + parameters[i] + "\r\n";
                        textBox2.Text += "Количество сравнений: " + parameters[i + 1] + "\r\n";
                        textBox2.Text += "Количество пререстановок: " + parameters[i + 2] + "\r\n";
                    }
                }
            }
            else
            {
                if (Choice == CHOICE.NUUL)
                {
                    MessageBox.Show("Задайте алгоритм!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Задайте хотя бы две вершины!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Choice = CHOICE.NUUL;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }           // вывод информации
        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            Arc line = new Arc();
            string GRAPH = "";
            List<Arc> newArc = line.BigGraph(array_of_arc, array_of_peak);
            int[,] newGraph = line.copy(newArc, array_of_peak.Count* array_of_peak.Count);
            line.sort(ref newGraph, array_of_peak.Count * array_of_peak.Count);
            int size = array_of_peak.Count * array_of_peak.Count;
            for (int i = 0; i < size; i++) {
                GRAPH += newGraph[i, 0].ToString() + " " + newGraph[i, 1].ToString() + " " + newGraph[i, 2].ToString() + " ";
            }
            string filename = saveFileDialog1.FileName;
            string filter = filename.Substring(filename.Length - 4);
            if (filter != ".txt")
            {
                filename += ".txt";
            }
            System.IO.File.WriteAllText(filename, GRAPH);
        }       // сохранить граф в файл
        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            string filter = filename.Substring(filename.Length - 4);
            if (filter != ".txt") {
                filename += ".txt";
            }
            System.IO.File.WriteAllText(filename, textBox2.Text);
        }       // сохранить информацию в файл
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В файле должна храниться матрица достижимости в формате: точка - точка - вес! В одну строчку!", 
                "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) {
                    return;
                }   
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                List<int> graph = new List<int>();
                array_of_arc.Clear(); array_of_peak.Clear();
                for (int i = 0; i < fileText.Length; i++)
                {
                    string elm = fileText[i].ToString();
                    if (elm != " ")
                    {
                        graph.Add(Convert.ToInt32(elm));
                    }
                }

                int size = (int)Math.Sqrt(graph.Count / 3);
                for (int i = 0; i < size; i++)
                {
                    Peak point = new Peak();
                    point.peak(i, 0, 0, 0);
                    array_of_peak.Add(point);
                }
                for (int i = 0; i < graph.Count; i += 3)
                {
                    Arc line = new Arc();
                    line.Box(array_of_peak, graph[i], graph[i + 1], graph[i + 2]);
                    array_of_arc.Add(line);
                }
                if (array_of_peak.Count == 0) {
                    MessageBox.Show("Пустой файл!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Open = true;
                textBox1.Text = "";
            }
            catch {
                Open = false;
                array_of_arc.Clear(); array_of_peak.Clear();
                MessageBox.Show("Не верный формат данных!", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       // открыть файл // взять данные из файла
        private void button6_Click(object sender, EventArgs e)
        {
            MATRIX_SIZE = array_of_peak.Count;
            if (MATRIX_SIZE!=0) {
                Arc point = new Arc();
                MATRIX = point.Matrix(array_of_arc, array_of_peak);
                Form3 newf = new Form3(MATRIX_SIZE + 1, MATRIX);
                newf.Owner = this;
                newf.creat_table();
                newf.ShowDialog();
            }
           
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}