using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Automapper;
using MoviesApi.Context;
using NetTopologySuite;

namespace MoviesApi.Test
{
    public class DatabaseTests
    {
        protected DataContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(dbName).Options;

            var dbContext = new DataContext(options);
            return dbContext;
        }

        protected IMapper ConfigAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMaperProfile(geometryFactory));
            });

            return config.CreateMapper();
        }
    }
}
