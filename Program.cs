using Microsoft.EntityFrameworkCore;
using WebBlazorApi.Data;
using WebBlazorApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WorkshopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("blazor", p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true)); 
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

app.UseCors("blazor");

app.MapGraphQL("/graphql");
app.Run();
