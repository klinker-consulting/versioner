using System;
using System.IO;

namespace Versioner
{
    public interface IVersionFile
    {
        string Path { get; }
        string GetVersion();
        void Increment();
        void Initialize();
    }

    public class VersionFile : IVersionFile
    {
        private readonly string _versionFileKey;

        public VersionFile(string versionFileKey)
        {
            _versionFileKey = versionFileKey;
        }

        public string Path => System.IO.Path.Combine(Directory.GetCurrentDirectory(), _versionFileKey);

        public string GetVersion()
        {
            return VersionFileExists()
                ? GetVersionFromFile()
                : null;
        }

        public void Increment()
        {
            if (!VersionFileExists())
                Initialize();
            else
                IncrementVersion();
        }

        public void Initialize()
        {
            if (VersionFileExists())
                return;
            File.WriteAllText(_versionFileKey, "1.0.0.0");
        }

        private void IncrementVersion()
        {
            var currentVersion = new Version(GetVersion());
            var newVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build, currentVersion.Revision + 1);
            File.WriteAllText(_versionFileKey, newVersion.ToString());
        }

        private bool VersionFileExists()
        {
            return File.Exists(_versionFileKey);
        }

        private string GetVersionFromFile()
        {
            return File.ReadAllText(_versionFileKey);
        }
    }
}