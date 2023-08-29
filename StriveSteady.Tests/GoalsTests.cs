using System;
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

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

			var subtaskList = new List<Subtask>();

			var goalCreate = new Goal
			{
				Id = 51,
				Name = "Code",
				Description = "At least code once a day",
				ImportanceType = Enums.ImportanceType.HIGH_PRIORITY,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(5),
				GoalType = Enums.GoalType.Educational,
                Subtasks = subtaskList,
                IsChecked = false
            };


            await _controller.CreateGoal(goalCreate);


            var goalInDatabase = await _context.Goal.FirstOrDefaultAsync(g => g.Id == goalCreate.Id);
            Assert.NotNull(goalInDatabase);
        }

		//goal creation fail on required
		[Fact]
		public async Task Goal_Fail_On_Required()
		{

		}
		//goal deletion success
		[Fact]
		public async Task Delete_Success()
		{

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

