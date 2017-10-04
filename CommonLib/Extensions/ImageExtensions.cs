﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CommonLib.Extensions {
  public static class ImageExtensions {
    public static byte[] ToBytes(this Image image) {
      return image.ToBytes(image.RawFormat);
    }

    public static byte[] ToBytes(this Image image, ImageFormat imageFormat) {
      using (var ms = new MemoryStream()) {
        image.Save(ms, imageFormat);
        return ms.ToArray();
      }
    }

    // see: https://stackoverflow.com/a/24199315
    public static Image Resize(this Image image, int width, int height) {
      var destRect = new Rectangle(0, 0, width, height);
      var destImage = new Bitmap(width, height);

      destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

      using (var graphics = Graphics.FromImage(destImage)) {
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using (var wrapMode = new ImageAttributes()) {
          wrapMode.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }
      }

      return destImage;
    }
  }
}