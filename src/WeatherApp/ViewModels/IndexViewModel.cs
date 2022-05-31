using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.ViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        public Dictionary<string, string>? AverageTemperatures { get; set; }
    }

    public class IndexViewModelValidator : AbstractValidator<IndexViewModel>
    {
        public IndexViewModelValidator()
        {
            RuleFor(x => x.Latitude)
                .NotNull()
                .WithMessage("Please specify a Latitude");

            RuleFor(x => x.Latitude)
                .LessThanOrEqualTo(90)
                .GreaterThanOrEqualTo(-90).WithMessage("Latitude must be between -90 <-> 90 degress.");

            RuleFor(x => x.Longitude)
                .NotNull()
                .WithMessage("Please specify a Longitude");
            
            RuleFor(x => x.Longitude)
                .LessThanOrEqualTo(180)
                .GreaterThanOrEqualTo(-180)
                .WithMessage("Longitude must be between -180 <-> 180 degress.");
        }
    }
}
