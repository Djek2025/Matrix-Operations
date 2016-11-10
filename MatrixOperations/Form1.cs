using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixOperations
{
    public partial class Form1 : Form
    {
        public double[,] bufer1, bufer2;
/*—————————————————————————Створення, і заповнненя Grid(a) 1-ицями по головній діагоналі————————————————————————————*/
        private void creatdatagrid (ref DataGridView c, string t1, string t2)
        {
            int x, y;
            while (!int.TryParse(t1, out x) | x < 1 || !int.TryParse(t2, out y) | y < 1)
            {
                MessageBox.Show("Помилка в данних!");
                textBox1.Focus();
                return;
            }
            c.ColumnCount = x;
            c.RowCount = y;
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    c[i, j].Value = i == j ? "1" : "0";
        }
/*—————————————————————————————————————————Заповнення Grid-а переданим массивом—————————————————————————————————————*/
        private void arrayindatagrid (double[,] arr, ref DataGridView dat)
        {
            int x = arr.GetLength(0), y = arr.GetLength(1);
            dat.ColumnCount = x; dat.RowCount = y;
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    dat[i, j].Value = arr[i,j];
        }
/*——————————————————————————————————Заповнення двовимірного массиву Grid(ом)————————————————————————————————————————*/
        private double[,] inarray(double[,] matrix, ref DataGridView c)
        {
            int x = Convert.ToInt32(c.ColumnCount), y = Convert.ToInt32(c.RowCount);
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                { matrix[i, j] = Convert.ToDouble(c[i, j].Value); }
            return matrix;
        }
/*—————————————————————————————————————————Сумма двох двовимірних масивів——————————————————————————————————————————————*/
        private void sum (double[,] mat1, double[,] mat2)
        {
            int x1=dataGridView1.RowCount, x2=dataGridView2.RowCount, y1=dataGridView1.ColumnCount,y2= dataGridView2.ColumnCount;
            if (x1 != x2 & y1 != y2) { MessageBox.Show("Матриці різного розміру"); return; } else
            {
                dataGridView3.RowCount = x1;
                dataGridView3.ColumnCount = y1;
                for (int i = 0; i < x1; i++)
                    for (int j = 0; j < y1; j++)
                    {
                        dataGridView3[i, j].Value = mat1[i, j] + mat2[i, j];
                    }
            }
        }
/*———————————————————————————————————————Різниця двох двовимірних масивів——————————————————————————————————————————*/
        private void dif(double[,] mat1, double[,] mat2)
        {
            int x1=dataGridView1.RowCount, x2=dataGridView2.RowCount, y1=dataGridView1.ColumnCount, y2=dataGridView2.ColumnCount;
            if (x1 == x2 & y1 == y2)
            {
                dataGridView3.RowCount = x1;
                dataGridView3.ColumnCount = y1;
                for (int i = 0; i < x1; i++)
                    for (int j = 0; j < y1; j++)
                    {
                        dataGridView3[i, j].Value = mat1[i, j] - mat2[i, j];
                    }
            }
        }
/*—————————————————————————————————Визначник матриці (Детермінант 1 спосіб)——————————————————————————————————*/
        static double DeterminantGaussElimination(double[,] matrix)
        {
            int n = int.Parse(System.Math.Sqrt(matrix.Length).ToString());
            int nm1 = n - 1;
            int kp1;
            double p;
            double det = 1;
            for (int k = 0; k < nm1; k++)
            {
                kp1 = k + 1;
                for (int i = kp1; i < n; i++)
                {
                    p = matrix[i, k] / matrix[k, k];
                    for (int j = kp1; j < n; j++)
                        matrix[i, j] = matrix[i, j] - p * matrix[k, j];
                }
            }
            for (int i = 0; i < n; i++)
                det = det * matrix[i, i];
            return det;
        }
/*—————————————————————————————————Визначник матриці (Детермінант 2 спосіб)————————————————————————————————————————————*/
        public static double Determinant(double[,] A)
        {
            double det = 1, divideEl;
            int size = A.GetLength(0);
            double[,] resultMatrx = A;

            for (int i = 0; i < size - 1; i++)
            {
                if (resultMatrx[i, i] == 0)
                {
                    int k = i + 1;
                    while (resultMatrx[i, k] == 0)
                    {
                        k++;
                        if (k == size)
                        break;
                    }
                    if (k == size)
                        return 0;
                    else
                    {
                        double[,] column = new double[size, 1];      //додатковий стовпець
                        for (int p = 0; p < size; p++)               //Обмін стовпців
                        {
                            column[p, 0] = resultMatrx[p, i];
                            resultMatrx[p, i] = resultMatrx[p, k];
                            resultMatrx[p, k] = column[p, 0];
                            det *= -1;                      //зміна знака при перестановці стовпців
                        }
                    }
                }
                divideEl = 1.0 / resultMatrx[i, i];
                for (int j = size - 1; j >= 0; j--)
                    for (int k = i + 1; k < size; k++)
                        resultMatrx[k, j] -= resultMatrx[i, j] * divideEl * resultMatrx[k, i];
            }
            for (int i = 0; i < size; i++)
                det *= resultMatrx[i, i];
            return det;
        }
/*—————————————————————Класи та методи для визначення оберненої матриці————————————————————————————*/
        public class Fraction
        {
            long numerator, denominator;

            public long Numerator
            {
                get { return numerator; }
                set { numerator = value; }
            }

            public long Denominator
            {
                get { return denominator; }
                set
                {
                    if (value != 0) denominator = value;
                    else throw new ArithmeticException("Чисельне значення знаменнику 0, не визначено.");
                }
            }

            public Fraction Reciprocal()
            {
                if (numerator == 0) throw new ArithmeticException("Чисельне значення знаменнику 0");
                else
                    return new Fraction(denominator, numerator);
            }

            public Fraction Irreducible()
            {
                if (this.Numerator == 0)
                    return new Fraction(0, 1);

                long gcd = Euclid(this.Numerator, this.Denominator);
                long numer, denom;
                numer = this.Numerator / gcd;
                denom = this.Denominator / gcd;

                if (denom < 0)
                {
                    denom = -denom; numer = -numer;
                }

                return new Fraction(numer, denom);
            }

            private long Euclid(long a, long b)
            {
                long big, small, r;
                a = Math.Abs(a);
                b = Math.Abs(b);
                if (a > b)
                {
                    big = a; small = b;
                }
                else
                {
                    big = b; small = a;
                }

                r = big % small;

                if (r == 0)
                    return small;
                else
                    return Euclid(small, r);
            }

            public static Fraction operator -(Fraction obj1, Fraction obj2)
            {
                long r_numer1, r_numer2, r_denom;
                checked
                {
                    r_numer1 = obj1.Numerator * obj2.Denominator;
                    r_numer2 = obj2.Numerator * obj1.Denominator;
                    r_denom = obj2.Denominator * obj1.Denominator;
                }
                return new Fraction(r_numer1 - r_numer2, r_denom);
            }

            public static Fraction operator *(Fraction obj1, Fraction obj2)
            {
                return new Fraction(checked(obj1.Numerator * obj2.Numerator), checked(obj1.Denominator * obj2.Denominator));
            }

            public static implicit operator Fraction(int obj)
            {
                return new Fraction(obj, 1);
            }

            public Fraction(long num, long den)
            {
                numerator = num;
                if (den == 0) denominator = 1;
                else denominator = den;
            }
        }


        public class Matrix
        {
            public Fraction[,] Value;
            int col, row;
            public int Column { get { return col; } }
            public int Row { get { return row; } }
            public Matrix Inverse()
            {
                int n = row;
                Fraction[,] ext = new Fraction[n, n * 2];
                Fraction[] buff_1 = new Fraction[n * 2];
                Fraction[] buff_2 = new Fraction[n * 2];
                Fraction[] buff_res = new Fraction[n * 2];
                long[] ol_numer = new long[n * 2];
                long[] ol_denom = new long[n * 2];
                if (row != col)
                    throw new ArithmeticException("Матриця не квадратна");

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n * 2; j++)
                    {
                        if (j < n)
                        {
                            ext[i, j] = Value[i, j];
                        }
                        else
                        {
                            if (i == (j - n))
                                ext[i, j] = (Fraction)1;
                            else
                                ext[i, j] = (Fraction)0;
                        }
                    }
                }

                for (int j = 0; j < n - 1; j++)
                {
                    for (int i = j + 1; i < n; i++)
                    {
                        if (ext[j, j].Numerator == 0)
                        {
                            bool ischange = false;
                            int k = 0;
                            do
                            {
                                if (ext[k, j].Numerator != 0)
                                {
                                    Fraction[] tmp = new Fraction[n * 2];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        tmp[kk] = ext[j, kk];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        ext[j, kk] = ext[k, kk];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        ext[k, kk] = tmp[kk];
                                    ischange = true;
                                }
                                k++;
                            } while (!ischange);
                        }
 
                        for (int k = 0; k < n; k++)
                        {
                            for (int kk = 0; kk < n * 2; kk++)
                            {
                                ol_numer[kk] = ext[k, kk].Numerator;
                                ol_denom[kk] = ext[k, kk].Denominator;
                            }
                         
                            long nu_gcd = 1; int en = 0;
                            for (int kk = 0; kk < ol_numer.Length; kk++)
                            {
                                if (ol_numer[kk] != 0)
                                {
                                    nu_gcd = ol_numer[kk];
                                    en = kk;
                                    break;
                                }
                            }

                            for (int kk = en + 1; kk < ol_numer.Length; kk++)
                            {
                                if (ol_numer[kk] != 0)
                                    nu_gcd = Euclid(nu_gcd, ol_numer[kk]);
                            }

                            long de_gcd = 1;
                            for (int kk = 0; kk < ol_denom.Length; kk++)
                            {
                                if (ol_denom[kk] != 0)
                                {
                                    de_gcd = ol_denom[kk];
                                    en = kk;
                                    break;
                                }
                            }

                            for (int kk = en + 1; kk < ol_denom.Length; kk++)
                            {
                                if (ol_denom[kk] != 0)
                                    de_gcd = Euclid(de_gcd, ol_denom[kk]);
                            }
                            Fraction fr_gcd = new Fraction(de_gcd, nu_gcd);
                                                                            
                            for (int kk = 0; kk < n * 2; kk++)
                                ext[k, kk] = (ext[k, kk] * fr_gcd).Irreducible();
                        }

                        for (int k = 0; k < n * 2; k++)
                        {
                            buff_1[k] = (ext[j, k] * ext[i, j]).Irreducible();
                            buff_2[k] = (ext[i, k] * ext[j, j]).Irreducible();
                            buff_res[k] = (buff_1[k] - buff_2[k]).Irreducible();
                        }
                        for (int k = 0; k < n * 2; k++)
                        {
                            buff_res[k] = buff_res[k].Irreducible();
                            ext[i, k] = buff_res[k];
                        }

                    }
                }

                if (ext[n - 1, n - 1].Numerator == 0)
                {
                    throw new ArithmeticException("Матриця не регулярна");
                }

                for (int j = n - 1; j > 0; j--)
                {
                    for (int i = j - 1; i >= 0; i--)
                    {
                        if (ext[j, j].Numerator == 0)
                        {
                            bool ischange = false;
                            int k = 0;
                            do
                            {
                                if (ext[k, j].Numerator != 0)
                                {
                                    Fraction[] tmp = new Fraction[n * 2];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        tmp[kk] = ext[j, kk];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        ext[j, kk] = ext[k, kk];
                                    for (int kk = 0; kk < n * 2; kk++)
                                        ext[k, kk] = tmp[kk];
                                    ischange = true;
                                }
                                k++;
                            } while (!ischange);
                        }

                        for (int k = 0; k < n; k++)
                        {
                            for (int kk = 0; kk < n * 2; kk++)
                            {
                                ol_numer[kk] = ext[k, kk].Numerator;
                                ol_denom[kk] = ext[k, kk].Denominator;
                            }

                            long nu_gcd = 1; int en = 0;
                            for (int kk = 0; kk < ol_numer.Length; kk++)
                            {
                                if (ol_numer[kk] != 0)
                                {
                                    nu_gcd = ol_numer[kk];
                                    en = kk;
                                    break;
                                }
                            }

                            for (int kk = en + 1; kk < ol_numer.Length; kk++)
                            {
                                if (ol_numer[kk] != 0)
                                    nu_gcd = Euclid(nu_gcd, ol_numer[kk]);
                            }

                            long de_gcd = 1;
                            for (int kk = 0; kk < ol_denom.Length; kk++)
                            {
                                if (ol_denom[kk] != 0)
                                {
                                    de_gcd = ol_denom[kk];
                                    en = kk;
                                    break;
                                }
                            }

                            for (int kk = en + 1; kk < ol_denom.Length; kk++)
                            {
                                if (ol_denom[kk] != 0)
                                    de_gcd = Euclid(de_gcd, ol_denom[kk]);
                            }
                            Fraction fr_gcd = new Fraction(de_gcd, nu_gcd);
                                                                           
                            for (int kk = 0; kk < n * 2; kk++)
                                ext[k, kk] = (ext[k, kk] * fr_gcd).Irreducible();
                        }

                        for (int k = 0; k < n * 2; k++)
                        {
                            buff_1[k] = (ext[j, k] * ext[i, j]).Irreducible();
                            buff_2[k] = (ext[i, k] * ext[j, j]).Irreducible();
                            buff_res[k] = (buff_1[k] - buff_2[k]).Irreducible();
                        }
                        for (int k = 0; k < n * 2; k++)
                        {
                            buff_res[k] = buff_res[k].Irreducible();
                            ext[i, k] = buff_res[k];
                        }
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    Fraction rec = ext[i, i].Reciprocal();
                    for (int k = 0; k < n * 2; k++)
                    {
                        ext[i, k] = ext[i, k] * rec;
                        ext[i, k] = ext[i, k].Irreducible();
                    }
                }

                Fraction[,] result = new Fraction[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = n; j < n * 2; j++)
                    {
                        result[i, j - n] = ext[i, j];
                    }
                }
                return new Matrix(result);
            }

            public void Show(ref DataGridView tat)
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (Value[i, j].Denominator == 1)
                             tat[i,j].Value = Convert.ToString(Value[i, j].Numerator);
                        else
                            tat[i,j].Value = Convert.ToString(Value[i, j].Numerator + "/" + Value[i, j].Denominator + "\t");
                    }
                }
            }

            private long Euclid(long a, long b)
            {
                long big, small, r;
                a = Math.Abs(a);
                b = Math.Abs(b);
                if (a > b)
                {
                    big = a; small = b;
                }
                else
                {
                    big = b; small = a;
                }
                r = big % small;
                if (r == 0)
                    return small;
                else
                    return Euclid(small, r);
            }

            public Matrix(Fraction[,] value)
            {
                Value = value;
                row = value.GetLength(0); col = value.GetLength(1);
            }
        }



        static public Fraction[] ToRow(String str, int col)
        {
            Fraction[] result = new Fraction[col];

            int last_indexer = -1;
            for (int i = 0; i < col - 1; i++)
            {
                last_indexer = str.IndexOf(' ', last_indexer + 1);
                if (last_indexer < 0)
                {
                    throw new Exception();
                }
            }

            for (int i = 0; i < col; i++)
            {
                long numer, denom;
                int sp_in = str.IndexOf(' ');    
                String value = null;

                if (sp_in != -1)
                {
                    value = str.Substring(0, sp_in + 1);
                    str = str.Remove(0, sp_in + 1);
                }
                else
                {    
                    value = str;
                }

                int sla_in = value.IndexOf('/');
                int dot_in = value.IndexOf('.');
                if (sla_in != -1)
                {                
                    numer = long.Parse(value.Substring(0, sla_in));
                    denom = long.Parse(value.Substring(sla_in + 1));
                }
                else if (dot_in != -1)
                {    
                    String int_part = value.Substring(0, dot_in);
                    String dec_part = value.Substring(dot_in + 1);
                    if (i == col - 1)
                        denom = (long)Math.Pow(10, dec_part.Length);
                    else
                        denom = (long)Math.Pow(10, dec_part.Length - 1);
                    numer = long.Parse(int_part + dec_part);
                }
                else
                {
                    numer = long.Parse(value);
                    denom = 1;
                }

                result[i] = new Fraction(numer, denom);
            }
            return result;
        }

/*———————————————————————————————————————Заповнення випадковими числами———————————————————————————————————————*/
        private void randomgrid (ref DataGridView dat)
        {
            Random rand = new Random();
            if (dat.RowCount == 0 | dat.ColumnCount == 0)
            {
                MessageBox.Show("Введіть розмірність матриці і створіть її\nперш ніж заповнити випадковими числами");
                return;
            }
            for (int i = 0; i < dat.ColumnCount; i++)
                for (int j = 0; j < dat.RowCount; j++)
                    dat[i, j].Value = Convert.ToDouble(rand.Next(-10, 10));
        }
/*——————————————————————————————Заповнення буферів та витягування з них—————————————————————————————*/
        private void inbuf1 (ref DataGridView grid, double[,] bufer)
        {
            if (Convert.ToInt32(grid.RowCount) > 1 == Convert.ToInt32(grid.ColumnCount) > 1)
            {
                int x= Convert.ToInt32(grid.RowCount), y= Convert.ToInt32(grid.ColumnCount);
                bufer = new double [x, y];
                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                        bufer[i, j] = Convert.ToDouble(grid[i,j].Value);
            }
            else
            {
                MessageBox.Show("Массив менше 2х2");
                return;
            }
            bufer1 = bufer;
        }

        private void inbuf2(ref DataGridView grid, double[,] bufer)
        {
            if (Convert.ToInt32(grid.RowCount) > 1 == Convert.ToInt32(grid.ColumnCount) > 1)
            {
                int x = Convert.ToInt32(grid.RowCount), y = Convert.ToInt32(grid.ColumnCount);
                bufer = new double[x, y];
                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                        bufer[i, j] = Convert.ToDouble(grid[i, j].Value);
            }
            else
            {
                MessageBox.Show("Массив менше 2х2");
                return;
            }
            bufer2 = bufer;
        }

        private void frombuf (ref DataGridView grid,double[,] bufer)
        {
            grid.RowCount = bufer.GetLength(0); grid.ColumnCount = bufer.GetLength(1);
            for (int i = 0; i < bufer.GetLength(0); i++)
                for (int j = 0; j < bufer.GetLength(1); j++)
                    grid[i, j].Value = bufer[i, j];
        }

/*——————————————————————————————————————————————————————————————————————————————————————————————————————*/
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Ведіть розмір матриці");
                return;
            }
            else if (Convert.ToDouble(textBox1.Text) != Convert.ToDouble(textBox2.Text))
            {   MessageBox.Show("К-ть рядків не співпадає з к-тью стовпців");
                return;}
            creatdatagrid (ref dataGridView1, textBox1.Text, textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Ведіть розмір матриці");
                return;
            }
            else if (Convert.ToDouble(textBox3.Text) != Convert.ToDouble(textBox4.Text)){
                MessageBox.Show("К-ть рядків не співпадає з к-тью стовпців");
                return; }
            creatdatagrid (ref dataGridView2, textBox3.Text, textBox4.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double[,] matrix1 = new double[dataGridView1.RowCount, dataGridView1.ColumnCount],
                      matrix2 = new double[dataGridView2.RowCount, dataGridView2.ColumnCount];
            matrix1 = inarray (matrix1, ref dataGridView1);
            matrix2 = inarray (matrix2, ref dataGridView2);
            sum(matrix1, matrix2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double[,] matrix1 = new double[dataGridView1.RowCount, dataGridView1.ColumnCount],
                      matrix2 = new double[dataGridView2.RowCount, dataGridView2.ColumnCount];
            matrix1 = inarray (matrix1, ref dataGridView1);
            matrix2 = inarray (matrix2, ref dataGridView2);
            dif(matrix1, matrix2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double[,] matrix1 = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            double a;
            matrix1 = inarray(matrix1, ref dataGridView1);
            a = Determinant(matrix1);
            MessageBox.Show("Determinant Matrix A = " + a);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            double[,] matrix2 = new double[dataGridView2.RowCount, dataGridView2.ColumnCount];
            double a;
            matrix2 = inarray(matrix2, ref dataGridView2);
            a = Determinant(matrix2);
            MessageBox.Show("Determinant Matrix B = " + a);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            double[,] a = new double[Convert.ToInt32(dataGridView1.RowCount), Convert.ToInt32(dataGridView1.ColumnCount)];

            int x = Convert.ToInt32(dataGridView1.RowCount), y = Convert.ToInt32(dataGridView1.ColumnCount);
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    a[i,j] = Convert.ToDouble(dataGridView1[i, j].Value);
            dataGridView3.RowCount = x;
            dataGridView3.ColumnCount = y;

            Fraction[,] fr = new Fraction[x, y];
            for (int i = 0; i < x; i++)
            {
                string buff = "";
                for(int j=0;j<x;j++)
                {
                    if ( x-j >= 1) buff += Convert.ToString(dataGridView1[i, j].Value) + " ";
                    else buff += Convert.ToString(dataGridView1[i, j].Value);
                }
                String rowstr = buff;
                Fraction[] rowfr = new Fraction[x];
                try
                {
                    rowfr = ToRow(rowstr, x);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Формат введення не працює належним чином");
                    i--; continue;
                }
                catch (Exception)
                {
                    MessageBox.Show("Елемент мало. Будь ласка, спробуйте ще раз.");
                    i--; continue;
                }
                for (int k = 0; k < x; k++)
                    fr[i, k] = rowfr[k];
            }

            Matrix mr = new Matrix(fr);
            try
            {
                mr = mr.Inverse();
                mr.Show(ref dataGridView3);
            }
            catch (Exception)
            {
                MessageBox.Show("Матриця не регулярна");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            randomgrid(ref dataGridView1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            randomgrid(ref dataGridView2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            inbuf1(ref dataGridView3, bufer1);
            button12.Enabled = true;
            button14.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            inbuf2(ref dataGridView3, bufer2);
            button13.Enabled = true;
            button15.Enabled = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            frombuf(ref dataGridView2, bufer2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            frombuf(ref dataGridView2, bufer1);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            frombuf(ref dataGridView1, bufer2);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            inbuf2(ref dataGridView2, bufer2);
            button13.Enabled = true;
            button15.Enabled = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            inbuf2(ref dataGridView1, bufer2);
            button13.Enabled = true;
            button15.Enabled = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            inbuf1(ref dataGridView2, bufer1);
            button12.Enabled = true;
            button14.Enabled = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            inbuf1(ref dataGridView1, bufer1);
            button12.Enabled = true;
            button14.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            frombuf(ref dataGridView1, bufer1);
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8 && e.KeyChar != 44)
                e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}
