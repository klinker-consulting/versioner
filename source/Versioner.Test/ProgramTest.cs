using System;
using Moq;
using NUnit.Framework;

namespace Versioner.Test
{
    [TestFixture]
    public class ProgramTest
    {
        private Program _program;
        private Mock<IVersionFileFactory> _versionFileFactoryMock;
        private Mock<IVersionFile> _versionFileMock;
        private Mock<IOutput> _outputMock;
        private string _path;

        [SetUp]
        public void Setup()
        {
            _path = Guid.NewGuid().ToString();

            _versionFileMock = new Mock<IVersionFile>();

            _versionFileFactoryMock = new Mock<IVersionFileFactory>();
            _versionFileFactoryMock.Setup(s => s.Create(_path))
                .Returns(_versionFileMock.Object);

            _outputMock = new Mock<IOutput>();
            _program = new Program(_versionFileFactoryMock.Object, _outputMock.Object);
        }

        [Test]
        public void RunWithCommandAndFilePath_ShouldCreateVersionFile()
        {
            _program.Run(new []{"get", _path});
            _versionFileFactoryMock.Verify(s => s.Create(_path), Times.Once());
        }

        [Test]
        public void RunWithInitAndFilePath_ShouldInitializeVersionFile()
        {
            _program.Run(new[] {"init", _path });
            
            _versionFileMock.Verify(s => s.Initialize(), Times.Once());
        }

        [Test]
        public void RunWithIncrementAndFilePath_ShouldIncrementVersionFile()
        {
            _program.Run(new [] {"increment", _path});
            _versionFileMock.Verify(s => s.Increment(), Times.Once());
        }

        [Test]
        public void RunWithGetAndFilePath_ShouldGetVersion()
        {
            _versionFileMock.Setup(s => s.GetVersion()).Returns("5.4.2.1");

            _program.Run(new []{"get", _path});
            _outputMock.Verify(s => s.Write("5.4.2.1"), Times.Once());
        }

        [Test]
        public void RunWithUnknownCommand_ShouldWriteUnknownCommand()
        {
            
            _program.Run(new [] {_path, _path});
            _outputMock.Verify(s => s.Write($"{_path} is an unknown command. Please use init, increment, get."));
        }

        [Test]
        public void RunWithOneParameter_ShouldWriteCommandAndPathIsRequired()
        {
            _program.Run(new []{"not good"});
            _outputMock.Verify(s => s.Write("You must supply a command and a file path."), Times.Once());
        }

        [Test]
        public void RunWithOddCaseCommand_ShouldWork()
        {
            _program.Run(new []{"INIT", _path});
            _versionFileMock.Verify(s => s.Initialize(), Times.Once());
        }
    }
}
