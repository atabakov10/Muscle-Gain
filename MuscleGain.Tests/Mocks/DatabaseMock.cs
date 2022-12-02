using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;

namespace MuscleGain.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static MuscleGainDbContext Instance
        {
            get
            {
                DbContextOptionsBuilder<MuscleGainDbContext> optionsBuilder =
                    new DbContextOptionsBuilder<MuscleGainDbContext>();

                optionsBuilder.UseInMemoryDatabase($"MuscleGain-TestDb-{DateTime.Now.Ticks}");

                return new MuscleGainDbContext(optionsBuilder.Options);
            }
        }
    }
}
