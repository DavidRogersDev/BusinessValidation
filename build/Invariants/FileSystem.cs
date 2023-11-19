namespace Invariants
{
    public sealed class FileSystem
    {
        public sealed class GlobPattern
        {
            public const string BinPattern = "**/bin";
            public const string ObjPattern = "**/obj";
        }

        public sealed class Nuget
        {
            public const string Packages = "*.nupkg";
            public const string SymbolsPackages = "symbols.nupkg";
        }


        public const string ArtifactsDirectory = "artifacts";
    }
}
