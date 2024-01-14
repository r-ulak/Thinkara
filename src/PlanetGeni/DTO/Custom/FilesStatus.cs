using Common;
using System;
using System.Drawing;
using System.IO;
public class FilesStatus
{

    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Url { get; set; }
    public string ThumbnailUrl { get; set; }

    public FilesStatus()
    {
    }

    public FilesStatus(string fileName, string source, string originalName)
    {
        SetValues(fileName, source,originalName);
    }

    private void SetValues(string fileName, string source, string originalName)
    {
        Url = AppSettings.ImageUploadPath + source + "\\" + fileName;
        Name = fileName;
        OriginalName = originalName;
    }

    private bool IsImage(string ext)
    {
        return ext == ".gif" || ext == ".jpg" || ext == ".png";
    }


    private static double ConvertBytesToMegabytes(long bytes)
    {
        return (bytes / 1024f) / 1024f;
    }
}