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
				Id = 1,
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
	


        //goal creation fail on invalid date
        //goal deletion success
        [Fact]
		public async Task Goal_Delete_Success()
		{
            var options = new DbContextOptionsBuilder<StriveSteadyContext>()
                       .UseInMemoryDatabase(databaseName: "GoalsTestDatabase")
                       .Options;

            _context = new StriveSteadyContext(options);
            _controller = new GoalsController(_context);


            var goal = new Goal
            {
                Id = 7,
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

			await _controller.Delete(1);

            var goalInDatabaseDelete = await _context.Goal.FirstOrDefaultAsync(g => g.Id == goal.Id);


            Assert.Null(goalInDatabaseDelete);


        }
		//goal deletion fail due to not found
		[Fact]
		public async Task Goal_Fail_Not_Found()
		{

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


            var goal = new Goal
            {
                Id = 7,
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

        }
		//goal get by id fail due to id not found
		[Fact]
		public async Task Goal_Get_Failed_Not_Found()
		{

		}
		//goal get all
		[Fact]
		public async Task Goal_Get_All()
		{

		}
		//goal get goal by name success
		[Fact]
		public async Task Goal_Get_Name_Success()
		{

		}
		//get goal by name fail
		[Fact]
		public async Task Goal_Get_Name_Failed_Not_Found()
		{

		}
	}
}

