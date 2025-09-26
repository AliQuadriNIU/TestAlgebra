using MLMath.Core;

namespace MLMath.Tests
{
    public class MatrixTests
    {
        private const int Precision = 12;

        [Fact]
        public void MatrixMatrixMultiply_IsCorrect()
        {
            // A is 2x3, B is 3x2, C should be 2x2
            var A = new Matrix(2, 3, new double[] { 1, 2, 3, 4, 5, 6 });
            var B = new Matrix(3, 2, new double[] { 7, 8, 9, 10, 11, 12 });
            var C = A * B;

            Assert.Equal(2, C.Rows);
            Assert.Equal(2, C.Cols);

            // C[0,0] = 1*7 + 2*9 + 3*11 = 7 + 18 + 33 = 58
            Assert.Equal(58, C[0, 0]);
            // C[0,1] = 1*8 + 2*10 + 3*12 = 8 + 20 + 36 = 64
            Assert.Equal(64, C[0, 1]);
            // C[1,0] = 4*7 + 5*9 + 6*11 = 28 + 45 + 66 = 139
            Assert.Equal(139, C[1, 0]);
            // C[1,1] = 4*8 + 5*10 + 6*12 = 32 + 50 + 72 = 154
            Assert.Equal(154, C[1, 1]);
        }

        [Fact]
        public void MatrixVectorMultiply_IsCorrect()
        {
            var A = new Matrix(2, 3, new double[] { 1, 2, 3, 4, 5, 6 });
            var x = new Vector(new double[] { 7, 8, 9 });
            var y = A * x;

            Assert.Equal(2, y.Length);
            // y[0] = 1*7 + 2*8 + 3*9 = 7 + 16 + 27 = 50
            Assert.Equal(50, y[0]);
            // y[1] = 4*7 + 5*8 + 6*9 = 28 + 40 + 54 = 122
            Assert.Equal(122, y[1]);
        }

        [Fact]
        public void IdentityProperties_HoldTrue()
        {
            var A = new Matrix(2, 3, new double[] { 1, 2, 3, 4, 5, 6 });
            var I2 = Matrix.Identity(2);
            var I3 = Matrix.Identity(3);

            var res1 = I2 * A; // 2x2 * 2x3 -> 2x3
            var res2 = A * I3; // 2x3 * 3x3 -> 2x3

            for (int i = 0; i < A.Rows * A.Cols; i++)
            {
                Assert.Equal(A[i / A.Cols, i % A.Cols], res1[i / res1.Cols, i % res1.Cols], Precision);
                Assert.Equal(A[i / A.Cols, i % A.Cols], res2[i / res2.Cols, i % res2.Cols], Precision);
            }
        }

        [Fact]
        public void Transpose_IsCorrect()
        {
            var A = new Matrix(2, 3, new double[] { 1, 2, 3, 4, 5, 6 });
            var AT = A.Transpose();
            var ATT = AT.Transpose();

            Assert.Equal(3, AT.Rows);
            Assert.Equal(2, AT.Cols);
            Assert.Equal(A[0, 1], AT[1, 0]); // A[0,1] is 2, AT[1,0] should be 2
            Assert.Equal(A[1, 2], AT[2, 1]); // A[1,2] is 6, AT[2,1] should be 6

            // (A^T)^T == A
            Assert.Equal(A.Rows, ATT.Rows);
            Assert.Equal(A.Cols, ATT.Cols);
            for (int r = 0; r < A.Rows; r++)
            {
                for (int c = 0; c < A.Cols; c++)
                {
                    Assert.Equal(A[r, c], ATT[r, c]);
                }
            }
        }

        [Fact]
        public void MismatchedShapes_ThrowArgumentException()
        {
            var m2x3 = new Matrix(2, 3, new double[6]);
            var m3x2 = new Matrix(3, 2, new double[6]);
            var v2 = new Vector(new double[2]);

            // Addition/Subtraction
            Assert.Throws<ArgumentException>(() => m2x3 + m3x2);
            Assert.Throws<ArgumentException>(() => m2x3 - m3x2);

            // Matrix-Vector multiply
            Assert.Throws<ArgumentException>(() => m2x3 * v2); // 2x3 * 2x1 is invalid

            // Matrix-Matrix multiply
            Assert.Throws<ArgumentException>(() => m2x3 * m2x3); // 2x3 * 2x3 is invalid
        }
    }
}
