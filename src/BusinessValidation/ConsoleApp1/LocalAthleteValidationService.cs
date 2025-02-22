using AthleteDomain;
using BusinessValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class LocalAthleteValidationService : BusinessValidator<Athlete>
    {
        public override BusinessValidationResult Validate(Athlete validationObject)
        {
            Validator.Validate(a => a.Name, "{fail-bundle} is not good locally", validationObject, a => a.Id > 0);

            Validator.Validate<Athlete, Guid>(a => a.Team, "{fail-bundle} not valid locally", validationObject.Team != Guid.Empty);

            return new BusinessValidationResult(Validator);
        }
    }
}
