using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("enrollments")]
public class EnrollmentsController : ControllerBase
{
	private readonly AppDBContex _context;

	public EnrollmentsController(AppDBContex context)
	{
		_context = context;
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionsresult> Enroll(EnrollmentCreateDto dto)
	{
		var userId = GetUserId();

		Guid studentId;

		if (userId.IsRole("Admin"))
		{
			if (dto.Studentid = null) {
				return BadRequest(new ProblemDetails
				{
					Title = "StudentId é obrigatório para Admin"
				});
			}

			studentId = dto.StudentId.Value;
		}
		else
		{
			var student = await _context.Students
				.FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

			if (student == null)
			{
				return NotFound(new ProblemDetails
				{
					Title = "Perfil de estudante não encontrado"
				}):
			}

			studentId = student.Id;
		}

		var studentExists = await _context.Students
			.AnyAsync(s => s.Id == studentId && !s.IsDeleted);

		if (!studentExists) {
			return NotFound(new ProblemDetails
			{
				Title = "Estudante não encontrado"
			});
		}

		var courseExists = await _context.Courses
			.anyAsysnc(c => c.Id == dto.CourseId && !c.IsDeleted);

		if (!courseExists) {
			return NotFound(new ProblemDetails
			{
				Title = "Curso não encontrado"
			});
		}

		var exists = await _context.Enrollments
			.AnyAsysnc(e => e.StudentId == studentId && e.CourseId == dto.CourseId);

		if (exists)
		{
			return Conflict(new ProblemDetails
			{
				Title = "Estudante já matriculado neste curso"

			});
		}

		var enrollment = new Enrollment
		{
			Id = Guid.NewGuid(),
			StudentId = studentId,
			CourseId = dto.CourseId,
			Status = Enrollmentstatus.Active
		}:

		_context.Enrollments.Add(enrollment);
		await _context.SaveChangesAsync();

		return Ok(enrollment);
	}

	[HttpGet("/students/{id}/enrollments")]
	[Authorize]
	public async Task<IActionsResult> GetStudentEnrollements(
		Guid id
		[Fromquery] string? status,
		[Fromquery] int page = 1,
		[Fromquery] int pageSize = 10
    ) {
    {
		var userId = GetUserId();

		var isAdmin = userId.IsRole("Admin");

		var student = await _context.Students.FindAsync(id);

		if (student == null)
			return NotFound();

		if (!isAdmin && student.UserId != userId) {
			return Forbid();
        }

		var query = _context.Enrollments
			.Where(e => e.StudentId == id)
			.Include(e => e.Course)
			.AsQueryable();

			if (!string.IsNullOrEmpty(status) && Enum.TryParse<EnrollmentStatus>(status, true, out var parsedStatus))
			{ 
				query = query.Where(e => e.Status == parsedStatus);
			}

            var total = await query.CountAsync();

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new
                {
                    e.Id,
                    Course = e.Course.Title,
                    e.Status,
                    e.EnrolledAt
                })
                .ToListAsync();

            Response.Headers["X-Total-Count"] = total.ToString();

            return Ok(result);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var userId = GetUserId();

        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enrollment == null)
            return NotFound();

        var isAdmin = User.IsInRole("Admin");
        var isOwner = enrollment.Student.UserId == userId;

        if (!isAdmin && !isOwner)
            return Forbid();

        enrollment.Status = EnrollmentStatus.Cancelled;

        await _context.SaveChangesAsync();

        return NoContent();
    }

   
    private string GetUserId()
    {
        return User.FindFirst("sub")?.Value
            ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
    }
}
