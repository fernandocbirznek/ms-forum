using Microsoft.EntityFrameworkCore;
using ms_forum;
using ms_forum.Domains;
using ms_forum.Extensions;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddControllers();

builder.Services.SetupDbContext(builder.Configuration.GetValue<string>("ConnectionStrings:DbContext"));
builder.Services.SetupRepositories();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Forum).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ForumTag).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ForumTopico).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ForumTopicoResposta).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ForumTopicoReplica).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ForumTopicoTag).Assembly));

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ForumDbContext>().Database.Migrate();

app.Run();
