using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Controllers;
using StriveSteady.Data;
using StriveSteady.Models;

namespace StriveSteady.Tests;

public class UserTests
{
    private UsersController _controller;
    private StriveSteadyContext _context;


    [Fact]
    //Test that when a user registers, It is created in the database.
    public async Task Test_Register()
    {
        //User a in-memory database to not touch the actual database
        var options = new DbContextOptionsBuilder<StriveSteadyContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new StriveSteadyContext(options);
        _controller = new UsersController(_context);

        //remove everything from database
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var user = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com"
        };

     
        await _controller.Register(user);

       
        var userInDatabase = await _context.User.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(userInDatabase); // Assert that the user exists in the in-memory database
        
    }

    [Fact]
    public async Task Returns_User_From_Database_When_LogIn()
    {
        //User a in-memory database to not touch the actual database
        var options = new DbContextOptionsBuilder<StriveSteadyContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new StriveSteadyContext(options);
        _controller = new UsersController(_context);

        
        //mapper
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ActionResult<User>, User>());
        var mapper = config.CreateMapper();

        //create 3 users in database
        var user1 = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com"
        };

        var user2 = new User
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jackson",
            Email = "MikeJack@gmail.com"
        };

        var user3 = new User
        {
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com"
        };
        //Insert in database
        var result1 = await _controller.Register(user1);
        var result2 = await _controller.Register(user2);
        var result3 = await _controller.Register(user3);

        var userLoggedIn = _controller.LogIn(2);

        Assert.NotNull(userLoggedIn);
        Assert.Equivalent(user2, userLoggedIn.Result);

    }
}