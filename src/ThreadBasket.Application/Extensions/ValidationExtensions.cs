using ErrorOr;
using FluentValidation.Results;

namespace ThreadBasket.Application.Extensions;

public static class ValidationExtensions
{
    public static IEnumerable<Error> ToErrorOrErrors(this ValidationResult result)
    {
        var errors = new List<Error>();

        foreach (var failure in result.Errors)
        {
            errors.Add(Error.Validation(
                failure.ErrorCode,
                failure.ErrorMessage));
        }

        return errors;
    }
}