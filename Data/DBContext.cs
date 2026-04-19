using System;

public class DBContext
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Student>()
			.HasIndex(e => e.Email)
			.IsUnique();

		modelBuilder.Entity<Enrollment>()
			.HastIndex(x => new { x.StudentID, x.CourseID })
			.IsUnique();

        modelBuilder.Entity<Enrollment>()
			.HasOne(e => e.Student)
			.WithMany(s => s.Enrollments)
			.HasForeignKey(e => e.StudentID);

		modelBuilder.Entity<Enrollment>()
			.HasOne(e => e.Course)
			.WithMany(c => c.Enrollments)
			.HasForeignKey(e => e.CourseID);
    }
}
