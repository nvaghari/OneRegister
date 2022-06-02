using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.JobFactory
{
    public class MCTasksJobService
    {
        private readonly IEnumerable<IMasterCardJob> _jobs;

        public MCTasksJobService(IEnumerable<IMasterCardJob> jobs)
        {
            _jobs = jobs;
        }
        public void Run()
        {
            _jobs.ToList().ForEach(j => j.Execute());
        }
    }
}
