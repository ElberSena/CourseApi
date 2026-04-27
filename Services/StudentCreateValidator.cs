using FluentValidation;

public class StudentCreateValidator : AbstractValidator<StudentCreateDto>
{
	public StudentCreateValidator()
	{
		RuleFor(x => x.Email).
			EmailAddress()
			.WithMessage("Email deve ser um endereço de email válido.");

		RuleFor(x => x.FullName)
			.NotEmpty();

    }
}
