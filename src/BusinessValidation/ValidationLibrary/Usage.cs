using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationLibrary
{
    internal class Usage
    {
        readonly IBusValidator<ValidationRule> validation;
        public Usage(IBusValidator<ValidationRule> validation)
        {
            this.validation = validation;
            _ = validation.ValidationFailures;
        }
    }
}
