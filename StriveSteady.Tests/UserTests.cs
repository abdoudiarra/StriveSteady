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
            Email = "JoTest@gmail.com",
            Password = "abcde",
            PasswordRepeat = "abcde"
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

        //remove everything from database
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        //mapper
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ActionResult<User>, User>());
        var mapper = config.CreateMapper();

        //create 3 users in database
        var user1 = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com",
            Password = "klmno",
            PasswordRepeat = "klmno"
        };

        var user2 = new User
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jackson",
            Email = "MikeJack@gmail.com",
            Password = "uvwxy",
            PasswordRepeat = "uvwxy"
        };

        var user3 = new User
        {
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "zabcd",
            PasswordRepeat = "zabcd"
        };
        //Insert in database
        var result1 = await _controller.Register(user1);
        var result2 = await _controller.Register(user2);
        var result3 = await _controller.Register(user3);

        var userLoggedIn = _controller.GetUserById(3);

        Assert.NotNull(userLoggedIn);
        Assert.Equivalent(user3.Id, userLoggedIn.Result.Id);

    }

    //Testing registration security
    [Fact]
    public async Task Registration_Fails_When_Required_Fields_Are_Null_Or_Empty()
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

        // Test with null or empty values for required fields
        var userWithEmptyValues = new User
        {
            Id = 7,
            FirstName = "",
            LastName = "",
            Email = "",
            Password = "",
            PasswordRepeat = ""
        };

        var userWithNullValues = new User
        {
            Id = 7,
            FirstName = null,
            LastName = null,
            Email = null,
            Password = null,
            PasswordRepeat = null,
        };

        // Test registration with users that have null or empty values
        async Task<User> RegisterAndReturnUserAsync(User u)
        {
            
            var result = await _controller.Register(u);
            return result.Value; // Assuming your controller returns ActionResult<User>
            
        }

        // Assert that exceptions are thrown when registering users with null or empty values
        var userException = await Assert.ThrowsAsync<NullReferenceException>(() => RegisterAndReturnUserAsync(userWithEmptyValues));
        Assert.Equal("User registration failed: Please fill in all required fields.", userException.Message);

    }

        //validate that both Password and PasswordRepeat are identical throw exception if not
    [Fact]
    public async Task Both_Password_Edentical()
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
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "zabcd",
            PasswordRepeat = "aabbcc"
        };


        // Test registration with users that have null or empty values
        async Task<User> RegisterAndReturnUserAsync(User u)
        {
            var result = await _controller.Register(u);
            return result.Value; // Assuming your controller returns ActionResult<User>
        }

        var userRegisterd = await Assert.ThrowsAsync<Exception>(() => RegisterAndReturnUserAsync(user));

        Assert.Equal("Password and Password repeat are not identical", userRegisterd.Message);

    }

    //validate that the password is at least 5 characters long

    [Fact]
    public async Task Password_Edentical_5_Characters_Minimum()
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
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "aa",
            PasswordRepeat = "aa"
        };


        // Test registration with users that have null or empty values
        async Task<User> RegisterAndReturnUserAsync(User u)
        {
            var result = await _controller.Register(u);
            return result.Value; // Assuming your controller returns ActionResult<User>
        }

        var userRegisterd = await Assert.ThrowsAsync<Exception>(() => RegisterAndReturnUserAsync(user));

        Assert.Equal("Password not long enough (minimum 5 characters)", userRegisterd.Message);

    }

    //validate that the FirstName and LastName are both minimum 3 characters long
    [Fact]
    public async Task FirstName_Lastname_Not_Long_Enough()
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
            Id = 3,
            FirstName = "Bn",
            LastName = "Jes",
            Email = "BronJames@gmail.com",
            Password = "aaash",
            PasswordRepeat = "aaash"
        };


        // Test registration with users that have null or empty values
        async Task<User> RegisterAndReturnUserAsync(User u)
        {
            var result = await _controller.Register(u);
            return result.Value; // Assuming your controller returns ActionResult<User>
        }

        var userRegisterd = await Assert.ThrowsAsync<Exception>(() => RegisterAndReturnUserAsync(user));

        Assert.Equal("First name or last name not long enough (minimum 3 characters)", userRegisterd.Message);

    }

    //Login test
    //User will Login using email and password.
    [Fact]
    public async Task Returns_Correct_User_In_Login()
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


        //mapper
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ActionResult<User>, User>());
        var mapper = config.CreateMapper();

        //create 3 users in database
        var user1 = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com",
            Password = "klmno",
            PasswordRepeat = "klmno"
        };

        var user2 = new User
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jackson",
            Email = "MikeJack@gmail.com",
            Password = "uvwxy",
            PasswordRepeat = "uvwxy"
        };

        var user3 = new User
        {
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "zabcd",
            PasswordRepeat = "zabcd"
        };

        //insert the users in the database
        await _controller.Register(user1);
        await _controller.Register(user2);
        await _controller.Register(user3);
        //connect as a user
        var userLoggedIn = await _controller.LogIn("MikeJack@gmail.com", "uvwxy");
        //assert user returned correctly
        Assert.Equivalent(user2.Id, userLoggedIn.Id);
    }

    
    [Fact]
    public async Task Returns_Bad_Password_For_User()
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


        //mapper
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ActionResult<User>, User>());
        var mapper = config.CreateMapper();

        //create 3 users in database
        var user1 = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com",
            Password = "klmno",
            PasswordRepeat = "klmno"
        };

        var user2 = new User
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jackson",
            Email = "MikeJack@gmail.com",
            Password = "uvwxy",
            PasswordRepeat = "uvwxy"
        };

        var user3 = new User
        {
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "zabcd",
            PasswordRepeat = "zabcd"
        };

        //insert the users in the database
        // Test registration with users that have null or empty values
        await _controller.Register(user1);
        await _controller.Register(user2);
        await _controller.Register(user3);

        async Task<User> LoginAndReturnUserAsync(string mail, string password)
        {
            var result = await _controller.LogIn(mail, password);
            return result; // Assuming your controller returns ActionResult<User>
        }

        //connect as a user
        var userLoggedIn = await Assert.ThrowsAsync<Exception>(() => LoginAndReturnUserAsync("MikeJack@gmail.com", "wrongPassword"));

        Assert.Equal("Incorrect password, try again", userLoggedIn.Message);
    }

    [Fact]
    public async Task Returns_User_Does_Not_Exist()
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


        //mapper
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ActionResult<User>, User>());
        var mapper = config.CreateMapper();

        //create 3 users in database
        var user1 = new User
        {
            Id = 1,
            FirstName = "Johnny",
            LastName = "Test",
            Email = "JoTest@gmail.com",
            Password = "klmno",
            PasswordRepeat = "klmno"
        };

        var user2 = new User
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jackson",
            Email = "MikeJack@gmail.com",
            Password = "uvwxy",
            PasswordRepeat = "uvwxy"
        };

        var user3 = new User
        {
            Id = 3,
            FirstName = "Bron",
            LastName = "James",
            Email = "BronJames@gmail.com",
            Password = "zabcd",
            PasswordRepeat = "zabcd"
        };
        //insert the users in the database
        // Test registration with users that have null or empty values

        await _controller.Register(user1);
        await _controller.Register(user2);
        await _controller.Register(user3);

        async Task<User> LoginAndReturnUserAsync(string mail, string password)
        {
            var result = await _controller.LogIn(mail, password);
            return result; // Assuming your controller returns ActionResult<User>
        }

        //connect as a user
        var userLoggedIn = await Assert.ThrowsAsync<Exception>(() => LoginAndReturnUserAsync("donotexist@yahoo.com","testmdp"));
       
        //assert user returned correctly
        Assert.Equal("User with mail donotexist@yahoo.com does not exist", userLoggedIn.Message);
    }

}