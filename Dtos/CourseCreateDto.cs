using System;

public class CourseCreateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Category { get; set; } = null!;
    public int WorkloadHours { get; set; }
}
