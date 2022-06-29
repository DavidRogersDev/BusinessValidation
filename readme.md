[![NuGet](https://img.shields.io/nuget/v/BusinessValidation.svg?style=flat-square)](https://www.nuget.org/packages/BusinessValidation/) BusinessValidation  

# BusinessValidation
An awfully small library for performing validation in Business Services.

## Run Business Validation Rules
A simple example:
```csharp
var validator = new Validator();
// basic validation. The checkRego method (last argument) would return a Boolean.
validator.Validate("CarRegistration", "Vehicle does not have a current, valid registration", checkRego(someVehicle));

if(validator) {
    var licensee = GetLicensee(licenceNr);

    if(licensee is null) {
        // Can't pass a null value to the validator, but can add a raw message to the relevant Failure Bundle.
        validator.AddFailure("Licensee", $"No licensee exists with the licence number {licenceNr}.");
    } else {
        // validate an object
        validator.Validate(l => l.DateOfBirth, $"No valid date of birth is stored for the licensee with licence number {licenceNr}", licensee, l => l.DateOfBirth > DateTime.MinValue);
    }    
}    
validator.Throw(); // throws exception, only if IsValid() is false, which wraps the Validation Failures dictionary.
return validator; // returns object which implicitly casts to "true" if valid. 
```
More example code can be found in the **sample** project in the repo code.
## Read About it
I wrote an article explaining the genesis and usage of Business Validation here.