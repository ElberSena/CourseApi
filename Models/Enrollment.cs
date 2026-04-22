public class Enrollment
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public EnrollmentStatus Status { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}