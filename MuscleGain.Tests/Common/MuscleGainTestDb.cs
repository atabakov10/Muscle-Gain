//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using MuscleGain.Infrastructure.Data;
//using MuscleGain.Infrastructure.Data.Models.Account;
//using MuscleGain.Infrastructure.Data.Models.Protein;
//using MuscleGain.Infrastructure.Data.Models.Quotes;
//using MuscleGain.Infrastructure.Data.Models.Reviews;

//namespace MuscleGain.Tests.Common
//{
//    public class MuscleGainTestDb
//    {
//        public MuscleGainTestDb(MuscleGainDbContext dbContext)
//        {
//            this.SeedDatabase(dbContext);
//        }

//        public ApplicationUser GuestUser { get; set; }

//        public ApplicationUser AdminUser { get; set; }

//        public ApplicationUser AuthorUser { get; set; }

//        public ApplicationUser SellerUser { get; set; }

//        public ProteinsCategories Category { get; set; }
//        public ProteinsCategories DeletedCategory { get; set; }


//        public Protein ProteinPage { get; set; }

//        public Protein DeletedProteinPage { get; set; }

//        public Quote Quotes { get; set; }

//        public Review GuestReview { get; set; }

//        //public Review ReviewWithDeletedProtein { get; set; }

//        private void SeedDatabase(MuscleGainDbContext dbContext)
//        {
//            UserOnlyStore<ApplicationUser, MuscleGainDbContext> userStore =
//                new UserOnlyStore<ApplicationUser, MuscleGainDbContext>(dbContext);
//            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
//            UpperInvariantLookupNormalizer normalizer = new UpperInvariantLookupNormalizer();
//            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
//                userStore,
//                null,
//                hasher,
//                null, null,
//                normalizer,
//                null, null, null);

//            this.GuestUser = new ApplicationUser()
//            {
//                Id = new string("1"),
//                UserName = $"guest{DateTime.Now.Ticks.ToString().Substring(10)}",
//                NormalizedUserName = $"GUEST{DateTime.Now.Ticks.ToString().Substring(10)}",
//                Email = "guest@mail.com",
//                NormalizedEmail = "GUEST@MAIL.COM",
//                FirstName = "Guest",
//                LastName = "Guestov",
//                ImageUrl = null,
//                PhoneNumber = null,
//                EmailConfirmed = true,
//            };

//            userManager.CreateAsync(this.GuestUser, "guestPassword123!")
//                .Wait();

//            this.AdminUser = new ApplicationUser()
//            {
//                Id = new string("2"),
//                UserName = $"Admin{DateTime.Now.Ticks.ToString().Substring(10)}",
//                NormalizedUserName = $"ADMIN{DateTime.Now.Ticks.ToString().Substring(10)}",
//                Email = "Admin@abv.bg",
//                NormalizedEmail = "ADMIN@ABV.BG",
//                FirstName = "Admin",
//                LastName = "Adminov",
//                ImageUrl = null,
//                PhoneNumber = null,
//                EmailConfirmed = false,
//            };

//            userManager.CreateAsync(this.AdminUser, "N!ce123Password")
//                .Wait();

//            this.AuthorUser = new ApplicationUser()
//            {
//                Id = new string("3"),
//                UserName = $"Author{DateTime.Now.Ticks.ToString().Substring(10)}",
//                NormalizedUserName = $"AUTHOR{DateTime.Now.Ticks.ToString().Substring(10)}",
//                Email = "author@abv.bg",
//                NormalizedEmail = "AUTHOR@MAIL.COM",
//                FirstName = "Author",
//                LastName = "Authorov",
//                ImageUrl = null,
//                PhoneNumber = null,
//                EmailConfirmed = true,
//            };

//            userManager.CreateAsync(this.AuthorUser, "Author123!")
//                .Wait();

//            this.SellerUser = new ApplicationUser()
//            {
//                Id = new string("4"),
//                UserName = $"Seller{DateTime.Now.Ticks.ToString().Substring(10)}",
//                NormalizedUserName = $"Seller{DateTime.Now.Ticks.ToString().Substring(10)}",
//                Email = "seller@abv.bg",
//                NormalizedEmail = "SELLER@ABV.BG",
//                FirstName = "Seller",
//                LastName = "Sellerov",
//                ImageUrl = null,
//                PhoneNumber = null,
//                EmailConfirmed = true,
//            };

//            userManager.CreateAsync(this.AuthorUser, "Author123!")
//                .Wait();

//            this.Category = new ProteinsCategories()
//            {
//                Id = new int(),
//                Name = "Vegan Protein",
//                IsDeleted = false
//            };
//            this.ProteinPage = new Protein()
//            {
//                Id = new int(),
//                Name = "Whey Protein 45", 
//                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
//                Grams = "500g",
//                Flavour = "Chocolate",
//                Price = new decimal(49.99),
//                Description = "This is a very nice protein!",
//                CategoryId = this.Category.Id,
//                ApplicationUserId = this.GuestUser.Id,
//                IsDeleted = false,
//                IsApproved = true,
//                OrderId = null
//            };

//            dbContext.Add<Protein>(this.ProteinPage);

//            this.Quotes = new Quote()
//            {
//                Id = new int(),
//                IsDeleted = false,
//                Text = "I'm so brave i cannot be stopped!",
//                AuthorName = "Angel Tabakov",
//                UserId = this.AuthorUser.Id,
//            };

//            dbContext.Add<Quote>(this.Quotes);

//            this.GuestReview = new Review()
//            {
//                Id = new int(),
//                Comment = "The best protein ever created!!!",
//                Rating = new int(),
//                DateOfPublication = DateTime.Now,
//                UserId = this.GuestUser.Id,
//                ProteinId = this.ProteinPage.Id,
//            };

//            dbContext.Add<Review>(this.GuestReview);

//            this.DeletedCategory = new ProteinsCategories()
//            {
//                Id = new int(),
//                Name = "Vegan deleted",
//                IsDeleted = true
//            };

//            dbContext.Add<ProteinsCategories>(this.DeletedCategory);

//            this.DeletedProteinPage = new Protein()
//            {
//                Id = new int(),
//                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
//                Name = "Deleted Protein",
//                Grams = "500g",
//                Flavour = "DeletedFlavour",
//                Price = new decimal(49.99),
//                Description = "This protein is deleted for testing purposes",
//                CategoryId = this.Category.Id,
//                ApplicationUserId = this.SellerUser.Id,
//                IsDeleted = true,
//                IsApproved = true,
//                OrderId = null
//            };

//            dbContext.Add<Protein>(this.DeletedProteinPage);

//            //this.ReviewWithDeletedProtein = new Review()
//            //{
//            //    Id = new int(),
//            //    Comment = "Existing comment with deleted protein page made by the GuestUser for testing purposes",
//            //    DateOfPublication = DateTime.Now.AddDays(-5),
//            //    UserId = this.GuestUser.Id,
//            //    ProteinId = this.DeletedProteinPage.Id,
//            //    IsDeleted = false,
//            //};
//            //dbContext.Add<RankPage>(this.DeletedPage);
//            //this.DeletedEntry = new RankEntry
//            //{
//            //    Id = Guid.NewGuid(),
//            //    Placement = 3,
//            //    Title = "DeletedEntry",
//            //    Image = null,
//            //    ImageAlt = "DeletedImg",
//            //    Description = "DeletedDescription",
//            //    IsDeleted = true,
//            //    RankPageId = this.GuestPage.Id,
//            //};

//            //dbContext.Add<RankEntry>(this.DeletedEntry);

//            //this.GuestEntry = new RankEntry
//            //{
//            //    Id = Guid.NewGuid(),
//            //    Placement = 2,
//            //    Title = "GuestEntry",
//            //    Image = null,
//            //    ImageAlt = "GuestImg",
//            //    Description = "GuestDescription",
//            //    IsDeleted = false,
//            //    RankPageId = this.GuestPage.Id,
//            //};

//            //dbContext.Add<RankEntry>(this.GuestEntry);

//            //this.GuestComment = new Comment
//            //{
//            //    Id = Guid.NewGuid(),
//            //    Content = "Comment made by the GuestUser for testing purposes",
//            //    CreatedOn = DateTime.Now.AddDays(-10),
//            //    CreatedByUserId = this.GuestUser.Id,
//            //    RankPageId = this.GuestPage.Id,
//            //    IsDeleted = false,
//            //};

//            //dbContext.Add<Comment>(this.GuestComment);

//            //this.CommentWithDeletedPage = new Comment
//            //{
//            //    Id = Guid.NewGuid(),
//            //    Content = "Existing comment with deleted page made by the GuestUser for testing purposes",
//            //    CreatedOn = DateTime.Now.AddDays(-5),
//            //    CreatedByUserId = this.GuestUser.Id,
//            //    RankPageId = this.DeletedPage.Id,
//            //    IsDeleted = false,
//            //};

//            //dbContext.Add<Comment>(this.CommentWithDeletedPage);

//            //this.LikedMap = new EasyRankUserRankPage
//            //{
//            //    EasyRankUserId = this.LikedUser.Id,
//            //    RankPageId = this.LikedPage.Id,
//            //    IsLiked = true,
//            //};

//            //dbContext.Add<EasyRankUserRankPage>(this.LikedMap);

//            //this.DislikedMap = new EasyRankUserRankPage
//            //{
//            //    EasyRankUserId = this.DislikedUser.Id,
//            //    RankPageId = this.DislikedPage.Id,
//            //    IsLiked = false,
//            //};

//            //dbContext.Add<EasyRankUserRankPage>(this.DislikedMap);

//            dbContext.SaveChanges();
//        }

//    }
//}
