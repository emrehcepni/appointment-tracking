using AppointmentTracking.Infrastructure;
using AppointmentTracking.Infrastructure.Repositories;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services;
using AppointmentTracking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        }));

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Repositories

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleDetailsRepository, VehicleDetailsRepository>();

#endregion

#region Services

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleDetailsService, VehicleDetailsService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

