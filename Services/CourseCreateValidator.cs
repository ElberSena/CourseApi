using FluentValidation;

public class CourseCreateValidator : AbstractValidator<CourseCreateDto>
{
	public CourseCreateValidator()
	{
		RuleFor(x => x.Title)
			.MinimumLength(3)
			.WithMessage("Título deve conter pelo menos 3 caracteres.");

		RuleFor(x => x.Category).
			NotEmpty();

		RuleFor(x => x.WorkloadHours)
			.GreaterThan(0); 


    }
}
