using APItask.Data;
using APItask.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EssentialProductsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));

// Register UserService and IUserService
builder.Services.AddScoped<IUserService, UserService>();

// Register ProductService and ProductRepository
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();

// Register FavoritesService and IFavoritesService
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();

//Register ProductByStore....
builder.Services.AddScoped<IProductByStoreRepository, ProductByStoreRepository>();
builder.Services.AddScoped<IProductByStoreService, ProductByStoreService>();

// Register StoreService and IStoreService
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();

//Order
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//Delivery
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();

builder.Services.AddControllers();

// CORS policy to allow any origin
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TWsMergedAPItask",
        Version = "V1",
        Description = "This API is designed for a Food Ordering app.",
        
        Contact = new OpenApiContact
        {
            Name = "Olawale and Taiwo",
            Email = "olawaleomobulejo@gmail.com",
            
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
           
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseExceptionHandler("/error"); // Optional error handling
}

app.UseCors("default");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Swagger configuration
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learn Smart Coding - EssentialProducts API V1");
});

app.Run();
