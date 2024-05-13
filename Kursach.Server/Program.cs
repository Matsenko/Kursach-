using Kursach.Data;
using Kursach.Service;
using Kursach.Service.IService;
using Kursach.Services;
using Kursach.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IReadService, ReadService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionCF' not found.");
builder.Services.AddDbContext<KursachContext>(options =>

{
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));

});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();
app.Run();
