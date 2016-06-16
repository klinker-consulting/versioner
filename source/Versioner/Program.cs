using System;
using System.Collections.Generic;

namespace Versioner
{
    public class Program
    {
        private readonly IVersionFileFactory _versionFileFactory;
        private readonly IOutput _output;
        private Dictionary<string, Action<IVersionFile>> _commands;

        public Program()
            : this(new VersionFileFactory(), new ConsoleOutput())
        {
            
        }

        public Program(IVersionFileFactory versionFileFactory, IOutput output)
        {
            _versionFileFactory = versionFileFactory;
            _output = output;
            _commands = new Dictionary<string, Action<IVersionFile>>
            {
                {"init", Initialize},
                {"increment", Increment},
                {"get", Get}
            };
        }

        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run(args);
        }

        public void Run(string[] args)
        {
            if (args.Length != 2)
            {
                WriteInvalid();
                return;
            }
            var command = args[0].ToLowerInvariant();
            if (!IsKnownCommand(command))
                WriteUknownCommand(command);
            else
                ExecuteCommand(command, args[1]);
        }

        private void WriteInvalid()
        {
            _output.Write("You must supply a command and a file path.");
        }

        private static void Initialize(IVersionFile versionFile)
        {
            versionFile.Initialize();
        }

        private static void Increment(IVersionFile versionFile)
        {
            versionFile.Increment();
        }

        private void Get(IVersionFile versionFile)
        {
            var version = versionFile.GetVersion();
            _output.Write(version);
        }

        private bool IsKnownCommand(string command)
        {
            return _commands.ContainsKey(command);

        }

        private void WriteUknownCommand(string command)
        {
            _output.Write($"{command} is an unknown command. Please use {string.Join(", ", _commands.Keys)}.");
        }

        private void ExecuteCommand(string command, string filePath)
        {
            var versionFile = _versionFileFactory.Create(filePath);
            _commands[command].Invoke(versionFile);
        }
    }
}
