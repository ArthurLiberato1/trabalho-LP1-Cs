using ArquivosLibrary.Repository;
using ArquivosLibrary.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trabalho Final 1� Bimestre - LP1",
        Version = "v1",
        Description = $@"<h3>API de <b>Cidades e Alunos</b></h3>
                                      <p>
                                          Arthur Liberato E�g�nio - 262318881
                                      </p>",
        Contact = new OpenApiContact
        {
            Name = "Suporte Unoeste",
            Email = string.Empty,
            Url = new Uri("https://www.unoeste.br"),
        },
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// Add services to the container.

builder.Services.AddControllers();





// Acessando o valor diretamente pelo builder.Configuration
Environment.SetEnvironmentVariable("STRING_CONEXAO", builder.Configuration["StringConexao"]);


builder.Services.AddScoped<AlunosRepository>();
builder.Services.AddScoped<AlunosService>();
//tem que adicionar no program.cs os reposit�rios e servi�os 
builder.Services.AddScoped<CidadeRepository>();
builder.Services.AddScoped<CidadesService>();

DbContext dbContext = new DbContext();
builder.Services.AddSingleton(dbContext);


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.RoutePrefix = ""; //habilitar a p�gina inicial da API ser a doc.
    c.DocumentTitle = "Gerenciamento de Produtos - API V1";
});


// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
