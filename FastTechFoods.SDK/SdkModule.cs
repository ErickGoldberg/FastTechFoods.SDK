using FastTechFoods.SDK.Middleware;
using FastTechFoods.SDK.Persistence.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace FastTechFoods.SDK
{
    public static class SdkModule
    {
        public static IServiceCollection AddMongoConnection(this IServiceCollection services, string connectionString)
        {
            var mongoClient = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(mongoClient);

            var mongoUrl = new MongoUrl(connectionString);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            services.AddSingleton(database);

            return services;
        }

        public static IServiceCollection AddSqlContext<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRabbitMqConnectionAsync(
            this IServiceCollection services,
            string hostName,
            string userName,
            string password,
            int port = 5672
        )
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            var connection = factory.CreateConnection();
            services.AddSingleton(connection);

            services.AddScoped<IModel>(sp =>
            {
                var conn = sp.GetRequiredService<IConnection>();
                return conn.CreateModel(); 
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
            where T : class
        {
            services.AddScoped<IMongoRepository<T>>(sp =>
            {
                var db = sp.GetRequiredService<IMongoDatabase>();
                return new MongoRepository<T>(db, collectionName);
            });

            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}