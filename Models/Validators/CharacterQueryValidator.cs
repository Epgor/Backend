using System.Linq;
using FluentValidation;
using Back.Entities;
using Back.Models;
namespace Back.Models.Validators
{
    public class CharacterQueryValidator : AbstractValidator<CharacterQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
            {nameof(Character.Name), nameof(Character.Location), nameof(Character.Class),
            nameof(Character.Race), nameof(Character.Level), nameof(Character.Money)};
        public CharacterQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
