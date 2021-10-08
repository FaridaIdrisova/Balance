namespace WebService2
{
    public class InitialData
    {
        public InitialData()
        {
        }

        public InitialData(double[,]a, double[]x0, double[]t)
        {
            this.A = a;
            this.X0 = x0;
            this.T = t;
        }

        public double[,] A { get; set; }

        public double[] X0 { get; set; }

        public double[] T { get; set; }

        public double[] I { get; set; }

        public double[] Lower { get; set; }

        public double[] Upper { get; set; }
    }
}
