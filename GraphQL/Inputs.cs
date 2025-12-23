using WebBlazorApi.Domain;

namespace WebBlazorApi.GraphQL;


public sealed record CustomerCreateInput(
    string FullName,
    string Email,
    string Phone,
    string PassportOrIdNumber
);

public sealed record CustomerUpdateInput(
    string? FullName,
    string? Email,
    string? Phone,
    string? PassportOrIdNumber
);


public sealed record PerformerCreateInput(
    string FullName,
    string Email,
    string Phone,
    string Position
);

public sealed record PerformerUpdateInput(
    string? FullName,
    string? Email,
    string? Phone,
    string? Position
);


public sealed record ProjectCreateInput(
    string Code,
    string Name,
    string Description,
    ProjectStatus Status,
    DateTime StartDateUtc,
    DateTime DueDateUtc,
    decimal PlannedCost,
    int CustomerId
);

public sealed record ProjectUpdateInput(
    string? Code,
    string? Name,
    string? Description,
    ProjectStatus? Status,
    DateTime? StartDateUtc,
    DateTime? DueDateUtc,
    decimal? PlannedCost,
    int? CustomerId
);


public sealed record TaskCreateInput(
    string Title,
    string Description,
    WorkTaskStatus Status,
    DateTime DueDateUtc,
    decimal Cost,
    int ProjectId,
    int PerformerId
);

public sealed record TaskUpdateInput(
    string? Title,
    string? Description,
    WorkTaskStatus? Status,
    DateTime? DueDateUtc,
    DateTime? CompletedAtUtc,
    decimal? Cost,
    int? ProjectId,
    int? PerformerId
);
