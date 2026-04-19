using System;

public class Course
{
	public Guid ID { get; set; }
	public string Title { get; set; } = null!;
	public string Description { get; set; }
	public string Category { get; set; } = null!;
    public string Workload { get; set; }
	public DateTime	CreatedAt { get; set; } = Datetime.UtcNow;
	public bool Isdelete { get; set; };

	public ICollecin<Enrollment> Enrollments { get; set; } + new List<Enrollment>();
}
