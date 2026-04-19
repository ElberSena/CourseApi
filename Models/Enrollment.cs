using System;

public class Enrollment
{
	public Guid ID { get; set; }

	public Guid StudentID { get; set; }
	public Student Student { get; set; } = null!;

	public Guid CourseID { get; set; }
	public Course Course { get; set; } = null!;

	public EnrollmentStatus Status { get; set; }
	public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

}
