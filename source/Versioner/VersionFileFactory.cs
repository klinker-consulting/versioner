namespace Versioner
{
    public interface IVersionFileFactory
    {
        IVersionFile Create(string versionFile);
    }

    public class VersionFileFactory : IVersionFileFactory
    {
        public IVersionFile Create(string versionFile)
        {
            return new VersionFile(versionFile);
        }
    }
}
