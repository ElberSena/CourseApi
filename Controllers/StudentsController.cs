using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

[ApiController]
[Route("students")]
public class StudentsController : ControllerBase
{
	private readonly AppDbContext _context;

	public StudentsController(AppDbContext context)
	{
		_context = context;
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Create(StudentCreateDto dto)
	{
		var exists = await _context.Students.AnyAsync(s => s.Email == dto.Email);

		if (exists)
			return Conflict(new ProblemDetails
			{
				Title = "Email já cadastrado"
			});

		var student = new Student
		{
			Id = Guid.NewGuid(),
			FullName = dto.FullName,
			Email = dto.Email,
			UserId = dto.UserId
		};
		_context.Students.Add(student);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetAll()
	{
		var students = await _context.Students
			.Where(s => !s.IsDeleted)
			.Select(s => new StudentResponseDto
			{
				Id = s.Id,
				FullName = s.FullName,
				Email = s.Email,
				CreatedAt = s.CreatedAt
			})
			.ToListAsync();

		return Ok(students);
	}

	[HttpGet("{id}")]
	[Authorize]
	public async Task<IActionResult> GetById(Guid id)
	{
		var student = await _context.Students.FindAsync(id);

		if (student == null || student.IsDeleted)
			return NotFound();

		var userId = GetUserId();

		var isAdmin = User.IsInRole("Admin");
		var isOwner = student.UserId == userId;

		if (!isAdmin && !isOwner)
			return Forbid();

		return Ok(new StudentResponseDto
		{
			Id = student.Id,
			FullName = student.FullName,
			Email = student.Email,
			CreatedAt = student.CreatedAt,
		});

	}

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, StudentUpdateDto dto)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null || student.IsDeleted)
            return NotFound();

        var userId = GetUserId();

        var isAdmin = User.IsInRole("Admin");
        var isOwner = student.UserId == userId;

        if (!isAdmin && !isOwner)
            return Forbid();

        student.FullName = dto.FullName;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        student.IsDeleted = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("/me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = GetUserId();

        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

        if (student == null)
            return NotFound();

        return Ok(new StudentResponseDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            CreatedAt = student.CreatedAt
        });
    }

    private string GetUserId()
    {
        return User.FindFirst("sub")?.Value
            ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
    }

}


