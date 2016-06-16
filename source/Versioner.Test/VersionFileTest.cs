using System;
using System.IO;
using NUnit.Framework;

namespace Versioner.Test
{
    [TestFixture]
    public class VersionFileTest
    {
        private string _versionFileKey;
        private VersionFile _versionFile;

        [SetUp]
        public void Setup()
        {
            _versionFileKey = Guid.NewGuid().ToString();
            _versionFile = new VersionFile(_versionFileKey);
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
            File.WriteAllText(_versionFile.Path, "2.4.5.2");

            var version = _versionFile.GetVersion();
            Assert.AreEqual("2.4.5.2", version);
        }

        [Test]
        public void Increment_ShouldInitializeVersionFile()
        {
            _versionFile.Increment();
            Assert.IsTrue(File.Exists(_versionFile.Path));
            Assert.AreEqual("1.0.0.0", File.ReadAllText(_versionFileKey));
        }

        [Test]
        public void Increment_ShouldIncrementRevision()
        {
            File.WriteAllText(_versionFile.Path, "2.3.5.4");

            _versionFile.Increment();
            Assert.AreEqual("2.3.5.5", File.ReadAllText(_versionFile.Path));
        }

        [Test]
        public void Initialize_ShouldInitializeVersionFile()
        {
            _versionFile.Initialize();
            Assert.AreEqual("1.0.0.0", File.ReadAllText(_versionFile.Path));
        }

        [Test]
        public void Initialize_ShouldDoNothing()
        {
            File.WriteAllText(_versionFile.Path, "2.3.4.6");

            _versionFile.Initialize();
            Assert.AreEqual("2.3.4.6", File.ReadAllText(_versionFileKey));
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_versionFile.Path))
                File.Delete(_versionFile.Path);
        }
    }
}