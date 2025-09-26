namespace MLMath.Core
{
    /// <summary>
    /// Represents an immutable, dense 2-D matrix of doubles, stored in row-major order.
    /// Data is stored in a single 1-D array. For a matrix with `r` rows and `c` columns,
    /// the element at (row, col) is at index `row * c + col` in the array. testing testing
    /// </summary>
    public sealed class Matrix
    {
        private readonly double[] _data;
        public int Rows { get; }
        public int Cols { get; }

        /// <summary>
        /// Gets the element at the specified row and column.
        /// </summary>
        /// <param name="r">The zero-based row index.</param>
        /// <param name="c">The zero-based column index.</param>
        /// <returns>The element at the specified position.</returns>
        public double this[int r, int c] => _data[r * Cols + c];

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with a defensive copy of the provided data.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        /// <param name="data">The matrix data in a 1-D row-major array.</param>
        public Matrix(int rows, int cols, double[] data)
        {
            if (rows <= 0 || cols <= 0)
            {
                throw new ArgumentException("Matrix dimensions must be positive.");
            }
            if (data == null || data.Length != rows * cols)
            {
                throw new ArgumentException("Data array length must match rows * cols.");
            }

            Rows = rows;
            Cols = cols;
            // Defensive copy to ensure immutability
            _data = (double[])data.Clone();
        }

        public static Matrix Zeros(int r, int c)
        {
            if (r <= 0 || c <= 0) throw new ArgumentException("Matrix dimensions must be positive.");
            return new Matrix(r, c, new double[r * c]);
        }

        public static Matrix Ones(int r, int c)
        {
            if (r <= 0 || c <= 0) throw new ArgumentException("Matrix dimensions must be positive.");
            var data = new double[r * c];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 1.0;
            }
            return new Matrix(r, c, data);
        }

        /// <summary>
        /// Creates an identity matrix of a specified size.
        /// An identity matrix is a square matrix with ones on the main diagonal and zeros elsewhere.
        /// </summary>
        /// <param name="n">The size (number of rows and columns) of the identity matrix.</param>
        /// <returns>An n-by-n identity matrix.</returns>
        public static Matrix Identity(int n)
        {
            if (n <= 0) throw new ArgumentException("Matrix size must be positive.");
            var m = Zeros(n, n);
            for (int i = 0; i < n; i++)
            {
                m._data[i * n + i] = 1.0;
            }
            return m;
        }

        #region Method Forms
        public Matrix Add(Matrix b) => this + b;
        public Matrix Sub(Matrix b) => this - b;
        public Vector MatVec(Vector x) => this * x;
        public Matrix MatMul(Matrix b) => this * b;

        /// <summary>
        /// Computes the transpose of this matrix.
        /// The transpose of a matrix A is another matrix Aᵀ where the element at (i, j) in Aᵀ
        /// is the element at (j, i) in A.
        /// </summary>
        /// <returns>The transposed matrix.</returns>
        public Matrix Transpose()
        {
            var result = new double[Rows * Cols];
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    result[c * Rows + r] = this[r, c];
                }
            }
            return new Matrix(Cols, Rows, result);
        }
        #endregion

        #region Operator Overloads
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new ArgumentException("Matrices must have the same dimensions for addition.");
            }
            var result = new double[a.Rows * a.Cols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = a._data[i] + b._data[i];
            }
            return new Matrix(a.Rows, a.Cols, result);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new ArgumentException("Matrices must have the same dimensions for subtraction.");
            }
            var result = new double[a.Rows * a.Cols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = a._data[i] - b._data[i];
            }
            return new Matrix(a.Rows, a.Cols, result);
        }

        public static Matrix operator -(Matrix a)
        {
            var result = new double[a.Rows * a.Cols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = -a._data[i];
            }
            return new Matrix(a.Rows, a.Cols, result);
        }

        public static Matrix operator *(Matrix a, double s)
        {
            var result = new double[a.Rows * a.Cols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = a._data[i] * s;
            }
            return new Matrix(a.Rows, a.Cols, result);
        }

        public static Matrix operator *(double s, Matrix a) => a * s;

        public static Vector operator *(Matrix a, Vector x)
        {
            if (a.Cols != x.Length)
            {
                throw new ArgumentException("Matrix column count must match vector length for multiplication.");
            }
            var result = new double[a.Rows];
            for (int r = 0; r < a.Rows; r++)
            {
                double sum = 0;
                for (int c = 0; c < a.Cols; c++)
                {
                    sum += a[r, c] * x[c];
                }
                result[r] = sum;
            }
            return new Vector(result);
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
            {
                throw new ArgumentException("Inner dimensions must match for matrix multiplication (A.Cols == B.Rows).");
            }
            var result = new double[a.Rows * b.Cols];
            for (int r = 0; r < a.Rows; r++)
            {
                for (int c = 0; c < b.Cols; c++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++) // a.Cols or b.Rows
                    {
                        sum += a[r, k] * b[k, c];
                    }
                    result[r * b.Cols + c] = sum;
                }
            }
            return new Matrix(a.Rows, b.Cols, result);
        }
        #endregion
    }
}
