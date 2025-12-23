using Microsoft.EntityFrameworkCore;
using WebBlazorApi.Data;
using WebBlazorApi.Domain;

namespace WebBlazorApi.GraphQL;

public sealed class Mutation
{
    
    public async Task<MutationResult<Customer>> CreateCustomer(
        CustomerCreateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = new Customer
        {
            FullName = input.FullName.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            PassportOrIdNumber = input.PassportOrIdNumber.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        db.Customers.Add(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<Customer>.Success(entity);
    }

    public async Task<MutationResult<Customer>> UpdateCustomer(
        int id,
        CustomerUpdateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return MutationResult<Customer>.Fail($"Customer {id} not found");

        if (input.FullName is not null) entity.FullName = input.FullName.Trim();
        if (input.Email is not null) entity.Email = input.Email.Trim();
        if (input.Phone is not null) entity.Phone = input.Phone.Trim();
        if (input.PassportOrIdNumber is not null) entity.PassportOrIdNumber = input.PassportOrIdNumber.Trim();

        await db.SaveChangesAsync(ct);
        return MutationResult<Customer>.Success(entity);
    }

    public async Task<MutationResult<bool>> DeleteCustomer(
        int id,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return MutationResult<bool>.Fail($"Customer {id} not found");

        var hasProjects = await db.Projects.AnyAsync(p => p.CustomerId == id, ct);
        if (hasProjects) return MutationResult<bool>.Fail("Cannot delete customer: customer has projects");

        db.Customers.Remove(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<bool>.Success(true);
    }

    

    public async Task<MutationResult<Performer>> CreatePerformer(
        PerformerCreateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = new Performer
        {
            FullName = input.FullName.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            Position = input.Position.Trim(),
            HiredAtUtc = DateTime.UtcNow
        };

        db.Performers.Add(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<Performer>.Success(entity);
    }

    public async Task<MutationResult<Performer>> UpdatePerformer(
        int id,
        PerformerUpdateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Performers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return MutationResult<Performer>.Fail($"Performer {id} not found");

        if (input.FullName is not null) entity.FullName = input.FullName.Trim();
        if (input.Email is not null) entity.Email = input.Email.Trim();
        if (input.Phone is not null) entity.Phone = input.Phone.Trim();
        if (input.Position is not null) entity.Position = input.Position.Trim();

        await db.SaveChangesAsync(ct);
        return MutationResult<Performer>.Success(entity);
    }

    public async Task<MutationResult<bool>> DeletePerformer(
        int id,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Performers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return MutationResult<bool>.Fail($"Performer {id} not found");

        var hasTasks = await db.Tasks.AnyAsync(t => t.PerformerId == id, ct);
        if (hasTasks) return MutationResult<bool>.Fail("Cannot delete performer: performer has tasks");

        db.Performers.Remove(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<bool>.Success(true);
    }

    

    public async Task<MutationResult<Project>> CreateProject(
        ProjectCreateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        if (input.DueDateUtc < input.StartDateUtc)
            return MutationResult<Project>.Fail("DueDateUtc must be >= StartDateUtc");

        var customerExists = await db.Customers.AnyAsync(c => c.Id == input.CustomerId, ct);
        if (!customerExists) return MutationResult<Project>.Fail($"Customer {input.CustomerId} not found");

        var codeExists = await db.Projects.AnyAsync(p => p.Code == input.Code, ct);
        if (codeExists) return MutationResult<Project>.Fail("Project code already exists");

        var entity = new Project
        {
            Code = input.Code.Trim(),
            Name = input.Name.Trim(),
            Description = input.Description.Trim(),
            Status = input.Status,
            StartDateUtc = input.StartDateUtc,
            DueDateUtc = input.DueDateUtc,
            PlannedCost = input.PlannedCost,
            CustomerId = input.CustomerId
        };

        db.Projects.Add(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<Project>.Success(entity);
    }

    public async Task<MutationResult<Project>> UpdateProject(
        int id,
        ProjectUpdateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (entity is null) return MutationResult<Project>.Fail($"Project {id} not found");

        if (input.CustomerId is not null)
        {
            var customerExists = await db.Customers.AnyAsync(c => c.Id == input.CustomerId.Value, ct);
            if (!customerExists) return MutationResult<Project>.Fail($"Customer {input.CustomerId} not found");
            entity.CustomerId = input.CustomerId.Value;
        }

        if (input.Code is not null)
        {
            var newCode = input.Code.Trim();
            var codeExists = await db.Projects.AnyAsync(p => p.Code == newCode && p.Id != id, ct);
            if (codeExists) return MutationResult<Project>.Fail("Project code already exists");
            entity.Code = newCode;
        }

        if (input.Name is not null) entity.Name = input.Name.Trim();
        if (input.Description is not null) entity.Description = input.Description.Trim();
        if (input.Status is not null) entity.Status = input.Status.Value;
        if (input.PlannedCost is not null) entity.PlannedCost = input.PlannedCost.Value;

        if (input.StartDateUtc is not null) entity.StartDateUtc = input.StartDateUtc.Value;
        if (input.DueDateUtc is not null) entity.DueDateUtc = input.DueDateUtc.Value;

        if (entity.DueDateUtc < entity.StartDateUtc)
            return MutationResult<Project>.Fail("DueDateUtc must be >= StartDateUtc");

        await db.SaveChangesAsync(ct);
        return MutationResult<Project>.Success(entity);
    }

    public async Task<MutationResult<bool>> DeleteProject(
        int id,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (entity is null) return MutationResult<bool>.Fail($"Project {id} not found");

        
        db.Projects.Remove(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<bool>.Success(true);
    }

    

    public async Task<MutationResult<TaskItem>> CreateTask(
        TaskCreateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var projectExists = await db.Projects.AnyAsync(p => p.Id == input.ProjectId, ct);
        if (!projectExists) return MutationResult<TaskItem>.Fail($"Project {input.ProjectId} not found");

        var performerExists = await db.Performers.AnyAsync(p => p.Id == input.PerformerId, ct);
        if (!performerExists) return MutationResult<TaskItem>.Fail($"Performer {input.PerformerId} not found");

        var entity = new TaskItem
        {
            Title = input.Title.Trim(),
            Description = input.Description.Trim(),
            Status = input.Status,
            DueDateUtc = input.DueDateUtc,
            Cost = input.Cost,
            ProjectId = input.ProjectId,
            PerformerId = input.PerformerId,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = input.Status == WorkTaskStatus.Done ? DateTime.UtcNow : null
        };

        db.Tasks.Add(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<TaskItem>.Success(entity);
    }

    public async Task<MutationResult<TaskItem>> UpdateTask(
        int id,
        TaskUpdateInput input,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return MutationResult<TaskItem>.Fail($"Task {id} not found");

        if (input.ProjectId is not null)
        {
            var projectExists = await db.Projects.AnyAsync(p => p.Id == input.ProjectId.Value, ct);
            if (!projectExists) return MutationResult<TaskItem>.Fail($"Project {input.ProjectId} not found");
            entity.ProjectId = input.ProjectId.Value;
        }

        if (input.PerformerId is not null)
        {
            var performerExists = await db.Performers.AnyAsync(p => p.Id == input.PerformerId.Value, ct);
            if (!performerExists) return MutationResult<TaskItem>.Fail($"Performer {input.PerformerId} not found");
            entity.PerformerId = input.PerformerId.Value;
        }

        if (input.Title is not null) entity.Title = input.Title.Trim();
        if (input.Description is not null) entity.Description = input.Description.Trim();
        if (input.DueDateUtc is not null) entity.DueDateUtc = input.DueDateUtc.Value;
        if (input.Cost is not null) entity.Cost = input.Cost.Value;

        if (input.Status is not null)
        {
            entity.Status = input.Status.Value;
            if (entity.Status == WorkTaskStatus.Done)
                entity.CompletedAtUtc ??= DateTime.UtcNow;
            else
                entity.CompletedAtUtc = null;
        }

        if (input.CompletedAtUtc is not null)
            entity.CompletedAtUtc = input.CompletedAtUtc;

        await db.SaveChangesAsync(ct);
        return MutationResult<TaskItem>.Success(entity);
    }

    public async Task<MutationResult<bool>> DeleteTask(
        int id,
        [Service] WorkshopDbContext db,
        CancellationToken ct)
    {
        var entity = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return MutationResult<bool>.Fail($"Task {id} not found");

        db.Tasks.Remove(entity);
        await db.SaveChangesAsync(ct);
        return MutationResult<bool>.Success(true);
    }
}
