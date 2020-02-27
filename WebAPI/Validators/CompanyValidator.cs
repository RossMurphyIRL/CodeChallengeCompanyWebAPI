using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Validators
{
    public class CompanyIsinValidator : AbstractValidator<string>
    {
        /// <summary>
		/// Validation check on Isin
		/// </summary>
        public CompanyIsinValidator()
        {
            RuleFor(x => x).Must(FirstTwoCharactersAreLetters).WithMessage("Isin not in correct format");
            RuleFor(x => x).Must(FullIsinLength).WithMessage("Full Isin value is not given");
        }

        /// <summary>
		/// Checks if first two characters of string are letters.
		/// </summary>
		/// <param name="isin"></param>
		/// <returns>bool</returns>
        private bool FirstTwoCharactersAreLetters(string isin)
        {
            var firstTwoLetters = isin.Substring(0,2);
            return Regex.IsMatch(firstTwoLetters, @"^[a-zA-Z]+$");
        }

        /// <summary>
		/// Checks that a full Isin value has been passed.
		/// </summary>
		/// <param name="isin"></param>
		/// <returns>bool</returns>
        private bool FullIsinLength(string isin)
        {
            return isin.Length == 12;
        }
    }
}
