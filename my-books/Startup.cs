using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using my_books.Data;
using my_books.Data.Services;
using my_books.Exceptions;

namespace my_books
{
  public class Startup
  {
    public string ConnectionString { get; set; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

      services.AddTransient<BooksService>();
      services.AddTransient<AuthorsService>();
      services.AddTransient<PublishersService>();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "my_books_updated_title", Version = "v2" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "my_books_ui_updated v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      // Custom Exception Handling
      // app.ConfigureCustomExceptionHandler();
      
      app.ConfigureBuiltInExceptionHandler();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      // Not in Use, kept as a reference
      // AppDbInitializer.Seed(app);

    }
  }
}
