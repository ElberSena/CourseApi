using System;

public class studentResponseDto
{
	public Guid Id { get; set; }
	public string Fullname { get; set; } = null!;
	public string Email { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}
