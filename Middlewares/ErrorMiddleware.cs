using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class ErrorMiddleware
{
	private readonly RequestDelegate _next;
	public ErrorMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);

		}
		catch (Exception ex)
		{
			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";
			var problem = new ProblemDetails
			{
				Title = "Ocorreu um erro inesperado.",
				Detail = ex.Message,
				Status = 500
			};

			var json = JsonSerializer.Serialize(problem);

			await context.Response.WriteAsJsonAsync(problem);
		}
	}
}
