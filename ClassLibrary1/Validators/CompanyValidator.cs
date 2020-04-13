using Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class CompanyValidator : AbstractValidator<CompanyDto>
    {
        /// <summary>
		/// Validation check on company
		/// </summary>
        public CompanyValidator()
        {
            RuleFor(company => company.Name).NotNull().WithMessage("Company Name Required");
            RuleFor(customer => customer.Exchange).NotNull().WithMessage("Exchange Required");
            RuleFor(customer => customer.Ticker).NotNull().WithMessage("Ticker Required");
            RuleFor(customer => customer.Isin).NotNull().WithMessage("Isin Required");
            RuleFor(company => company.Isin).SetValidator(new CompanyIsinValidator()).When(x=>x.Isin != null);
            RuleFor(company => company.Website).Must(LinkMustBeAUri).When(x => !string.IsNullOrEmpty(x.Website)).WithMessage("Website must be Url"); ;
        }
        private static bool LinkMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            Uri outUri;
            return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
