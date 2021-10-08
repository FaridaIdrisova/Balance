namespace MaterialBalanceUnitTests
{
    using System;
    using System.Linq;

    using Accord.Math;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MaterialBalance;

    [TestClass]
    public class BalanceUnitTests
    {
        [TestMethod]
        public void TestScript1()
        {
            double[,] A = new double[,] { 
                { 1, -1, -1, 0, 0, 0, 0, 0 }, 
                { 0, 0, 1, -1, -1, 0, 0, 0 }, 
                { 0, 0, 0, 0, 1, -1, -1, -1 } };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991, 6.667 };
            double[] t = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020, 0.667 };

            double[] expected = new double[] { 10.1530973758986, 2.97841637083808, 7.17468100506053, 1.97781650206667, 5.19686450299386, 3.96213896399276, 0.985540522793925, 0.24918501620718 };
            
            Solver solver = new Solver();
            solver.Solve(x0, A, t);

            double[] actual = solver.GetSolution();
            actual = actual.Select(x => Math.Round(x, 2)).ToArray();
            expected = expected.Select(x => Math.Round(x, 2)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestScript2()
        {
            Solver solver = new Solver();
            double[,] A = new double[,] { 
                { 1, -1, -1, 0, 0, 0, 0, 0 }, 
                { 0, 0, 1, -1, -1, 0, 0, 0 }, 
                { 0, 0, 0, 0, 1, -1, -1, -1 } };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991, 6.667 };
            double[] t = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020, 0.667 };

            solver.Solve(x0, A, t);
            double[] result = solver.GetSolution();
            var answer = Matrix.Dot(A, result).Sum() <= 0.01;

            Assert.AreEqual(true, answer);
        }

        [TestMethod]
        public void TestScript3()
        {
            double[,] A = new double[,] { 
                { 1, -1, -1, 0, 0, 0, 0 }, 
                { 0, 0, 1, -1, -1, 0, 0 }, 
                { 0, 0, 0, 0, 1, -1, -1 }, 
                { 1, -10, 0, 0, 0, 0, 0 } };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991 };
            double[] t = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020 };

            Solver solver = new Solver();
            solver.Solve(x0, A, t);
            double[] result = solver.GetSolution();
            bool answer = Matrix.Dot(A, result).Sum() <= 0.01;

            Assert.IsTrue(answer);
        }
    }
}
