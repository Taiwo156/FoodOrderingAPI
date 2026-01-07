using APItask.Data;
using APItask.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using APItask.Services;
using APItask.Core.Models;

namespace APItask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<EssentialProductsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbContext")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            // Register UserService and IUserService
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register FavoritesService and IFavoritesService
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IFavoritesRepository, FavoritesRepository>();

            // Register StoreService and IStoreService
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreRepository, StoreRepository>();

            //Register ProductByStore...
            services.AddScoped<IProductByStoreRepository, ProductByStoreRepository>();
            services.AddScoped<IProductByStoreService, ProductByStoreService>();

            //Register Order
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            //Delivery
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IPaymentProviderService, PaystackService>();



            //chatbot
            services.AddHttpClient();
            services.AddSingleton<IChatbotService, ChatbotService>();
            services.Configure<MistralAISettings>(Configuration.GetSection("MistralAI"));


            services.AddControllers();

            // CORS policy to allow any origin
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Configure Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Food Ordering API",
                    Version = "V1",
                    Description = "Production-ready food ordering system with multi-payment integration and AI chatbot",
                    TermsOfService = new Uri("https://karthiktechblog.com/copyright"),
                    Contact = new OpenApiContact
                    {
                        Name = "Taiwo Oluboyede",
                        Email = "oluboyedetaiwo156@gmail.com",
                        Url = new Uri("https://taiwo156.github.io/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });

                //options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/error"); // Add error handling
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
                c.SwaggerEndpoint("./v1/swagger.json", "Learn Smart Coding - EssentialProducts API V1");
            });
        }
    }
}
