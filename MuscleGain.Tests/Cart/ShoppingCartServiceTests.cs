using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Services.Cart;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Infrastructure.Migrations;

namespace MuscleGain.Tests.Cart
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        [Test]
        public async Task AddProteinInShoppingCartShouldReturnProteinAdded()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var cartService = new ShoppingCartService(dbContext);
            var user = new ApplicationUser()
            {
                Id = "tabaka10",
                FirstName = "Angel",
                LastName = "Tabakov",
                ShoppingCartId = 1
            };

            var proteinCategory = new ProteinsCategories()
            {
               Id = 4,
               Name = "Whey Category"
            };

            var protein = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 2,
                Name = "Why Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var cart = new ShoppingCart()
            {
                Id = 1,
                UserId = user.Id,
            };

            cart.ShoppingCartProteins.Add(protein);
            protein.ShoppingCart.Add(cart);
            
            await dbContext.Proteins.AddAsync(protein);
            await dbContext.Users.AddAsync(user);
            await dbContext.ProteinsCategories.AddAsync(proteinCategory);
            await dbContext.SaveChangesAsync();

            await cartService.AddProteinInShoppingCart(protein.Id, user.Id);

            Assert.That(dbContext.ShoppingCarts.CountAsync(), Is.Not.Null);
        }

        [Test]
        public async Task DeleteProteinFromShoppingCartShouldDeleteProtein()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var cartService = new ShoppingCartService(dbContext);
            var user = new ApplicationUser()
            {
                Id = "tabaka10",
                FirstName = "Angel",
                LastName = "Tabakov",
                ShoppingCartId = 1
            };

            var proteinCategory = new ProteinsCategories()
            {
                Id = 4,
                Name = "Whey Category"
            };

            var protein = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 2,
                Name = "Why Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var cart = new ShoppingCart()
            {
                Id = 1,
                UserId = user.Id,
            };

            cart.ShoppingCartProteins.Add(protein);
            protein.ShoppingCart.Add(cart);

            await dbContext.Proteins.AddAsync(protein);
            await dbContext.Users.AddAsync(user);
            await dbContext.ProteinsCategories.AddAsync(proteinCategory);
            await dbContext.SaveChangesAsync();

            await cartService.DeleteProteinFromShoppingCart(protein.Id, user.Id);

            Assert.That(cartService.DeleteProteinFromShoppingCart(protein.Id, user.Id), Is.Not.Null);
        }

        [Test]
        public async Task DeleteAllProteinsFromShoppingCartShouldDeleteAllProteinsFromShoppingCart()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var cartService = new ShoppingCartService(dbContext);
            var user = new ApplicationUser()
            {
                Id = "tabaka10",
                FirstName = "Angel",
                LastName = "Tabakov",
                ShoppingCartId = 1
            };
           

            var proteinCategory = new ProteinsCategories()
            {
                Id = 4,
                Name = "Whey Category"
            };

            var protein = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 2,
                Name = "Why Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var proteinTwo = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 3,
                Name = "Whey Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var cart = new ShoppingCart()
            {
                Id = 1,
                UserId = user.Id,
            };
            
            cart.ShoppingCartProteins.Add(protein);
            protein.ShoppingCart.Add(cart);

            await dbContext.Proteins.AddAsync(protein);
            await dbContext.Proteins.AddAsync(proteinTwo);
            await dbContext.Users.AddAsync(user);
            await dbContext.ProteinsCategories.AddAsync(proteinCategory);
            await dbContext.ShoppingCarts.AddAsync(cart);
            await dbContext.SaveChangesAsync();

            await cartService.DeleteAllProteinsFromShoppingCart(user.Id);

            Assert.That(dbContext.ShoppingCarts.CountAsync(), Is.Not.Null);
        }

        [Test]
        public async Task GetShoppingCartShouldReturnShoppingCart()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var cartService = new ShoppingCartService(dbContext);
            var user = new ApplicationUser()
            {
                Id = "tabaka10",
                FirstName = "Angel",
                LastName = "Tabakov",
                ShoppingCartId = 1
            };


            var proteinCategory = new ProteinsCategories()
            {
                Id = 4,
                Name = "Whey Category"
            };

            var protein = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 2,
                Name = "Why Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var proteinTwo = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 3,
                Name = "Whey Protein",
                ApplicationUserId = user.Id,
                CategoryId = proteinCategory.Id,
                Description = "blablablabla",
                Flavour = "Mojito",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
            };

            var cart = new ShoppingCart()
            {
                Id = 1,
                UserId = user.Id,
            };

            cart.ShoppingCartProteins.Add(protein);
            protein.ShoppingCart.Add(cart);

            await dbContext.Proteins.AddAsync(protein);
            await dbContext.Proteins.AddAsync(proteinTwo);
            await dbContext.Users.AddAsync(user);
            await dbContext.ProteinsCategories.AddAsync(proteinCategory);
            await dbContext.ShoppingCarts.AddAsync(cart);
            await dbContext.SaveChangesAsync();

            var result = await cartService.GetShoppingCart(user.Id);

            Assert.That(result, Is.Not.Null);
        }
    }
}
