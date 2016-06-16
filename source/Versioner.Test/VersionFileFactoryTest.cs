using System;
using NUnit.Framework;

namespace Versioner.Test
{
    [TestFixture]
    public class VersionFileFactoryTest
    {
        [Test]
        public void Create_ShouldCreateVersionFile()
        {
            var factory = new VersionFileFactory();
            var versionFile = factory.Create(Guid.NewGuid().ToString());
            Assert.IsInstanceOf<VersionFile>(versionFile);
        }
    }
}
