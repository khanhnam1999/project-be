using BusinessLogicLayer;
using CommonDataLayer.Entities;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Đắng ký sử dụng cache
builder.Services.AddMemoryCache();
builder.Services.AddTransient<SmsService>();

// Dependency Injection
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

builder.Services.AddScoped<IApartmentDL, ApartmentDL>();
builder.Services.AddScoped<IApartmentBL, ApartmentBL>();

builder.Services.AddScoped<IServiceDL, ServiceDL>();
builder.Services.AddScoped<IServiceBL, ServiceBL>();

builder.Services.AddScoped<IAccountDL, AccountDL>();
builder.Services.AddScoped<IAccountBL, AccountBL>();

builder.Services.AddScoped<IContractDL, ContractDL>();
builder.Services.AddScoped<IContractBL, ContractBL>();

builder.Services.AddScoped<IBookingDL, BookingDL>();
builder.Services.AddScoped<IBookingBL, BookingBL>();

builder.Services.AddScoped<IResidentDL, ResidentDL>();
builder.Services.AddScoped<IResidentBL, ResidentBL>();

builder.Services.AddScoped<IIncidentDL, IncidentDL>();
builder.Services.AddScoped<IIncidentBL, IncidentBL>();

builder.Services.AddScoped<IPaymentDL, PaymentDL>();
builder.Services.AddScoped<IPaymentBL, PaymentBL>();
// Thêm SignalR service
builder.Services.AddSignalR();

// Đăng ký DbContext với SQL Server
builder.Services.AddDbContext<CondoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions =>
        {
            sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
            //sqlServerOptions.MigrationsAssembly("ApartmentsManagement.Ntier");
        }
    )
);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials(); // cần cho SignalR;
                    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Config Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"];
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        ),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký BackgroundService
builder.Services.AddHostedService<MonthlyPaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Kiểm tra kết nối khi khởi động
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CondoContext>();
    if (context.Database.CanConnect())
    {
        Console.WriteLine("✅ Kết nối thành công tới SQL Server!");
    }
    else
    {
        Console.WriteLine("❌ Không thể kết nối tới SQL Server.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map Hub
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
