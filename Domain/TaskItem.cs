namespace WebBlazorApi.Domain;

public sealed class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public WorkTaskStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime DueDateUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public decimal Cost { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int PerformerId { get; set; }
    public Performer Performer { get; set; } = null!;
}
