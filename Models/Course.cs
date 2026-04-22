public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Category { get; set; } = null!;

    public int WorkloadHours { get; set; }  

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}