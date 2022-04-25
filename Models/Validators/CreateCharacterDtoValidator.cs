using Back.Entities;
using FluentValidation;
namespace Back.Models.Validators
{
    public class CreateCharacterDtoValidator : AbstractValidator<CreateCharacterDto>
    {
        public CreateCharacterDtoValidator(ApiDbContext apiDbContext)
        {
            RuleFor(x => x.Name).
                NotEmpty();

            RuleFor(x => x.Class)
                .NotEmpty();
            RuleFor(x => x.Race)
                .NotEmpty();

            RuleFor(x => x.Name)
                    .Custom((value, context) =>
                    {
                        var nameInUse = apiDbContext.Characters.Any(u => u.Name == value);
                        if (nameInUse)
                        {
                            context.AddFailure("Name", "That name is taken");
                        }
                    });
        }
    }
}
