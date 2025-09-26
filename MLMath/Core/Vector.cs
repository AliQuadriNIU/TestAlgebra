namespace MLMath.Core
{
    /// <summary>Immutable dense 1‑D vector of doubles.</summary>
    public sealed class Vector
    {
        //define data storage
        private readonly double[] _data;

        /// <summary>
        /// Gets the number of elements in the vector.
        /// </summary>
        public int Length => _data.Length;

        /// <summary>
        /// implement indexer
        /// </summary>
        /// <param name="i">The zero-based index of the element to get from data</param>
        /// <returns>The element at the specified index</returns>
        public double this[int i] => _data[i];

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class with a defensive copy of the provided data.
        /// </summary>
        /// <param name="data">The array of doubles to initialize the vector with.</param>
        public Vector(double[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            // data placed inside an array
            _data = (double[])data.Clone();
        }

        /// <summary>
        /// Creates a new vector of a specified length, filled with zeros.
        /// </summary>
        /// <param name="n">The length of the vector.</param>
        /// <returns>A new vector containing all zeros.</returns>
        public static Vector Zeros(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "Length cannot be negative.");
            return new Vector(new double[n]);
        }

        /// <summary>
        /// Creates a new vector of a specified length, filled with ones.
        /// </summary>
        /// <param name="n">The length of the vector.</param>
        /// <returns>A new vector containing all ones.</returns>
        public static Vector Ones(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "Length cannot be negative.");
            var data = new double[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = 1.0;
            }
            return new Vector(data);
        }

        #region Method Forms
        public Vector Add(Vector b) => this + b;
        public Vector Sub(Vector b) => this - b;
        public Vector Scale(double s) => this * s;

        /// <summary>
        /// Computes the dot product of this vector with another vector.
        /// Dot Product: a · b = Σ (a_i * b_i)
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>The dot product.</returns>
        public double Dot(Vector b)
        {
            if (Length != b.Length)
            {
                throw new ArgumentException("Vectors must have the same length for dot product.");
            }
            return _data.Select((val, i) => val * b[i]).Sum();
        }

        /// <summary>
        /// Computes the L2 norm (Euclidean norm) of the vector.
        /// L2 Norm: ||v||₂ = sqrt(Σ v_i²)
        /// </summary>
        /// <returns>The L2 norm.</returns>
        public double NormL2() => Math.Sqrt(this.Dot(this));

        /// <summary>
        /// Computes the L1 norm (Manhattan norm) of the vector.
        /// L1 Norm: ||v||₁ = Σ |v_i|
        /// </summary>
        /// <returns>The L1 norm.</returns>
        public double NormL1() => _data.Sum(Math.Abs);

        /// <summary>
        /// Returns a new vector with the same direction but a magnitude of 1 (a unit vector).
        /// </summary>
        /// <param name="eps">A small tolerance to avoid division by zero for a near-zero-magnitude vector.</param>
        /// <returns>The normalized vector.</returns>
        public Vector Normalize(double eps = 1e-12)
        {
            double norm = NormL2();
            if (Math.Abs(norm) < eps)
            {
                return Zeros(Length); // Return zero vector if magnitude is negligible
            }
            return this * (1.0 / norm);
        }

        /// <summary>
        /// Calculates the cosine similarity between this vector and another.
        /// Cosine Similarity: cos(θ) = (a · b) / (||a||₂ * ||b||₂)
        /// Reference: https://en.wikipedia.org/wiki/Cosine_similarity
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <param name="eps">A small tolerance to avoid division by zero.</param>
        /// <returns>The cosine similarity, a value between -1 and 1.</returns>
        public double CosineSimilarity(Vector b, double eps = 1e-12)
        {
            double dotProduct = this.Dot(b);
            double normA = this.NormL2();
            double normB = b.NormL2();
            double denominator = normA * normB;

            if (denominator < eps)
            {
                return 0.0; // Or handle as an undefined case, 0 is a common practice
            }

            return dotProduct / denominator;
        }
        #endregion

        #region Operator Overloads
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Vectors must have the same length for addition.");
            }
            var result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] + b[i];
            }
            return new Vector(result);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Vectors must have the same length for subtraction.");
            }
            var result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return new Vector(result);
        }

        public static Vector operator -(Vector a)
        {
            var result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = -a[i];
            }
            return new Vector(result);
        }

        public static Vector operator *(Vector a, double s)
        {
            var result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] * s;
            }
            return new Vector(result);
        }

        public static Vector operator *(double s, Vector a) => a * s;
        #endregion
    }
}