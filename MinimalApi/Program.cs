using Microsoft.EntityFrameworkCore;
using MinimalApi;
using MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PubContext>();
builder.Services.AddTransient<Some>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//utilizacion de swagger para testear los metodos en una minimal API
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("/beers", () =>
{
    using (var context = new PubContext()) 
    {
        return context.Beers.ToList();
    }
});

app.MapGet("/beerinject", async (PubContext context) =>
    await context.Beers.ToListAsync()
); ;

app.MapGet("/some", (Some some) => some.Hi());

app.MapGet("/beer/{id}", async(int id, PubContext context) =>
{
    var beer = await context.Beers.FindAsync(id);
    return beer != null ? Results.Ok(beer) : Results.NotFound();
});

app.MapPost("/post", (Data data)=> $"{data.Id} {data.Name}");


app.Run();
