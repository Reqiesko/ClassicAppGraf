using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Graf
{
    public class Peak
    {
        public int number_peak;    // номер вершины
        public int[] location = new int [2];   // положение вершины
        public int color;        // цвет вершины
        public void peak(int number, int X, int Y, int color) {
            this.number_peak = number;
            this.location[0] = X; this.location[1] = Y;
            this.color = color;
        }
        public int match(List<Peak> array, int X, int Y) 
        {
            int match = -1;
            for (int i = 0; i < array.Count; i++) 
            {
                if ((array[i].location[0] - 13) <= X && X <= (array[i].location[0] + 13)) 
                {
                    if ((array[i].location[1] - 13) <= Y && Y <= (array[i].location[1] + 13))
                    {
                        match = i;
                        return match;
                    }
                }
            }
            return match;
        }
    }
    public class Arc
    {
        
        public Peak peak1;      // от вершины 1
        public Peak peak2;      // до вершины 2
        public int weight;  // вес
        public bool arc;    // флажок на дугу
        public void Box(List<Peak> array, List<int>index , int weight, bool key) {
            this.peak1 = array[index[0]];
            this.peak2 = array[index[1]];
            this.weight = weight;
            this.arc = key;
        }       // собираем - собираем
        public void Box(List<Peak> array, int index1, int index2, int weight)       // собираем - собираем
        {
            this.peak1 = array[index1];
            this.peak2 = array[index2];
            this.weight = weight;
            this.arc = false;
        }           
        public int match(List<Arc> array, double X, double Y)
        {
            double[] v1 = new double[2]; double[] v2 = new double[2]; double[] v3 = new double[2]; double[] v4 = new double[2];
            double[] p1 = new double[2]; double[] p2 = new double[2];
            double var1, var2, var3, var4;
            double x = X; double y = -Y;
            double k;
            int match = -1;
            for (int i = 0; i < array.Count; i++)
            {
                if (!array[i].arc) {
                    double newp1X = array[i].peak1.location[0]; double newp1Y = -array[i].peak1.location[1];
                    double newp2X = array[i].peak2.location[0]; double newp2Y = -array[i].peak2.location[1];
                    try { k = Math.Atan((newp2Y - newp1Y) / (newp2X - newp1X)); }
                    catch { k = 90 * Math.PI / 180; }
                    double COS = Math.Cos(k);
                    double SIN = Math.Sin(k);
                    p1[0] = newp1X * COS + newp1Y * SIN;
                    p1[1] = -newp1X * SIN + newp1Y * COS;
                    p2[0] = newp2X * COS + newp2Y * SIN;
                    p2[1] = -newp2X * SIN + newp2Y * COS;

                    X = x * COS + y * SIN;
                    Y = -x * SIN + y * COS;

                    v1[0] = p1[0];
                    v1[1] = p1[1] + 3;
                    v2[0] = p1[0];
                    v2[1] = p1[1] - 3;
                    v3[0] = p2[0];
                    v3[1] = p2[1] - 3;
                    v4[0] = p2[0];
                    v4[1] = p2[1] + 3;

                    var1 = (v2[0] - v1[0]) * (Y - v1[1]) - (v2[1] - v1[1]) * (X - v1[0]);
                    var2 = (v3[0] - v2[0]) * (Y - v2[1]) - (v3[1] - v2[1]) * (X - v2[0]);
                    var3 = (v4[0] - v3[0]) * (Y - v3[1]) - (v4[1] - v3[1]) * (X - v3[0]);
                    var4 = (v1[0] - v4[0]) * (Y - v4[1]) - (v1[1] - v4[1]) * (X - v4[0]);
                    if (var1 >= 0 && var2 >= 0 && var3 >= 0 && var4 >= 0)
                    {
                        match = i;
                        return match;
                    }
                    if (var1 <= 0 && var2 <= 0 && var3 <= 0 && var4 <= 0)
                    {
                        match = i;
                        return match;
                    }
                }
            }
            return match;
        }       // клик по прямой дуге
        public bool search_for_matches(List<Arc> array, List<Peak>arrayP, List<int> index, ref bool key) {
            bool flag = false;
            for (int i = 0; i < array.Count; i++) {
                if (array[i].peak1 == arrayP[index[0]] && array[i].peak2 == arrayP[index[1]]
                     || array[i].peak2 == arrayP[index[0]] && array[i].peak1 == arrayP[index[1]])
                {
                    if (array[i].peak2 == arrayP[index[0]] && array[i].peak1 == arrayP[index[1]])
                    {
                        if (!array[i].arc) {
                            for (int j = 0; j < array.Count; j++) {
                                if (array[j].peak1 == arrayP[index[0]] && array[j].peak2 == arrayP[index[1]]) {
                                    flag = true;
                                    return flag;
                                }
                            }
                            key = true;
                        }
                        return flag;
                    }
                    else {
                        flag = true;
                        return flag;
                    }
                }
            }
            return flag;
        }       // задаем дуги
        public int search_for_loop(List<Arc> array, double X, double Y) {
            int  match = -1;
            for (int i = 0; i < array.Count; i++){
                if (array[i].peak1 == array[i].peak2)
                {
                    double p = 50 * Math.Sin(3 * 30 * Math.PI / 180);
                    double newX = p * Math.Cos(30 * Math.PI / 180) + array[i].peak1.location[0];
                    double newY = p * Math.Sin(30 * Math.PI / 180) + array[i].peak1.location[1];
                    if ((newX - 5) <= X && X <= (newX + 5))
                    {
                        if ((newY - 5) <= Y && Y <= (newY + 5))
                        {
                            match = i;
                            return match;
                        }
                    }
                }
            }
            return match;
        }       // кликнули по петле
        public int search_for_arc(List<Arc> array, double Xt, double Yt) {
            double[] v1 = new double[2]; double[] v2 = new double[2]; double[] v3 = new double[2]; double[] v4 = new double[2];
            double[] p1 = new double[2]; double[] p2 = new double[2];
            double k;
            Yt = -Yt;
            int match = -1;
            for (int j = 0; j < array.Count; j++) {
                if (array[j].arc){
                    double newp1X = array[j].peak1.location[0]; double newp1Y = -array[j].peak1.location[1];
                    double newp2X = array[j].peak2.location[0]; double newp2Y = -array[j].peak2.location[1];
                    try { k = Math.Atan((newp2Y - newp1Y) / (newp2X - newp1X)); }
                    catch { k = 90 * Math.PI / 180; }
                    double COS = Math.Cos(k);
                    double SIN = Math.Sin(k);
                    p1[0] = newp1X * COS + newp1Y * SIN;
                    p1[1] = -newp1X * SIN + newp1Y * COS;
                    p2[0] = newp2X * COS + newp2Y * SIN;
                    p2[1] = -newp2X * SIN + newp2Y * COS;
                    double X = Xt * COS + Yt * SIN;
                    double Y = -Xt * SIN + Yt * COS;
                    double x = (p2[0] + p1[0]) / 2;
                    double y = (p2[1] + p1[1]) / 2 + 20;
                    if ((x - 6) <= X && X <= (x + 6))
                    {
                        if ((y - 6) <= Y && Y <= (y + 6))
                        {
                            match = j;
                            return match;
                        }
                    }
                }
            }
            return match;
        }       // кликнули по кривой дуге
        public int[,] copy(List<Arc> array, int vertices) {
            int[,] newgraph = new int[vertices, 3];
            int count = 0;
            for (int i = 0; i < vertices; i++, count++) {
                newgraph[i, 0] = array[count].peak1.number_peak;
                newgraph[i, 1] = array[count].peak2.number_peak;
                newgraph[i, 2] = array[count].weight;
            }
            return newgraph;
        }       // копирует граф
        public void sort(ref int[,] newgraph, int vertices)
        {
            int size = vertices - 1;
            bool flag = false;
            while (!flag)
            {
                flag = true;
                for (int i = 0; i < size; i++)
                {
                    if (newgraph[i, 0] >= newgraph[i + 1, 0])
                    {
                        int peak1, peak2, weight;
                        flag = false;
                        if (newgraph[i, 0] == newgraph[i + 1, 0])
                        {
                            if (newgraph[i, 1] > newgraph[i + 1, 1])
                            {
                                peak1 = newgraph[i, 0];
                                peak2 = newgraph[i, 1];
                                weight = newgraph[i, 2];
                                newgraph[i, 0] = newgraph[i + 1, 0];
                                newgraph[i, 1] = newgraph[i + 1, 1];
                                newgraph[i, 2] = newgraph[i + 1, 2];
                                newgraph[i + 1, 0] = peak1;
                                newgraph[i + 1, 1] = peak2;
                                newgraph[i + 1, 2] = weight;
                            }
                        }
                        else {
                            peak1 = newgraph[i, 0];
                            peak2 = newgraph[i, 1];
                            weight = newgraph[i, 2];
                            newgraph[i, 0] = newgraph[i + 1, 0];
                            newgraph[i, 1] = newgraph[i + 1, 1];
                            newgraph[i, 2] = newgraph[i + 1, 2];
                            newgraph[i + 1, 0] = peak1;
                            newgraph[i + 1, 1] = peak2;
                            newgraph[i + 1, 2] = weight;
                        }
                       
                    }
                }
                size--;
            }
        }       // сортирует граф
        private int[,] creatSmallGraph(int[,] array, int size) {
            int[,] newgraph = new int[size, 3];
            int count = 0;
            int newsize = 0;
            for (int i = 0; i < size; i++, count++)
            {
                if (array[i, 2] > 0) {
                    newgraph[i, 0] = array[i, 0];
                    newgraph[i, 1] = array[i, 1];
                    newgraph[i, 2] = array[i, 2];
                    newsize++;
                }
            }
            int[,] newgraph2 = new int[newsize, 3];
            Array.Copy(newgraph, newgraph2, newsize);
            return newgraph;
        }   // делает маленький граф
        public List<Arc> BigGraph(List<Arc> arc, List<Peak> peak) {
            List<Arc> BigGraph = new List<Arc>(arc);
            for (int i = 0; i < peak.Count; i++)
            {   
                for (int j = 0; j < peak.Count; j ++)
                {
                    bool flag = false;
                    for (int k = 0; k < arc.Count; k++) {
                        if (peak[i].number_peak == arc[k].peak1.number_peak && peak[j].number_peak == arc[k].peak2.number_peak)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        List<int> index = new List<int>(2);
                        index.Add(i); index.Add(j);
                        Arc Line = new Arc();
                        Line.Box(peak, index, 0, false);
                        BigGraph.Add(Line);
                    }
                }
            }
                    return BigGraph;
        }   // делает большой граф
        public int[,] Matrix(List<Arc> arc, List<Peak> peak) {
            int[,] graph = new int[peak.Count * peak.Count, 3];
            arc = BigGraph(arc, peak);
            graph = copy(arc, peak.Count * peak.Count);
            sort(ref graph, peak.Count * peak.Count);
            return graph;
        }
        private int[] FloydWarshell(List<Arc> arc, List<Peak> peak, ref int comparisons, ref int permutations)
        {
            arc = BigGraph(arc, peak);
            int vertices = peak.Count;
            int[,] renewGraph = copy(arc, vertices*vertices);
            sort(ref renewGraph, vertices * vertices);
            comparisons = 0;
            permutations = 0;
            for (int k = 0; k < vertices; k++)
            {
                for (int i = 0; i < vertices; i++)
                {
                    for (int j = 0; j < vertices; j++)
                    {
                        if (renewGraph[i * vertices + k, 2] != 0 && renewGraph[k * vertices + j, 2] != 0 
                            && (renewGraph[i * vertices + k, 2] + renewGraph[k * vertices + j, 2] < renewGraph[i * vertices + j, 2] || renewGraph[i * vertices + j, 2] == 0) 
                            && renewGraph[i * vertices + k, 2] + renewGraph[k * vertices + j, 2] > 0)
                        {
                            renewGraph[i * vertices + j, 2] = (renewGraph[i * vertices + j, 2] == 0 ? renewGraph[i * vertices + k, 2] + renewGraph[k * vertices + j, 2] : 
                            Math.Min(renewGraph[i * vertices + j, 2], renewGraph[i * vertices + k, 2] + renewGraph[k * vertices + j, 2]));
                            permutations++;
                            comparisons++;
                        }
                        comparisons++;
                    }
                }
            }
            int[] distance = new int[vertices];
            for (int i = 0; i < vertices; i++)
                distance[i] = renewGraph[i, 2];
            return distance;
        }
        private int[] BellmanFord(List<Arc> graph, int vertices, ref int comparisons, ref int permutations)
        {
            int edges = graph.Count;
            int[,] regraph = copy(graph, edges);
            regraph = creatSmallGraph(regraph, edges);
            int[] distance = new int[vertices];
            comparisons = 0;
            permutations = 0;
            for (int i = 0; i < vertices; i++)
                distance[i] = int.MaxValue;
            distance[0] = 0;
            for (int i = 0; i < vertices - 1; i++)
                for (int j = 0; j < edges; j++)
                {
                    if (distance[regraph[j, 0]] + regraph[j, 2] < distance[regraph[j, 1]] && distance[regraph[j, 0]] + regraph[j, 2] > 0)
                    {
                        distance[regraph[j, 1]] = distance[regraph[j, 0]] + regraph[j, 2];
                        permutations++;
                    }
                    comparisons++;
                }
            return distance;
        }
        private int[] Dijkstra(List<Arc> arc, List<Peak> peak, ref int comparisons, ref int permutations)
        {
            arc = BigGraph(arc, peak);
            int vertices = peak.Count;
            int[,] renewGraph = copy(arc, vertices * vertices);
            sort(ref renewGraph, vertices * vertices);
            int[] distance = new int[vertices];
            int index = -1, u;
            bool[] visited = new bool[vertices];
            distance = new int[vertices];
            visited = new bool[vertices];
            comparisons = 0;
            permutations = 0;
            for (int i = 0; i < vertices; i++) { distance[i] = Int32.MaxValue; visited[i] = false; }
            distance[0] = 0;
            for (int i = 0; i < vertices - 1; i++)
            {
                int min = Int32.MaxValue;
                for (int j = 0; j < vertices; j++)
                {
                    if (!visited[j] && distance[j] <= min) { min = distance[j]; index = j; }
                    comparisons++;
                }
                u = index;
                visited[u] = true;
                for (int j = 0; j < vertices; j++)
                {
                    if (!visited[j] && renewGraph[u * vertices + j, 2] > -1 + 1 && distance[u] != Int32.MaxValue && distance[u] + renewGraph[u * vertices + j, 2] < distance[j])
                    {
                        distance[j] = distance[u] + renewGraph[u * vertices + j, 2];
                        permutations++;
                    }
                    comparisons++;
                }
            }
            return distance;
        }
        public int[] Work(List<Arc> arc, List<Peak> peak, CHOICE Choice, ref List<int> parameters)
        {
            Stopwatch stopForStop = new Stopwatch();
            int[] distance = new int[peak.Count];
            int comparisons = 0; // сравнения
            int permutations = 0; // перестановки
            switch (Choice)
            {
                case CHOICE.FloydWarshell:
                    stopForStop.Start();
                    distance = FloydWarshell(arc, peak, ref comparisons, ref permutations);
                    stopForStop.Stop();
                    parameters.Add((int)stopForStop.ElapsedMilliseconds);
                    stopForStop.Reset();
                    break;
                case CHOICE.BellmanFord:
                    stopForStop.Start();
                    distance = BellmanFord(arc, peak.Count, ref comparisons, ref permutations);
                    stopForStop.Stop();
                    parameters.Add((int)stopForStop.ElapsedMilliseconds);
                    stopForStop.Reset();
                    break;
                case CHOICE.Dijkstra:
                    stopForStop.Start();
                    distance = Dijkstra(arc, peak, ref comparisons, ref permutations);
                    stopForStop.Stop();
                    parameters.Add((int)stopForStop.ElapsedMilliseconds);
                    stopForStop.Reset();
                    break;
                case CHOICE.Analith:
                    parameters = Analith(ref distance, arc, peak);
                    break;
            }
            if (Choice != CHOICE.Analith) {
                parameters.Add(comparisons);
                parameters.Add(permutations);
            }
            return distance;
        }       // вызов алгоритмов
        private List<int> Analith(ref int[] distance, List<Arc> arc, List<Peak> peak)
        {
            Stopwatch stopForStop = new Stopwatch();
            int comparisons = 0; int permutations = 0;
            List<int> array_of_com_and_perm = new List<int>(9);
            stopForStop.Start();
            distance = FloydWarshell(arc, peak, ref comparisons, ref permutations);
            stopForStop.Stop();
            array_of_com_and_perm.Add((int)stopForStop.ElapsedMilliseconds);
            array_of_com_and_perm.Add(comparisons); array_of_com_and_perm.Add(permutations);
            stopForStop.Reset();
            ///////////
            stopForStop.Start();
            BellmanFord(arc, peak.Count, ref comparisons, ref permutations);
            stopForStop.Stop();
            array_of_com_and_perm.Add((int)stopForStop.ElapsedMilliseconds);
            array_of_com_and_perm.Add(comparisons); array_of_com_and_perm.Add(permutations);
            stopForStop.Reset();
            /////////
            stopForStop.Start();
            Dijkstra(arc, peak, ref comparisons, ref permutations);
            stopForStop.Stop();
            array_of_com_and_perm.Add((int)stopForStop.ElapsedMilliseconds);
            array_of_com_and_perm.Add(comparisons); array_of_com_and_perm.Add(permutations);
            stopForStop.Reset();
            return array_of_com_and_perm;
        }       // анализ алгоритмов
    }
} 