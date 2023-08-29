using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Data;
using StriveSteady.Enums;
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

        //Create Goal
        [HttpPost("Goal/Create")]
        public async Task<Goal> CreateGoal(Goal goal)
        {

            if (goal.Name == null || goal.Name.Length < 4)
            {
                throw new Exception("Invalid name (has to be at least 4 characters long)");
            }

            var goalCreate = new Goal
            {
                Name = goal.Name,
                Description = goal.Description,
                ImportanceType = goal.ImportanceType,
                StartDate = goal.StartDate,
                EndDate = goal.EndDate,
                GoalType = goal.GoalType,
                Subtasks = goal.Subtasks,
                IsChecked = goal.IsChecked
            };

            _context.Goal.Add(goalCreate);
            await _context.SaveChangesAsync();

            return goalCreate;

        }

        //Delete Goal
        [HttpDelete("Goal/Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var goal = await GetGoalById(id);
            if(goal == null)
            {
                throw new Exception("Goal does not exist");
            }
            _context.Goal.Remove(goal);
            _context.SaveChanges();

            return Ok(goal);
        }
        
        //Get GoalById
        [HttpGet("Goal/GetGoal/{id}")]
        public async Task<Goal> GetGoalById(int id)
        {
            var goal = await _context.Goal.FirstOrDefaultAsync(g => g.Id == id);

            if (goal == null)
            {

                throw new Exception("This goal does not exist");
            }
            return goal;
        }

        //Get AllGoals
        [HttpGet("Goals/GetAllGoals")]
        public async Task<List<Goal>> GetAllGoals()
        {
            var goalsList = new List<Goal>();
            foreach(var goal in _context.Goal)
            {
                goalsList.Add(goal);
            }

            return goalsList;
        }

        //Get GoalByName
        [HttpGet("Goal/GetGoal/{name}")]
        public async Task<Goal> GetGoalByName(string name)
        {
            if (name == null)
            {
                throw new NullReferenceException("No name for goal");
            }

            var goal = await _context.Goal.FirstOrDefaultAsync(g => g.Name == name);

            if(goal == null)
            {

                throw new Exception("This goal does not exist");
            }
            return goal;
        }
        
    }
}