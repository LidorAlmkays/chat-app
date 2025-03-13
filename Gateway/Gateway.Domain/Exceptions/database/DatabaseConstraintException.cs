using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Domain.Exceptions.database
{

    public class ConstraintViolationException : Exception
    {
        public ConstraintType ConstraintType { get; }

        public ConstraintViolationException(ConstraintType constraintType, string message)
            : base(message)
        {
            ConstraintType = constraintType;
        }
    }

}