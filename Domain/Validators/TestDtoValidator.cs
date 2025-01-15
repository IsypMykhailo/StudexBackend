using FluentValidation;
using Studex.Domain.Dto;

namespace Studex.Domain.Validators;

public class TestDtoValidator : AbstractValidator<TestDto>
{
    public TestDtoValidator()
    {
    }
}