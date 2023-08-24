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
    public async Task Test_Register()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        // Act
        var result = await _controller.Register(user);

        // Assert
        Assert.IsType<ActionResult<User>>(result);
        // Add more assertions as needed
        //test that it actually creates the user in the database
        var userInDatabase = await _context.User.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(userInDatabase); // Assert that the user exists in the in-memory database

        // You can add more assertions to check the user's properties if needed
        Assert.Equal(user.FirstName, userInDatabase.FirstName);
        Assert.Equal(user.LastName, userInDatabase.LastName);
        Assert.Equal(user.Email, userInDatabase.Email);
    }
}