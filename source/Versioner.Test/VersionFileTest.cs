using System;
using System.IO;
using NUnit.Framework;

namespace Versioner.Test
{
    [TestFixture]
    public class VersionFileTest
    {
        private string _versionFilePath;
        private VersionFile _versionFile;

        [SetUp]
        public void Setup()
        {
            _versionFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _versionFile = new VersionFile(_versionFilePath);
        }

        [Test]
        public void GetVersion_ShouldGetNull()
        {
            var version = _versionFile.GetVersion();
            Assert.IsNull(version);
        }

        [Test]
        public void GetVersion_ShouldGetVersionFromFile()
        {
            File.WriteAllText(_versionFilePath, "2.4.5.2");

            var version = _versionFile.GetVersion();
            Assert.AreEqual("2.4.5.2", version);
        }

        [Test]
        public void Increment_ShouldInitializeVersionFile()
        {
            _versionFile.Increment();
            Assert.IsTrue(File.Exists(_versionFilePath));
            Assert.AreEqual("1.0.0.0", File.ReadAllText(_versionFilePath));
        }

        [Test]
        public void Increment_ShouldIncrementRevision()
        {
            File.WriteAllText(_versionFilePath, "2.3.5.4");

            _versionFile.Increment();
            Assert.AreEqual("2.3.5.5", File.ReadAllText(_versionFilePath));
        }

        [Test]
        public void Initialize_ShouldInitializeVersionFile()
        {
            _versionFile.Initialize();
            Assert.AreEqual("1.0.0.0", File.ReadAllText(_versionFilePath));
        }

        [Test]
        public void Initialize_ShouldDoNothing()
        {
            File.WriteAllText(_versionFilePath, "2.3.4.6");

            _versionFile.Initialize();
            Assert.AreEqual("2.3.4.6", File.ReadAllText(_versionFilePath));
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_versionFilePath))
                File.Delete(_versionFilePath);
        }
    }
}