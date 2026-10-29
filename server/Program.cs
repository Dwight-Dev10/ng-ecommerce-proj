using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection")));


var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length == 0)
    throw new InvalidOperationException("Cors:AllowedOrigins is empty.");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularEcommercePolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.MapScalarApiReference(options => 
//     {
//         options.Title = "Ecommerce APi";
//         options.Theme = ScalarTheme.Saturn;
        
//     });
    
// }

app.MapOpenApi();
    app.MapScalarApiReference(options => 
    {
        options.Title = "Ecommerce APi";
        options.Theme = ScalarTheme.Saturn;
        
    });


app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AngularEcommercePolicy");

// app.UseAuthorization();
app.MapControllers();

app.Run();
