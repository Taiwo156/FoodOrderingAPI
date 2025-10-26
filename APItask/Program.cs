using APItask.Data;
using APItask.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Configuration;
using APItask.Services;
using APItask.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EssentialProductsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));

// Register UserService and IUserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IChatbotService, ChatbotService>();
builder.Services.Configure<MistralAISettings>(builder.Configuration.GetSection("MistralAI"));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.WebHost.UseUrls("http://0.0.0.0:5273");



builder.Services.AddControllers();

// CORS policy to allow any origin
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddDbContext<EssentialProductsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Data Source=tcp:DESKTOP-QVHERC7,1433;Initial Catalog=Swagger;Persist Security Info=True;User ID=Enterprise;Password=entx!2003n;Trust Server Certificate=True;"),
        b => b.MigrationsAssembly("APItask.Data") // Add this line
    ));

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FoodOrderingAPItask",
        Version = "V1",
        Description = "This API is designed for a Food Ordering app.",
        
        Contact = new OpenApiContact
        {
            Name = "Taiwo",
            Email = "oluboyedetaiwo@gmail.com",
            
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

app.MapControllers();

// Swagger configuration
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", " EssentialProducts API V1");
});

app.Run();
