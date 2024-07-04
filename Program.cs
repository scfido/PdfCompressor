using Bering.OneApp.AgiService.Host.Cli;
using CommandLine;

namespace CompressPdf
{
    class Program
    {
        static int Main(string[] args)
        {
            return Run(args);
        }


        public static int Run(string[] args)
        {
            var parser = new Parser(config =>
            {
                config.IgnoreUnknownArguments = true;
                config.HelpWriter = Console.Error;
            });

            return parser.ParseArguments<RunOptions, int>(args)
                 .MapResult(
                    (RunOptions opts) =>
                    {

                        if (!File.Exists(opts.InputFile))
                        {
                            Console.WriteLine($"输入的文件{opts.InputFile}不存在");
                            return -2;
                        }

                        SpirePDFResizer.ResizeImage(opts.InputFile, opts.OutputFile, opts.CompressionRate, opts.ExportImage);
                        return 0;
                    },

                    _ => -1
                );
        }
    }
}