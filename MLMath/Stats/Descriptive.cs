using MLMath.Core;

namespace MLMath.Stats
{
    /// <summary>
    /// Provides methods for calculating descriptive statistics on vectors.
    /// Mathematical Definitions (1-based index):
    /// Mean: μ = (1/n) Σ_i x_i
    /// Population Variance: σ² = (1/n) Σ_i (x_i − μ)²
    /// Sample (unbiased) Variance: s² = (1/(n−1)) Σ_i (x_i − μ)²
    /// </summary>
    public static class Descriptive
    {
        public const double Epsilon = 1e-12;

        /// <summary>
        /// Calculates the arithmetic mean (average) of the elements in a vector.
        /// </summary>
        /// <param name="x">The input vector.</param>
        /// <returns>The mean of the vector's elements.</returns>
        public static double Mean(Vector x)
        {
            if (x.Length == 0) return 0.0;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i];
            }
            return sum / x.Length;
        }

        /// <summary>
        /// Calculates the variance of the elements in a vector.
        /// </summary>
        /// <param name="x">The input vector.</param>
        /// <param name="unbiased">If true, calculates the sample variance (divides by n-1). If false, calculates the population variance (divides by n).</param>
        /// <returns>The variance of the vector's elements.</returns>
        public static double Variance(Vector x, bool unbiased = true)
        {
            if (x.Length == 0) return 0.0;
            if (unbiased && x.Length < 2)
            {
                throw new ArgumentException("Sample variance requires at least 2 data points.");
            }

            double mean = Mean(x);
            double sumOfSquares = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sumOfSquares += (x[i] - mean) * (x[i] - mean);
            }

            int denominator = unbiased ? x.Length - 1 : x.Length;
            return sumOfSquares / denominator;
        }

        /// <summary>
        /// Calculates the standard deviation of the elements in a vector.
        /// </summary>
        /// <param name="x">The input vector.</param>
        /// <param name="unbiased">If true, calculates the sample standard deviation. If false, calculates the population standard deviation.</param>
        /// <returns>The standard deviation of the vector's elements.</returns>
        public static double StdDev(Vector x, bool unbiased = true)
        {
            return Math.Sqrt(Variance(x, unbiased));
        }
    }
}
