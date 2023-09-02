using System;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Controllers;
using StriveSteady.Data;
using StriveSteady.Models;

namespace StriveSteady.Tests
{
	public class GoalsTests
	{
		private GoalsController _controller;
		private StriveSteadyContext _context;

		[Fact]
		public async Task Goal_Creation_Success()
		{
			var options = new DbContextOptionsBuilder<StriveSteadyContext>()
		   .UseInMemoryDatabase(databaseName: "GoalsTestDatabase")
		   .Options;

			_context = new StriveSteadyContext(options);
			_controller = new GoalsController(_context);


			var goal = new Goal
			{
				Id = 77,
				Name = "Code",
				Description = "At least code once a day",
				ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(5),
				GoalType = Enums.GoalType.Educational,
				Subtasks = new List<Subtask>(),
				IsChecked = false
			};

			await _controller.CreateGoal(goal);


			var goalInDatabase = await _context.Goal.FirstOrDefaultAsync(g => g.Id == goal.Id);

			Assert.NotNull(goal);
			Assert.NotNull(goalInDatabase);
			Assert.Equal(goalInDatabase.Id, goal.Id);
			Assert.Equal(goalInDatabase.Description, goal.Description);
		}

		//goal creation fail on required
		[Fact]
		public async Task Goal_Fail_On_Required()
		{
			var options = new DbContextOptionsBuilder<StriveSteadyContext>()
		   .UseInMemoryDatabase(databaseName: "GoalsTestDatabase")
		   .Options;

			_context = new StriveSteadyContext(options);
			_controller = new GoalsController(_context);


			var goal = new Goal
			{
				Id = 1,
				Name = "",
				Description = "At least code once a day",
				ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(5),
				GoalType = Enums.GoalType.Educational,
				Subtasks = new List<Subtask>(),
				IsChecked = false
			};


			async Task<Goal> CreateAndReturnGoalAsync(Goal g)
			{

				var result = await _controller.CreateGoal(g);
				return result;

			}

			var goalException = await Assert.ThrowsAsync<NullReferenceException>(() => CreateAndReturnGoalAsync(goal));
			Assert.Equal("Invalid name (has to be at least 4 characters long)", goalException.Message);

		}
	


        
        //goal get by id success
        [Fact]
		public async Task Goal_Get_Success()
		{

            var options = new DbContextOptionsBuilder<StriveSteadyContext>()
                       .UseInMemoryDatabase(databaseName: "GoalsTestDatabase")
                       .Options;

            _context = new StriveSteadyContext(options);
            _controller = new GoalsController(_context);


            var goal8 = new Goal
            {
                Id = 8,
                Name = "Code8",
                Description = "At least code once a day8",
                ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                GoalType = Enums.GoalType.Educational,
                Subtasks = new List<Subtask>(),
                IsChecked = false
            };
			var goal9 = new Goal
            {
                Id = 9,
                Name = "Code9",
                Description = "At least code once a day9",
                ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                GoalType = Enums.GoalType.Educational,
                Subtasks = new List<Subtask>(),
                IsChecked = false
            };

			await _controller.CreateGoal(goal8);
			await _controller.CreateGoal(goal9);


            var goalById = await _controller.GetGoalById(8);

			Assert.Equal(goalById.Id, goal8.Id);

        }
	
	
		//goal get goal by name success
		[Fact]
		public async Task Goal_Get_Name_Success()
		{
            var options = new DbContextOptionsBuilder<StriveSteadyContext>()
                       .UseInMemoryDatabase(databaseName: "GoalsTestDatabase")
                       .Options;

            _context = new StriveSteadyContext(options);
            _controller = new GoalsController(_context);


            var goal10 = new Goal
            {
                Id = 10,
                Name = "Code10",
                Description = "At least code once a day10",
                ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                GoalType = Enums.GoalType.Educational,
                Subtasks = new List<Subtask>(),
                IsChecked = false
            };
            var goal11 = new Goal
            {
                Id = 11,
                Name = "Code11",
                Description = "At least code once a day11",
                ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                GoalType = Enums.GoalType.Educational,
                Subtasks = new List<Subtask>(),
                IsChecked = false
            };

            await _controller.CreateGoal(goal10);
            await _controller.CreateGoal(goal11);


            var goalById = await _controller.GetGoalByName("Code10");

            Assert.Equal(goalById.Id, goal10.Id);
        }
		
	}
}

