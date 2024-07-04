## 简介

可用于缩小图片生成的PDF文件大小。

使用方法：
```
.\PdfCompressor.exe .\test.pdf -c 0.2
```

## 参数说明

```txt  
PdfCompressor.exe <PdfFile>

  -e, --exportImage    导出图片到output目录。
  -o, --output         输出文件名。
  -c, --compression    (Default: 0.5) 设置图像压缩率 (0.1 ~ 0.9).
```