namespace WebBlazorApi.Domain;

public sealed class Project
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ProjectStatus Status { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Cost { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime DueDateUtc { get; set; }
    public decimal PlannedCost { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
