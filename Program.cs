using Microsoft.EntityFrameworkCore;
using ms_forum;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Interface;
using ms_forum.Service;

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

// Configurar explicitamente a porta HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5004); // Porta HTTP
    //options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Porta HTTPS (opcional)
});

builder.WebHost.UseUrls("http://*:5004");

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

// Acessar outro MS
builder.Services.AddHttpClient<IUsuarioService, UsuarioService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Services:UsuarioService"));
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";  // Isso vai permitir acessar o Swagger via http://localhost:8100/
});

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ForumDbContext>().Database.Migrate();

app.Run();
