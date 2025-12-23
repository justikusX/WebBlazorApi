using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using WebBlazorApi.Data;
using WebBlazorApi.Domain;

namespace WebBlazorApi.GraphQL;

public sealed class Query
{
    [UseProjection, UseFiltering, UseSorting]
    public IQueryable<Customer> GetCustomers([Service] WorkshopDbContext db) =>
        db.Customers.AsNoTracking();

    [UseProjection, UseFiltering, UseSorting]
    public IQueryable<Project> GetProjects([Service] WorkshopDbContext db) =>
        db.Projects.AsNoTracking();

    [UseProjection, UseFiltering, UseSorting]
    public IQueryable<Performer> GetPerformers([Service] WorkshopDbContext db) =>
        db.Performers.AsNoTracking();

    [UseProjection, UseFiltering, UseSorting]
    public IQueryable<TaskItem> GetTasksByPerformer(int performerId, [Service] WorkshopDbContext db) =>
        db.Tasks.AsNoTracking().Where(t => t.PerformerId == performerId);
}
