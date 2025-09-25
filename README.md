[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/2UnD-f4K)
[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=20463262&assignment_repo_type=AssignmentRepo)
# Assignment 1  – ML Math Core Basics (C#)

**Goal:** Build the foundational math structures (vectors, matrices, and a couple of basic statistics functions) for machine learning
**Collaboration:** Discuss concepts, but all code must be your own. Cite any references used for formulas.

---

## Learning Objectives
By the end of this assignment, you will be able to:
1. Implement vector and matrix data structures with **operator overloading** in C# using classes.
2. Write correct, well-documented implementations of basic computations used in ML.
3. Explain the mathematical meaning of each computation in clear prose.
4. Validate results with a comprehensive, reproducible unit test suite.

---

## Scope (What You Will Build)
Create a .NET solution named **`MLMathSolution.sln`** with two projects:
- **`MLMath`** (class library)
  - `MLMath.Core`
    - `Vector` (immutable, 1‑D **class**)
    - `Matrix` (immutable, 2‑D, row‑major **class**)
  - `MLMath.Stats`
    - `Descriptive` (mean and variance)
- **`MLMath.Tests`** (xUnit or NUnit)

> **Constraint:** Do not use external math libraries (e.g., MathNet). Only the .NET Base Class Library.

---

## Required API & Formal Definitions

### Vector (class)
```csharp
namespace MLMath.Core;

/// <summary>Immutable dense 1‑D vector of doubles.</summary>
public sealed class Vector
{
    // define your data storage    
    
    // return length of data
    public int Length();
    public double this[int i] // implement indexer

    public Vector(double[] data)
    {
       // Do not forget to ensure data is in the actual array
    }

    public static Vector Zeros(int n)  // return a vector of size n containing all zeros
    public static Vector Ones(int n) // return a vector of size n containing all ones
    
    // Method forms
    public Vector Add(Vector b) 
    public Vector Sub(Vector b) 
    public Vector Scale(double s) 
    public double Dot(Vector b) 
    public double NormL2() 
    public double NormL1() 
    public Vector Normalize(double eps = 1e-12)
    public double CosineSimilarity(Vector b, double eps = 1e-12) 

    // Operator overloads
    public static Vector operator +(Vector a, Vector b) 
    public static Vector operator -(Vector a, Vector b) 
    public static Vector operator -(Vector a) 
    public static Vector operator *(Vector a, double s)  
    public static Vector operator *(double s, Vector a) 
}
```

**Examples:**
```csharp
var a = new Vector(new double[]{1, 2, 3});
var b = new Vector(new double[]{4, 5, 6});
var c = a + b;            // [5, 7, 9]
var d = a - b;            // [-3, -3, -3]
var neg = -a;             // [-1, -2, -3]
var scaled = 2.0 * a;     // [2, 4, 6]
var dot = a.Dot(b);       // 32 (1*4 + 2*5 + 3*6)
var l2 = a.NormL2();      // sqrt(14)
var l1 = a.NormL1();      // 6
var unit = a.Normalize(); // length ≈ 1
```

---

### Matrix (class)
```csharp
namespace MLMath.Core;

/// <summary>Immutable dense 2‑D row‑major matrix of doubles.</summary>
/// Data is stored in row major order.  So you must map the single array into a matrix, 
/// with the size of rows and columns 
/// So, [1,2,3,4,5,6,7,8,9] and a 3x3 matrix would become 3 rows, [1,2,3] [4,5,6], [7,8,9]
public sealed class Matrix
{
    // define your data as a single array of numbers.  That's how you will store it.
    

    public double this[int r, int c] =>//return the data at row, col

    public Matrix(int rows, int cols, double[] data)
    {
        // make sure the parameters are valid, and data is sized correctly to be treated as a matrix
        
    }

    public static Matrix Zeros(int r, int c)  // return a matrix of all zeros
    public static Matrix Ones(int r, int c) // return a matrix of all ones

    
    public static Matrix Identity(int n) // return the identity matrix  
    
    // Method forms
    public Matrix Add(Matrix b) 
    public Matrix Sub(Matrix b)     
    public Matrix Transpose()
    public Vector MatVec(Vector x) 
    public Matrix MatMul(Matrix b) 
    
    // Operator overloads
    public static Matrix operator +(Matrix a, Matrix b)
    public static Matrix operator -(Matrix a, Matrix b)
    public static Matrix operator -(Matrix a) 
    public static Matrix operator *(Matrix a, double s)
    public static Matrix operator *(double s, Matrix a) 
    public static Vector operator *(Matrix a, Vector x) 
    public static Matrix operator *(Matrix a, Matrix b) 
}
```
**Examples:**
```csharp
var A = new Matrix(2, 3, new double[]{ 1, 2, 3, 4, 5, 6 });
var B = new Matrix(3, 2, new double[]{ 7, 8, 9, 10, 11, 12 });
var C = A * B; // 2x2 => [ [58, 64], [139, 154] ]
var I = Matrix.Identity(3);
var AT = A.Transpose(); // 3x2
var y = A * new Vector(new double[]{ 7, 8, 9 }); // [ 50, 122 ]
```


### Descriptive (class)

```csharp
// <summary>
/// Descriptive statistics on vectors.
/// Math (1-based): μ = (1/n) Σ_i x_i;
/// Population variance: σ² = (1/n) Σ_i (x_i − μ)²;
/// Sample (unbiased) variance: s² = (1/(n−1)) Σ_i (x_i − μ)².
/// </summary>
public static class Descriptive
{
    public const double Epsilon = 1e-12;

    public static double Mean(Vector x)
    
    public static double Variance(Vector x, bool unbiased = true)
    
    public static double StdDev(Vector x, bool unbiased = true)
}
```

**Examples:**
```csharp


var x = new Vector(new double[] { 1, 2, 3 });
double mu = Descriptive.Mean(x);                      // 2.0
double varPop = Descriptive.Variance(x, unbiased:false); // 2.0/3.0 ≈ 0.6666667
double varSmp = Descriptive.Variance(x, unbiased:true);  // 1.0
double sdPop = Descriptive.StdDev(x, unbiased:false);    // sqrt(2/3)
```
---


## Operator Overloading Examples (End‑to‑End)
```csharp
var a = new Vector(new double[]{1,2,3});
var b = new Vector(new double[]{4,5,6});
var sum = a + b;             // [5,7,9]
var diff = a - b;            // [-3,-3,-3]
var scaled = 3 * a;          // [3,6,9]
var dot = a.Dot(b);          // 32
var A = new Matrix(2,2,new double[]{1,2,3,4});
var B = new Matrix(2,2,new double[]{5,6,7,8});
var C = A + B;               // [6,8,10,12]
var D = A * B;               // [19,22,43,50]
var x = new Vector(new double[]{10,20});
var y = A * x;               // [50, 110]
```

---

```csharp
// <summary>
/// Descriptive statistics on vectors.
/// Math (1-based): μ = (1/n) Σ_i x_i;
/// Population variance: σ² = (1/n) Σ_i (x_i − μ)²;
/// Sample (unbiased) variance: s² = (1/(n−1)) Σ_i (x_i − μ)².
/// </summary>
public static class Descriptive
{
    public const double Epsilon = 1e-12;

    public static double Mean(Vector x)
    
    public static double Variance(Vector x, bool unbiased = true)
    
    public static double StdDev(Vector x, bool unbiased = true)}

```

## Testing Requirements (What must be tested)
Create **at least 12 tests** (xUnit or NUnit). Organize logically (e.g., `VectorTests`, `MatrixTests`, `StatsTests`). Use **negative tests** for invalid shapes/lengths.

### Vector (minimum 5 tests)
1. **Addition/Subtraction correctness**: `a + b` and `a − b` match element‑wise expectations.
2. **Scalar multiply (both sides)**: `a * s` and `s * a` equal.
3. **Dot product**: compare to hand‑computed value; check commutativity `a.Dot(b) == b.Dot(a)`.
4. **Norms/Normalize**: `a.Normalize().NormL2() ≈ 1` (unless `a` is zero‑vector). L1 and L2 vs known results.
5. **Negative/Edge**: length mismatch throws `ArgumentException`; zero‑vector normalize does not divide by zero.

### Matrix (minimum 5 tests)
1. **Matrix–matrix multiply**: multiply small known matrices → expected result.
2. **Matrix–vector multiply**: `A * x` equals manual computation.
3. **Identity properties**: `A * I == A` and `I * A == A` when shapes allow.
4. **Transpose**: `(Aᵀ)ᵀ == A`; element checks.
5. **Negative/Edge**: shape mismatches throw (addition/subtraction; matmul inner‑dim mismatch).

### Statistics (minimum 2 tests)
1. **Mean**: simple vectors (e.g., `[1,2,3] → 2`).
2. **Variance**: population vs sample for `[1,2,3]` → `2/3` and `1` respectively.

> **Precision guidance:** Use tolerances like `1e-12` for floating‑point comparisons.

---

## Deliverables
1. **Code:** `MLMath` class library implementing required API with operator overloads using **classes** (not records). Immutability via defensive copies.
2. **Tests:** `MLMath.Tests` meeting the above coverage and negative cases.
4. **Solution File:** `MLMathSolution.sln` that includes both projects.

---

## Grading Rubric (100 pts)
- **Correctness (75 pts):** All computations match definitions; stable normalization; correct matmul.
- **Testing (15 pts):** ≥12 tests with positives and negatives; tolerances used appropriately.
- **Design & Style (10 pts):** Clean operator overloads; immutability; clear XML docs; argument validation.


---

## Submission
- Push to your repository

