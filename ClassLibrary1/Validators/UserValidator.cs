using Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).MaximumLength(20).WithMessage("Username exceeds maximum length of 20");
            RuleFor(user => user.Password).MaximumLength(20).WithMessage("Password exceeds maximum length of 20");
        }
    }
}
