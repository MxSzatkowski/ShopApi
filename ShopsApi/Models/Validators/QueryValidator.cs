using FluentValidation;
using System.Linq;

namespace ShopsApi.Models.Validators
{
    public class QueryValidator : AbstractValidator<ShopQuery>
    {
        private int[] allowPageSizes = new[] { 5, 10, 15 };
        public string[] allowedSortByCollumnNames = 
            {nameof(Shop.Name), nameof(Shop.Description)};
        public QueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
            {
                if (!allowPageSizes.Contains(value))
                {
                    context.AddFailure("PageZise", $"PageSize mus in [{string.Join(",", allowPageSizes)}]");
                }

            });
            RuleFor(x => x.Sort).Must(value => string.IsNullOrEmpty(value) || allowedSortByCollumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or you must if one of [{string.Join(",", allowedSortByCollumnNames)}]");
        }
    }
}
