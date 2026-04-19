using System;

public class Student
{
	public Guid ID { get; set; }
	public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserID { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public bool IsDelete { get; set; } = true;

	public IColletion<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
