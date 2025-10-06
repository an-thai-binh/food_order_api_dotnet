using FoodOrderApi.Controllers;
using FoodOrderApi.Repositories;
using FoodOrderApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// đọc connection string từ configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add DI
builder.Services.AddScoped<FoodService>();
builder.Services.AddScoped<FoodRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<CartItemRepository>();
builder.Services.AddScoped<FoodOrderService>();
builder.Services.AddScoped<FoodOrderRepository>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddScoped<OrderDetailRepository>();

// cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
