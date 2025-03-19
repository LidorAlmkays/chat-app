using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Validators
{
    public sealed class ValidRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is string roleString)
            {
                return Enum.GetNames<Role>().Contains(roleString);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be one of the following: {string.Join(", ", Enum.GetNames<Role>())}.";
        }
    }
}