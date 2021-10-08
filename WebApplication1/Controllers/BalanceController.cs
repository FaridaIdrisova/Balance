namespace WebApplication1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using MaterialBalance;

    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        [HttpGet]
        public string Post(string data)
        {
            InitialData initialData = JsonConvert.DeserializeObject<InitialData>(data);
            Solver solver = new Solver();
            solver.SolveWithRestrictions(initialData.X0, initialData.A, initialData.T, initialData.I, initialData.Lower, initialData.Upper);

            double[] actual = solver.GetSolution();

            return JsonConvert.SerializeObject(actual);
        }

        /*[HttpGet]
        public string SolveWithRestrictions(string data)
        {
            InitialData initialData = JsonConvert.DeserializeObject<InitialData>(data);
            Solver solver = new Solver();
            solver.SolveWithRestrictions(initialData.X0, initialData.A, initialData.T, initialData.I, initialData.Lower, initialData.Upper);

            double[] actual = solver.GetSolution();

            return JsonConvert.SerializeObject(actual);
        }*/
    }
}
