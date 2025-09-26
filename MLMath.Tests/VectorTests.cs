using MLMath.Core;

namespace MLMath.Tests
{
    public class VectorTests
    {
        private const int Precision = 12;

        [Fact]
        public void AdditionAndSubtraction_Correctness()
        {
            var a = new Vector(new double[] { 1, 2, 3 });
            var b = new Vector(new double[] { 4, 5, 6 });
            var sum = a + b;
            var diff = a - b;

            Assert.Equal(5, sum[0]);
            Assert.Equal(7, sum[1]);
            Assert.Equal(9, sum[2]);

            Assert.Equal(-3, diff[0]);
            Assert.Equal(-3, diff[1]);
            Assert.Equal(-3, diff[2]);
        }

        [Fact]
        public void ScalarMultiply_BothSides_AreEqual()
        {
            var a = new Vector(new double[] { 1, 2, 3 });
            var s = 2.0;
            var scaled1 = a * s;
            var scaled2 = s * a;

            Assert.Equal(2, scaled1[0]);
            Assert.Equal(4, scaled1[1]);
            Assert.Equal(6, scaled1[2]);

            Assert.Equal(scaled1[0], scaled2[0]);
            Assert.Equal(scaled1[1], scaled2[1]);
            Assert.Equal(scaled1[2], scaled2[2]);
        }

        [Fact]
        public void DotProduct_IsCorrectAndCommutative()
        {
            var a = new Vector(new double[] { 1, 2, 3 });
            var b = new Vector(new double[] { 4, 5, 6 });

            double dot_ab = a.Dot(b);
            double dot_ba = b.Dot(a);

            Assert.Equal(32, dot_ab); // 1*4 + 2*5 + 3*6 = 4 + 10 + 18 = 32
            Assert.Equal(dot_ab, dot_ba);
        }

        [Fact]
        public void NormsAndNormalize_AreCorrect()
        {
            var a = new Vector(new double[] { 3, -4, 0 }); // A 3-4-5 triangle
            var zeroVec = Vector.Zeros(3);

            Assert.Equal(7, a.NormL1(), Precision); // |3| + |-4| + |0| = 7
            Assert.Equal(5, a.NormL2(), Precision); // sqrt(9 + 16 + 0) = 5

            var normalizedA = a.Normalize();
            Assert.Equal(1.0, normalizedA.NormL2(), Precision);

            // Normalizing a zero vector should result in a zero vector
            var normalizedZero = zeroVec.Normalize();
            Assert.Equal(0.0, normalizedZero.NormL2(), Precision);
            Assert.Equal(0.0, normalizedZero[0]);
        }

        [Fact]
        public void MismatchedLengths_ThrowArgumentException()
        {
            var a = new Vector(new double[] { 1, 2 });
            var b = new Vector(new double[] { 1, 2, 3 });

            Assert.Throws<ArgumentException>(() => a + b);
            Assert.Throws<ArgumentException>(() => a - b);
            Assert.Throws<ArgumentException>(() => a.Dot(b));
        }

        [Fact]
        public void UnaryNegation_IsCorrect()
        {
            var a = new Vector(new double[] { 1, -2, 3 });
            var negA = -a;

            Assert.Equal(-1, negA[0]);
            Assert.Equal(2, negA[1]);
            Assert.Equal(-3, negA[2]);
        }
    }
}
