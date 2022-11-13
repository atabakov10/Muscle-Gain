//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MuscleGain.Contracts;
//using MuscleGain.Infrastructure.Data.Common;
//using MuscleGain.Infrastructure.Data;
//using MuscleGain.Infrastructure.Data.Models.Protein;

//namespace MuscleGain.Services.Seller
//{
//	public class SellerService : ISellerService
//	{
//        private readonly IRepository repo;

//        public SellerService(IRepository repo)
//        {
//            this.repo = repo;
//        }

//        public async Task Create(string userId, string phoneNumber)
//        {
//            var seller = new Infrastructure.Data.Models.Sellers.Seller
//            {
//                UserId = userId,
//                PhoneNumber = phoneNumber
//            };
//            await repo.AddAsync(seller);
//            await repo.SaveChangesAsync();
//        }
//        public async Task<bool> ExistsById(string userId)
//        {
//            return await repo.All<Infrastructure.Data.Models.Sellers.Seller>()
//                .AnyAsync(a => a.UserId == userId);
//        }

//        public async Task<int> GetSellerId(string userId)
//        {
//            return (await repo.AllReadonly<Infrastructure.Data.Models.Sellers.Seller>()
//                .FirstOrDefaultAsync(a => a.UserId == userId))?.Id ?? 0;
//        }
//        public Task<bool> UserWithPhoneNumberExists(string phoneNumber)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<bool> UserHasSells(string userId)
//        {
//            //return await repo.All<Protein>()
//            //    .AnyAsync(h => h.RenterId == userId);
//            return true;
//        }

//    }
//}
