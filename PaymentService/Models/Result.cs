using FluentValidation.Results;

namespace PaymentService.Models;

public class Result<T>
{
    public T? Data { get; set; }
    public ValidationResult? ValidationResult { get; set; }
    public bool IsSuccess => ValidationResult?.IsValid ?? true;
    
    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            Data = data
        };
    }
    
    public static Result<T> Failure(ValidationResult validationResult)
    {
        return new Result<T>
        {
            ValidationResult = validationResult
        };
    }
}