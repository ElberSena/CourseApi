using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


[ApiController]
[Route("courses")]	
public class CoursesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CoursesController(AppDbContext context)
		{
			_context = context;
        }

		[HttpPost]
		[Authorize(Roles = "Admin, Instructor")]
		public async Task <IActionResult> Create(CourseCreateDto dto)
		{
			if (dto.Title.Length < 3)
				return BadRequest(new ProblemDetails
				{
					Title = "Título inválido"
				});

			var course = new Course
			{
				Id = Guid.NewGuid(),
				Title = dto.Title,
				Description = dto.Description,
				Category = dto.Category,
				WorkloadHours = dto.WorkloadHours
			};

			_context.Courses.Add(course);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

		[HttpGet]
		[AllowAnonymous]
		[ResponseCache(Duration = 60)]
		public async Task<IActionResult> GetAll(
				[FromQuery] string? category,
				[FromQuery] string? search,
				[FromQuery] string? orderBy = "date",
				[FromQuery] int page = 1,
				[FromQuery] int pageSize = 10
			)
		{
			var query = _context.Courses.AsQueryable();
			if (!string.IsNullOrEmpty(category))
				query = query.Where(c => c.Category == category);

			if (!string.IsNullOrEmpty(search))
				query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));

			query = orderBy switch
			{
				"title" => query.OrderBy(c => c.Title),
				_ => query.OrderByDescending(c => c.CreatedAt)
			};

			var total = await query.CountAsync();

			var courses = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CourseResponseDto
				{
					Id = c.Id,
					Title = c.Title,
					Category = c.Category,
					WorkloadHours = c.WorkloadHours,
					CreatedAt = c.CreatedAt
				})
				.ToListAsync();

			Response.Headers.Add("X-Total-Count", total.ToString());
		
			return Ok(courses);
        }

		[HttpGet("{id}")]
		[AllowAnonymous]
		[ResponseCache(Duration = 60)]
		public async Task<IActionResult> GetById(Guid id)
		{
			var course = await _context.Courses.FindAsync(id);

			if (course == null)
				return NotFound(new ProblemDetails
					 {
						 Title = "Curso não encontrado"
					 });

           

            return Ok(course);
        }

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin, Instructor")]
		public async Task<IActionResult> Update(Guid id, CourseCreateDto dto)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course == null)
				return NotFound(new ProblemDetails
				{
					Title = "Curso não encontrado"
				});
			
			course.Title = dto.Title;
			course.Description = dto.Description;
			course.Category = dto.Category;
			course.WorkloadHours = dto.WorkloadHours;
			
			await _context.SaveChangesAsync();

			return NoContent();
        }

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin, Instructor")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var course = await _context.Courses.FindAsync(id);

			if (course == null)
				return NotFound(new ProblemDetails
				{
					Title = "Curso não encontrado"
				});

			_context.Courses.Remove(course);

			await _context.SaveChangesAsync();

			return NoContent();
        }
    }

