using System;
using System.IO;

namespace Versioner
{
    public interface IVersionFile
    {
        string GetVersion();
        void Increment();
        void Initialize();
    }

    public class VersionFile : IVersionFile
    {
        private readonly string _versionFilePath;

        public VersionFile(string versionFilePath)
        {
            _versionFilePath = versionFilePath;
        }

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
            File.WriteAllText(_versionFilePath, "1.0.0.0");
        }

        private void IncrementVersion()
        {
            var currentVersion = new Version(GetVersion());
            var newVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build, currentVersion.Revision + 1);
            File.WriteAllText(_versionFilePath, newVersion.ToString());
        }

        private bool VersionFileExists()
        {
            return File.Exists(_versionFilePath);
        }

        private string GetVersionFromFile()
        {
            return File.ReadAllText(_versionFilePath);
        }
    }
}