using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StriveSteady.Controllers
{
    public class GoalsController : Controller
    {
        private readonly StriveSteadyContext _context;

        public GoalsController(StriveSteadyContext _context)
        {
            _context = context;
        }
        
    }
}