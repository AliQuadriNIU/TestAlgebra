using MLMath.Core;
using MLMath.Stats;

namespace MLMath.Tests
{
    public class StatsTests
    {
        private const int Precision = 12;

        [Fact]
        public void Mean_IsCorrect()
        {
            var x = new Vector(new double[] { 1, 2, 3 });
            double mu = Descriptive.Mean(x);
            Assert.Equal(2.0, mu, Precision);

            var y = new Vector(new double[] { 10, 20, 30, 40, 50 });
            double mu_y = Descriptive.Mean(y);
            Assert.Equal(30.0, mu_y, Precision);
        }

        [Fact]
        public void Variance_And_StdDev_AreCorrect()
        {
            var x = new Vector(new double[] { 1, 2, 3 }); // mean is 2

            // Population variance: ((1-2)^2 + (2-2)^2 + (3-2)^2) / 3 = (1 + 0 + 1) / 3 = 2/3
            double varPop = Descriptive.Variance(x, unbiased: false);
            Assert.Equal(2.0 / 3.0, varPop, Precision);

            // Sample variance: ((1-2)^2 + (2-2)^2 + (3-2)^2) / (3-1) = (1 + 0 + 1) / 2 = 1
            double varSmp = Descriptive.Variance(x, unbiased: true);
            Assert.Equal(1.0, varSmp, Precision);

            // StdDev is sqrt of variance
            double sdPop = Descriptive.StdDev(x, unbiased: false);
            Assert.Equal(Math.Sqrt(2.0 / 3.0), sdPop, Precision);

            double sdSmp = Descriptive.StdDev(x, unbiased: true);
            Assert.Equal(1.0, sdSmp, Precision);
        }
    }
}
