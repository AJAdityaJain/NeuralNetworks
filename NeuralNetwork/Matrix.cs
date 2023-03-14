using System;
using System.Xml.Serialization;

namespace NeuralNetworks
{
    public class Matrix
    {
        public int rows;
        public int cols;
        [XmlIgnore]
        public float[,] data;
        [XmlArray("Data")]
        public float[] ReadingsDto
        {
            get { return Flatten(data); }
            set { data = Expand(value, rows); }
        }
        public Matrix()
        {
            rows = 0;
            cols = 0;
            data = new float[0, 0];
        }
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            data = new float[rows, cols];
        }

        public Matrix copy()
        {
            Matrix m = new(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m.data[i, j] = data[i, j];
                }
            }
            return m;
        }
        public static Matrix fromArray(float[] arr)
        {
            Matrix m = new(arr.Length, 1);
            for (int i = 0; i < arr.Length; i++)
            {
                m.data[i, 0] = arr[i];
            }
            return m;
        }
        public float[] toArray()
        {
            int a = 0;
            float[] arr = new float[cols * rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    arr[a] = data[i, j];
                    a++;
                }
            }
            return arr;
        }

        public void add(float n)
        {
            map((_, i, j) => data[i, j] + n);
        }
        public void add(Matrix a)
        {
            if (a.rows != rows || a.cols != cols)
            {
                Console.WriteLine("Columns and Rows of A must match Columns and Rows of B.");
                return;
            }

            map((_, i, j) => a.data[i, j] + data[i, j]);
        }

        public void multiply(Matrix a)
        {
            // Matrix product
            if (a.rows != rows || a.cols != cols)
            {
                Console.WriteLine("Columns and Rows of A must match Columns and Rows of B.");
                return;
            }

            map((_, i, j) => a.data[i, j] * data[i, j]);
        }
        public void multiply(float a)
        {
            map((_, i, j) => a * data[i, j]);
        }

        public static Matrix subtract(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols)
            {
                Console.WriteLine("Columns and Rows of A must match Columns and Rows of B.");
                return new Matrix(0, 0);
            }

            // Return a new Matrix a-b
            return new Matrix(a.rows, a.cols)
              .map((_, i, j) => a.data[i, j] - b.data[i, j]);
        }
        public static Matrix multiply(Matrix a, Matrix b)
        {
            // Matrix product
            if (a.cols != b.rows)
            {
                Console.WriteLine("Columns of A must match rows of B.");
                return new Matrix(0, 0);
            }

            return new Matrix(a.rows, b.cols)
              .map((e, i, j) =>
              {
                  // Dot product of values in col
                  float sum = 0;
                  for (int k = 0; k < a.cols; k++)
                  {
                      sum += a.data[i, k] * b.data[k, j];
                  }
                  return sum;
              });
        }

        public static T[] Flatten<T>(T[,] arr)
        {
            int rows0 = arr.GetLength(0);
            int rows1 = arr.GetLength(1);
            T[] arrFlattened = new T[rows0 * rows1];
            for (int j = 0; j < rows1; j++)
            {
                for (int i = 0; i < rows0; i++)
                {
                    var test = arr[i, j];
                    arrFlattened[i + j * rows0] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(T[] arr, int rows0)
        {
            int length = arr.GetLength(0);
            int rows1 = length / rows0;
            T[,] arrExpanded = new T[rows0, rows1];
            for (int j = 0; j < rows1; j++)
            {
                for (int i = 0; i < rows0; i++)
                {
                    arrExpanded[i, j] = arr[i + j * rows0];
                }
            }
            return arrExpanded;
        }

        public Matrix randomize()
        {
            return map((e, _, _) => (float)(new Random().NextDouble() * 2 - 1));
        }
        public Matrix map(Func<float, int, int, float> func)
        {
            // Apply a function to every element of matrix
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    float val = data[i, j];
                    data[i, j] = func(val, i, j);
                }
            }
            return this;
        }



        public static Matrix transpose(Matrix matrix)
        {
            return new Matrix(matrix.cols, matrix.rows)
              .map((_, i, j) => matrix.data[j, i]);
        }

        public static Matrix map(Matrix matrix, Func<float, int, int, float> func)
        {
            // Apply a function to every element of matrix
            return new Matrix(matrix.rows, matrix.cols)
              .map((e, i, j) => func(matrix.data[i, j], i, j));
        }

        public void normalize()
        {
            float max = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (data[i, j] > max)
                    {
                        max = data[i, j];
                    }
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] /= max;
                }
            }
        }
    }
}
