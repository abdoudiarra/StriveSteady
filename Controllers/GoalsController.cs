using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Data;
using StriveSteady.Models;


namespace StriveSteady.Controllers
{
    public class GoalsController : Controller
    {
        private readonly StriveSteadyContext _context;

        public GoalsController(StriveSteadyContext context)
        {
            _context = context;
        }
        
    }
}