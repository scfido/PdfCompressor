using CommandLine;

namespace Bering.OneApp.AgiService.Host.Cli
{
    [Verb("run", isDefault: true, HelpText = "运行")]
    public class RunOptions
    {
        [Value(0, Required = true, HelpText = "输入文件名。")]
        public string InputFile { get; set; } = "";

        [Option('e', "exportImage", HelpText = "导出图片到output目录。")]
        public bool ExportImage { get; set; }

        [Option('o', "output", HelpText = "输出文件名。")]
        public string? OutputFile { get; set; }

        [Option('c', "compression", Required = false, HelpText = "设置图像压缩率 (0.1 ~ 0.9).", Default = 0.5f)]
        public double CompressionRate { get; set; }

    }
}
