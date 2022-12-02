//using Microsoft.AspNetCore.Identity;
//using Moq;
//using MuscleGain.Infrastructure.Data.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MuscleGain.Infrastructure.Data;
//using MuscleGain.Infrastructure.Data.Models.Account;
//using MuscleGain.Tests.Common;
//using MuscleGain.Tests.Mocks;

//namespace MuscleGain.Tests
//{
//    public class UnitTestBase
//    {
//        protected MuscleGainTestDb testDb;
//        private MuscleGainDbContext dbContext;
//        protected IRepository repo;
//        protected Mock<UserManager<ApplicationUser>> userManager;
//        //protected Mock<SignInManager<ApplicationUser>> signInManager;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            this.dbContext = DatabaseMock.Instance;
//            this.testDb = new MuscleGainTestDb(this.dbContext);
//            this.repo = new RepoMock(this.dbContext);
//            //this.userManager = UserManagerMock.MockUserManager(new List<ApplicationUser>
//            //{
//            //    this.testDb.GuestUser,
//            //    this.testDb.AdminUser,
//            //    this.testDb.AuthorUser
//            //});
//            //this.signInManager = SignInManagerMock.MockSignInManager();
//        }

//        [OneTimeTearDown]
//        public void OneTimeTearDown()
//        {
//            this.dbContext.Dispose();
//        }
//    }
//}
