namespace WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

//SQLite + EF Core đã làm việc đó rồi khi bạn có DbContext + migration.
//File này tồn tại để TỰ ĐỘNG ÁP DỤNG MIGRATION KHI APP CHẠY.
// File này đảm bảo database luôn được migrate lên version mới nhất mỗi khi application start.
// we need provision our scope in order to allows us to actually start interacting with database 
// create an instance in our scope, we can use to request the service container of .NET core to give us an instance some of the service that have been registerd in the applciaiton
public static class DataExtensions
{
    //Đây là Extension Method => cho phép bạn gọi trong Program.cs như thể này app.MigrationDb() 
    public static void MigrationDb(this WebApplication app)
    {
        //GameStoreContext được đăng ký là Scoped
        //Scope giống như “vòng đời ngắn” để dùng service tạm thời
        using var scope = app.Services.CreateScope();
        //Lấy instance của GameStoreContext từ service container
        //EF Core:
        //Injection connection string
        //Inject provider (SQLite, SQL Server, etc.)
        var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        //Kiểm tra lại database hiệnt tại
        //Xem migration nào đã được apply (__EFMigrationHistory)
        //Apply migration còn thiếu
        //Tạo database nếu chưa tồn tại
        db.Database.Migrate(); // Giữ database schema luôn sync với code
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        //this line is going to register our GameStoreContext as a service in the dependency injection container of .NET Core
        //this means that whenever we need an instance of GameStoreContext, we can request it from the service container, 
        //and it will provide us with a properly configured instance that we can use to interact with our database.
        //this is Dependency Injection (DI) + Data access
        var connString = builder.Configuration.GetConnectionString("GameStore");
        builder.Services.AddSqlServer<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                // Seed the database with initial data if it's empty
                if (!context.Set<Genre>().Any())
                {
                context.Set<Genre>().AddRange(
                        new Genre { Name = "Action" },
                        new Genre { Name = "Adventure" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Strategy" }
                    );
                    context.SaveChanges();
                }
            })
    
        );
    }
}