namespace WebService2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using MaterialBalance;

    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        [HttpGet]
        public string GetSolution(string data)
        {
            InitialData initialData = JsonConvert.DeserializeObject<InitialData>(data);

            Solver solver = new Solver();

            if (initialData.I.Length == 0)
            {
                solver.Solve(initialData.X0, initialData.A, initialData.T);
            }
            else
            {
                solver.SolveWithRestrictions(
                initialData.X0,
                initialData.A,
                initialData.T,
                initialData.I,
                initialData.Lower,
                initialData.Upper);
            }

            double[] actual = solver.GetSolution();

            return JsonConvert.SerializeObject(actual);
        }
    }
}
