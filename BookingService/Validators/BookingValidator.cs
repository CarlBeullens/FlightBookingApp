using BookingService.Models;
using FluentValidation;
using FluentValidation.Results;
using Shared.DTOs.Bookings;
using Shared.DTOs.Passengers;

namespace BookingService.Validators;

public static class BookingValidator
{
    public static ValidationResult ValidateEmail(string email)
    {
        var validator = new InlineValidator<string>();
        
        validator.RuleFor(e => e).NotEmpty();
        validator.RuleFor(e => e).EmailAddress();
        
        return validator.Validate(email);
    }
    
    public static ValidationResult ValidateBookingReference(string reference)
    {
        var validator = new InlineValidator<string>();
        
        validator.RuleFor(r => r).NotEmpty();
        validator.RuleFor(r => r).Length(6);
        
        return validator.Validate(reference);
    }
    
    public static ValidationResult ValidateForConfirmation(Booking? booking)
    {
        if (booking == null) return NullResult();
        
        var validator = new InlineValidator<Booking>();
        
        validator.RuleFor(b => b.BookingStatus).Equal(BookingStatus.Pending).WithMessage($"Bookings must be in status {BookingStatus.Pending} to be confirmed");
        //validator.RuleFor(b => b.PaymentStatus).Equal(PaymentStatus.Paid).WithMessage($"Bookings must be in payment status {PaymentStatus.Paid} to be confirmed");
            
        return validator.Validate(booking);
    }

    public static ValidationResult ValidateForCreation(CreateBookingRequest booking)
    {
        var passengerValidator = new InlineValidator<PassengerDto>();
        passengerValidator.RuleFor(p => p.FirstName).NotEmpty().WithMessage("Passenger first name is required");
        passengerValidator.RuleFor(p => p.LastName).NotEmpty().WithMessage("Passenger last name is required");
        passengerValidator.RuleFor(p => p.DateOfBirth).NotEmpty().WithMessage("Passenger date of birth is required");
        passengerValidator.RuleFor(p => p.PassportNumber).NotEmpty().WithMessage("Passenger passport number is required");
        passengerValidator.RuleFor(p => p.Nationality).NotEmpty().WithMessage("Nationality is required");
        
        var createBookingValidator = new InlineValidator<CreateBookingRequest>();
        createBookingValidator.RuleFor(b => b.PrimaryContactName).NotEmpty().WithMessage("Primary contact name is required");
        createBookingValidator.RuleFor(b => b.PrimaryContactEmail).NotEmpty().WithMessage("Primary contact email is required");
        createBookingValidator.RuleFor(b => b.Passengers).NotEmpty().WithMessage("Passengers are required");
        createBookingValidator.RuleForEach(b => b.Passengers).SetValidator(passengerValidator);
        
        return createBookingValidator.Validate(booking);
    }

    public static ValidationResult ValidateForCancellation(Booking? booking)
    {
        if (booking == null) return NullResult();
        
        var validator = new InlineValidator<Booking>();
        
        validator.RuleFor(b => b.BookingStatus).NotEqual(BookingStatus.Cancelled).WithMessage($"Booking was already in status {BookingStatus.Cancelled}");
        
        return validator.Validate(booking);
    }
    
    private static ValidationResult NullResult()
    {
        var validationResult = new ValidationResult();
            
        validationResult.Errors.Add(new ValidationFailure("Booking", "Booking not found"));
            
        return validationResult;
    }
}