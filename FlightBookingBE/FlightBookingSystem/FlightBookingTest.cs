using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.User;
using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FlightBookingSystem.Tests
{
    [TestFixture]
    public class UserServiceDbTests
    {
        private UserServiceDb _userService;
        private Mock<ILogger<UserServiceDb>> _loggerMock;
        private FlightContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<FlightContext>()
                .UseInMemoryDatabase(databaseName: "FlightBookingSystem")
                .Options;
            _context = new FlightContext(options);
            _loggerMock = new Mock<ILogger<UserServiceDb>>();
            _userService = new UserServiceDb(_context, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            _context.Users.Add(
                new UserEntity
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNo = "1234567890",
                    Role = "User",
                    Password = "hashedpassword",
                }
            );
            _context.Users.Add(
                new UserEntity
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@example.com",
                    PhoneNo = "0987654321",
                    Role = "Admin",
                    Password = "hashedpassword",
                }
            );
            await _context.SaveChangesAsync();

            var result = await _userService.GetAllUsers();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Role = "User",
                Password = "hashedpassword",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userService.GetUserById(user.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.FirstName, result.FirstName);
        }

        [Test]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _userService.GetUserById(999);

            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateUser_ShouldAddUser()
        {
            var user = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                PhoneNo = "1234567890",
                Role = "User",
            };

            await _userService.CreateUser(user);
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            Assert.IsNotNull(createdUser);
            Assert.AreEqual(user.FirstName, createdUser.FirstName);
        }

        [Test]
        public async Task UpdateUser_ShouldModifyUser()
        {
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Role = "User",
                Password = "hashedpassword",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var updateUser = new UpdateDTO
            {
                FirstName = "Johnny",
                LastName = "Doe",
                Email = "johnny.doe@example.com",
                PhoneNo = "1234567890",
            };

            await _userService.UpdateUser(user.UserId, updateUser);
            var updatedUser = await _context.Users.FindAsync(user.UserId);
            Assert.AreEqual(updateUser.FirstName, updatedUser.FirstName);
        }

        [Test]
        public async Task DeleteUser_ShouldRemoveUser()
        {
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Role = "User",
                Password = "hashedpassword",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await _userService.DeleteUser(user.UserId);
            var deletedUser = await _context.Users.FindAsync(user.UserId);
            Assert.IsNull(deletedUser);
        }

        [Test]
        public async Task LogIn_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = hashedPassword,
                PhoneNo = "1234567890",
                Role = "User",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var loginDto = new LogInDTO { Email = user.Email, Password = password };

            var result = await _userService.LogIn(loginDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public async Task LogIn_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LogInDTO { Email = "invalid@example.com", Password = "invalid" };

            // Act
            var result = await _userService.LogIn(loginDto);

            Assert.IsNull(result);
        }

        [Test]
        public async Task ResetPassword_ShouldUpdatePassword()
        {
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("oldpassword"),
                PhoneNo = "1234567890",
                Role = "User",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var newPassword = "newpassword";

            var result = await _userService.ResetPassword(user.Email, newPassword);
            var updatedUser = await _context.Users.FindAsync(user.UserId);

            Assert.IsTrue(result);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(newPassword, updatedUser.Password));
        }

        [Test]
        public async Task ValidateEmail_ShouldReturnTrue_WhenEmailExists()
        {
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Role = "User",
                Password = "hashedpassword",
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var result = await _userService.ValidateEmail(user.Email);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ValidateEmail_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            var result = await _userService.ValidateEmail("nonexistent@example.com");
            Assert.IsFalse(result);
        }
    }
}
