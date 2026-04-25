using System;

public class StudentResponseDto
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}
