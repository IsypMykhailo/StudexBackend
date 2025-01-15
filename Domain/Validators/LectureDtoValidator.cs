using FluentValidation;
using Studex.Domain.Dto;

namespace Studex.Domain.Validators;

public class LectureDtoValidator : AbstractValidator<LectureDto>
{
    public LectureDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}