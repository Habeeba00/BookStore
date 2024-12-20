
using BookStore.MappingConfigs;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Add Swagger configurations
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "BookStore API",
                    Version = "v1",
                    Description = "API for managing the BookStore application.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Habiba Mohamed",
                        Email = "habiibamohamed259@gmail.com",
                    }
                });
                options.EnableAnnotations();
            });
                builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BookStoreDBContext>();
            builder.Services.AddDbContext<BookStoreDBContext>
                (op => op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("conn"))
                //.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
                );
            builder.Services.AddScoped<UnitOfWorks>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                .AddJwtBearer(
                //validate token
                op =>
                {
                    //op.SaveToken = true;
                    #region secret key
                    string key = "welcome to my secret key Habiba Mohamed";
                    var secetkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                    #endregion
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = secetkey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            var app = builder.Build();

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
