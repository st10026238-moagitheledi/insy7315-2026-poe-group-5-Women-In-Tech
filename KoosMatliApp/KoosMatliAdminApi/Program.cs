
using KoosMatliAdminApi.Data;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            // Use an in-memory database for development / local testing
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("KoosMatliDev"));

            var app = builder.Build();

            // Seed sample data
            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataSeeder.Seed(ctx);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
