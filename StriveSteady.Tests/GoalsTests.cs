using System;
using StriveSteady.Controllers;
using StriveSteady.Data;

namespace StriveSteady.StriveSteady.Tests
{
	public class GoalsTests
	{
		private GoalsController _controller;
		private StriveSteadyContext _context;

		[Fact]
		public async Task Goal_Creation_Success()
		{

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

