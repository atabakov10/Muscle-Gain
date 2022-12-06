using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Models.Users;
using MuscleGain.Core.Services.User;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Tests.User
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public async Task GetUserByIdShouldReturnUserId()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabakov10",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = userService.GetUserById(user.Id);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetUsersShouldReturnAllUsers()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabakov10",
                Email = "atabakov@abv.bg",
                UserName = "tabakov10",
            };

            var userTwo = new ApplicationUser()
            {
                Id = "tabakov10Two",
                Email = "atabakov2@abv.bg",
                UserName = "tabakov10Two",
            };

            await dbContext.AddAsync(user);
            await dbContext.AddAsync(userTwo);
            await dbContext.SaveChangesAsync();


            Assert.NotNull(userService.GetUsers());
            Assert.That(userService.GetUsers().Result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserProfileShouldReturnUserProfile()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabakov10",
                Email = "atabakov@abv.bg",
                FirstName = "Angel",
                LastName = "Angelov"
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = await userService.GetUserProfile(user.Id);

            Assert.NotNull(result);
        }
        
        [Test]
        public async Task GetUserProfileShouldThrowNullReferenceException()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);
            
            Assert.That(
                async() => await userService.GetUserProfile("null"),
                Throws.Exception.TypeOf<NullReferenceException>(), "Id is invalid");
        }

        [Test]
        public async Task GetUserForEditShouldReturnEditView()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabakov10",
                FirstName = "Angel",
                LastName = "Angelov"
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = await userService.GetUserForEdit("tabakov10");

            Assert.NotNull(result);
        }

        [Test]
        public async Task UpdateUserShouldUpdateTheUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabakov10",
                FirstName = "Angel",
                LastName = "Angelov"
            };

            var userEdited = new UserEditViewModel()
            {
                Id = "tabakov10",
                FirstName = "AngelEdit",
                LastName = "Angelov"
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = await userService.UpdateUser(userEdited);

            Assert.NotNull(result);
            Assert.That("AngelEdit" ,Is.EqualTo(dbContext.Users.FirstOrDefaultAsync().Result.FirstName));
            Assert.That("Angelov", Is.EqualTo(dbContext.Users.FirstOrDefaultAsync().Result.LastName));
        }
        [Test]
        public async Task UpdateUserShouldThrowNullReferenceException()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var userService = new UserService(dbContext);
            
            var userEdited = new UserEditViewModel()
            {
                Id = "tabakov10",
                FirstName = "AngelEdit",
                LastName = "Angelov"
            };
            
            Assert.That(
                async() => await userService.UpdateUser(userEdited),
                Throws.Exception.TypeOf<NullReferenceException>(), "Invalid user");
        }
    }
}
