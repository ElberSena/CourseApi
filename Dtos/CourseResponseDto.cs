
using System;

public class CourseResponseDto
{

    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int WorkloadHours { get; set; }
    public DateTime CreatedAt { get; set; }
}   
