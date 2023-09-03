using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using Moq;


namespace OFOS
{
    public class UserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public interface ofos
    {
        void AddUser(UserData userData);
    }

    public class UserManager
    {
        private ofos _database;

        public UserManager(ofos database)
        {
            _database = database;
        }

        public void CreateUser(UserData userData)
        {
            // Проверки на входните данни
            if (string.IsNullOrWhiteSpace(userData.Username) || string.IsNullOrWhiteSpace(userData.Email))
            {
                throw new ArgumentException("Username и Email са задължителни.");
            }

            _database.AddUser(userData);
        }
    }

    [TestFixture]
    public class UserManagerTests
    {
        [Test]
        public void TestCreateUserWithValidData()
        {
            var databaseMock = new Mock<ofos>();
            var userManager = new UserManager(databaseMock.Object);

            UserData validUserData = new UserData
            {
                Username = "user123",
                Email = "user@example.com"
            };

            userManager.CreateUser(validUserData);

            databaseMock.Verify(db => db.AddUser(It.IsAny<UserData>()), Times.Once);
        }

        [Test]
        public void TestCreateUserWithMissingData()
        {
            var databaseMock = new Mock<ofos>();
            var userManager = new UserManager(databaseMock.Object);

            UserData invalidUserData = new UserData
            {
                Username = "",
                Email = ""
            };

            Assert.Throws<ArgumentException>(() => userManager.CreateUser(invalidUserData));

            databaseMock.Verify(db => db.AddUser(It.IsAny<UserData>()), Times.Never);
        }
    }
}
