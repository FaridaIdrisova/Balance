namespace MaterialBalance
{
    using Accord.Math;
    using Accord.Math.Optimization;
    using System;
    using System.Collections.Generic;

    public class Solver
    {
        private GoldfarbIdnani qp;

        private double[] balance;
        
        public bool SolutionFound { get; private set; }

        public double? DisbalanceOriginal { get; private set; }
        
        public double? Disbalance { get; private set; }

        public Solver()
        {
        }

        public void Solve(double[] x0, double[,] A, double[] t)
        {
            double[] b = Vector.Create(A.GetLength(0), 0.0);

            double[] I = Vector.Create(A.GetLength(1), 1.0);
            double[,] diagI = Matrix.Diagonal(I);

            double[] T = Elementwise.Pow(t, -2);
            double[,] diagW = Matrix.Diagonal(T);

            double[,] Q = Matrix.Dot(diagI, diagW);
            double[,] tempQ = Elementwise.Multiply(Q, -1);
            double[] d = Matrix.Dot(tempQ, x0);

            qp = new GoldfarbIdnani(Q, d, A, b, b.Length);
            qp.Minimize();

            balance = Matrix.Dot(A, qp.Solution);
        }

        public void SolveWithRestrictions(double[] x0, double[,] A, double[] t, double[] I, double[] lower, double[] upper)
        {
            double[,] diagI = Matrix.Diagonal(I);
            double[,] diagW = Matrix.Diagonal(1.Divide(t.Pow(2)));

            double[,] H = diagI.Dot(diagW);
            double[] d = H.Dot(x0).Multiply(-1);

            QuadraticObjectiveFunction func = new QuadraticObjectiveFunction(H, d);

            List<LinearConstraint> constraints = new List<LinearConstraint>();

            double[] b = Vector.Create(A.GetLength(0), 0.0);
            // Нижние и верхние границы
            for (int j = 0; j < x0.Length; j++)
            {
                constraints.Add(new LinearConstraint(1)
                {
                    VariablesAtIndices = new[] { j },
                    ShouldBe = ConstraintType.GreaterThanOrEqualTo,
                    Value = lower[j]
                });

                constraints.Add(new LinearConstraint(1)
                {
                    VariablesAtIndices = new[] { j },
                    ShouldBe = ConstraintType.LesserThanOrEqualTo,
                    Value = upper[j]
                });
            }

            // Ограничения для решения задачи баланса
            for (var j = 0; j < b.Length; j++)
            {
                var notNullElements = Array.FindAll(A.GetRow(j), x => Math.Abs(x) > 0);
                var notNullElementsIndexes = new List<int>();
                for (var k = 0; k < x0.Length; k++)
                {
                    if (Math.Abs(A[j, k]) > 0)
                    {
                        notNullElementsIndexes.Add(k);
                    }
                }

                constraints.Add(new LinearConstraint(notNullElements.Length)
                {
                    VariablesAtIndices = notNullElementsIndexes.ToArray(),
                    CombinedAs = notNullElements,
                    ShouldBe = ConstraintType.EqualTo,
                    Value = b[j]
                });
            }

            qp = new GoldfarbIdnani(func, constraints);

            SolutionFound = qp.Minimize();

            if (SolutionFound)
            {
                DisbalanceOriginal = A.Dot(x0).Subtract(b).Euclidean();
                Disbalance = A.Dot(qp.Solution).Subtract(b).Euclidean();
            }
            else
            {
                DisbalanceOriginal = null;
                Disbalance = null;
            }
        }

        public double[] GetBalance()
        {
            if (balance != null)
            {
                return balance;
            }
            return null;
        }

        public double[] GetSolution()
        {
            if (qp != null)
            {
                return qp.Solution;
            }
            return null;
        }
    }
}
