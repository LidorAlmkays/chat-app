using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Validators
{
    public sealed class StrictEmailAttribute : ValidationAttribute
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

        public override bool IsValid(object? value)
        {
            if (value is string email)
            {
                return EmailRegex.IsMatch(email);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is not a valid email address. It must include a full domain (e.g., example@gmail.com).";
        }
    }
}