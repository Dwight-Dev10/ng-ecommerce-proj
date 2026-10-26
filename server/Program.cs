using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection")));

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];


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
if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => 
    {
        options.Title = "Ecommerce APi";
        options.Theme = ScalarTheme.Saturn;
        
    });
    
}
// Configure Forwarded Headers Middleware
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseHttpsRedirection();

app.UseCors("AngularEcommercePolicy");

app.MapControllers();


app.Run();
