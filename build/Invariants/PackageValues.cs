using System;
using System.Linq;

namespace Invariants
{
    public sealed class PackageValues
    {
        public const string Author = "David Rogers";
        public static string Copyright = $"{Author} 2022 - {DateTime.Now.Year}";
        public const string Description = "A library to perform validation in business services and give a mechanism to report failures back to the user interface.";
        public const string Icon = "icon.png";
        public const string MITLicence = "MIT";
        public const string ProjectUrl = "https://github.com/DavidRogersDev/BusinessValidation";
        public const string Readme = "readme.md";
        public const string RepositoryUrl = $"{ProjectUrl}.git";
        public const string Tags = "BusinessValidation Business-Validation Business-Validators Business-Validator Validation Validator Validators";
    }
}
