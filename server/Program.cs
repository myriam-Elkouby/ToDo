using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//how to do a dependency injection?
builder.Services.AddDbContext<ToDoDbContext>();

builder.Services.AddSwaggerGen();

var  AllowReactOrigins = "ReactOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowReactOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(AllowReactOrigins);


//getTasks

app.MapGet("/items", async(ToDoDbContext context) => {
    var items = await context.Items.ToListAsync();
    return Results.Ok(items);
});

//addTask
app.MapPost("/items", async (string name,ToDoDbContext context) => {   
    Item item = new Item { Name = name, IsComplete = false };
    context.Items.Add(item);
    await context.SaveChangesAsync();
    return Results.Ok(item);
});

//setCompleted
app.MapPut("/items/{id:int}", async(int id, bool complete,ToDoDbContext context) => {
    var item = await context.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    item.IsComplete = complete;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

//deleteTask
app.MapDelete("/items/{id:int}", async(int id,ToDoDbContext context) => {
    var item = await context.Items.FindAsync(id);
    if(item == null){
        return Results.NotFound();
    }  
    context.Items.Remove(item);
    await context.SaveChangesAsync(); 
    return Results.NoContent();
});




// if (app.Environment.IsDevelopment())
// {
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapGet("/",()=>"server Api is running");

app.Run();
//app.Run("https://localhost:5170");



//https://localhost:5170