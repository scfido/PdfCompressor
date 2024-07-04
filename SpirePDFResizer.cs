using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Utilities;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CompressPdf;

public class SpirePDFResizer
{
    public static void ResizeImage(string inputFilePath, string? outputFilePath, double scaleRatio, bool exportImage = false)
    {

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("文件不存在");
            return;
        }

        // 创建 PdfDocument 对象并加载 PDF 文件
        PdfDocument inputDoc = new PdfDocument(inputFilePath);
        PdfDocument outputDoc = new PdfDocument();

        PdfImageHelper pdfImageHelper = new PdfImageHelper();
        var pageIndex = 0;
        foreach (PdfPageBase page in inputDoc.Pages)
        {
            pageIndex++;
            Console.WriteLine($"正在处理第{pageIndex}页。");

            // 获取页面中的所有图片
            var imageInfos = pdfImageHelper.GetImagesInfo(page);
            foreach (var imageInfo in imageInfos)
            {
                Console.WriteLine($"第{pageIndex}页图片大小{imageInfo.Image.Size}。旋转：{page.Rotation}");

                if (exportImage)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo("output");
                    if (!directoryInfo.Exists)
                        Directory.CreateDirectory("output");

                    imageInfo.Image.Save($"output\\page{pageIndex:000}.png");
                }

                // 缩小图片
                Image newImage = ResizeImage(imageInfo.Image, scaleRatio);
                newImage.Save($"output\\page{pageIndex:000}-s.png");

                var newPage = outputDoc.Pages.Add(PdfPageSize.A4, new PdfMargins(0));
                WirteImage(newImage, newPage);
            }
        }

        inputDoc.Close();
        // 保存更改后的 PDF 文件

        if (outputFilePath == null)
        {
            outputFilePath = $"{Path.GetFileNameWithoutExtension(inputFilePath)}-压缩{Path.GetExtension(inputFilePath)}";
        }
        outputDoc.SaveToFile(outputFilePath);
        outputDoc.Close();
    }

    private static void WirteImage(Image image, PdfPageBase page)
    {
        // 加载图片
        PdfImage pdfImage = PdfImage.FromImage(image);

        // 计算图片的缩放比例
        float pageWidth = page.Canvas.ClientSize.Width;
        float pageHeight = page.Canvas.ClientSize.Height;
        float imageWidth = pdfImage.Width;
        float imageHeight = pdfImage.Height;

        float widthScale = pageWidth / imageWidth;
        float heightScale = pageHeight / imageHeight;
        float scale = Math.Min(widthScale, heightScale);

        float scaledWidth = imageWidth * scale;
        float scaledHeight = imageHeight * scale;

        // 居中绘制图片
        float x = (pageWidth - scaledWidth) / 2;
        float y = (pageHeight - scaledHeight) / 2;
        page.Canvas.DrawImage(pdfImage, x, y, scaledWidth, scaledHeight);
    }

    private static Image ResizeImage(Image imgToResize, double scaleRatio)
    {
        int destWidth = (int)(imgToResize.Width * scaleRatio);
        int destHeight = (int)(imgToResize.Height * scaleRatio);

        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage(b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return b;
    }
}
