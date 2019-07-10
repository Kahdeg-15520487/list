using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace list
{
    public class Options
    {
        [Value(0)]
        public string Directory { get; set; }

        [Value(1)]
        public string Extension { get; set; }

        [Option('r', "recursive", Required = false, HelpText = "list file recursively")]
        public bool IsRecursive { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err);
            }
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            if (string.IsNullOrWhiteSpace(opts.Directory))
            {
                Console.WriteLine("no target directory");
                return;
            }
            if (string.IsNullOrWhiteSpace(opts.Extension))
            {
                Console.WriteLine("no target file extension");
                return;
            }

            string[] exts = opts.Extension.Split("|");
            DirectoryInfo d = new DirectoryInfo(opts.Directory);
            foreach (var ext in exts)
            {
                FileInfo[] Files = d.GetFiles($"*.{ext}", opts.IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in Files)
                {
                    Console.WriteLine(file);
                }
            }
        }
    }
}
