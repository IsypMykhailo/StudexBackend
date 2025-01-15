using FluentValidation;
using Studex.Domain.Dto;

namespace Studex.Domain.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Topic).NotEmpty().WithMessage("Topic is required");
        RuleFor(x => x.Area).NotEmpty().WithMessage("Area is required");
    }
}