﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// http://stackoverflow.com/questions/2368757/easy-way-to-convert-a-bitmap-and-png-image-to-text-and-vice-versa
    /// http://www.developerfusion.com/thread/47978/how-can-i-convert-image-type-to-bitmap-type-in-cnet/
    /// http://stackoverflow.com/questions/10442269/scaling-a-system-drawing-bitmap-to-a-given-size-while-maintaining-aspect-ratio
    public static class EmBitmap
    {
        public static string EncodeToString(this Bitmap bitmap)
        {
            if ( bitmap == null )
                return String.Empty;

            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            byte[] bitmapBytes = memoryStream.GetBuffer();
            return Convert.ToBase64String(bitmapBytes, Base64FormattingOptions.InsertLineBreaks);            
        }

        public static Bitmap FromEncodedString(string bitmapString)
        {
            byte[] bitmapBytes = Convert.FromBase64String(bitmapString);
            MemoryStream memoryStream = new MemoryStream(bitmapBytes);
            return new Bitmap(Image.FromStream(memoryStream));            
        }

        public static Bitmap Resize(this Bitmap bitmap, int width)
        {
            double height = width * ((double)bitmap.Height / bitmap.Width);
            return new Bitmap(bitmap, new Size(width, (int)height));
        }


        public static Icon ToIcon(this Bitmap bitmap)
        {
            return Icon.FromHandle(bitmap.GetHicon());
        }

        public static Bitmap ToBitmap(this Image image)
        {
            return (Bitmap) image;
        }

        public static Icon ToIcon(this Image image)
        {
            return Icon.FromHandle(((Bitmap)image).GetHicon());            
        }
    }
    

    public class ImageHelper
    {
        #region CropUnwantedBackground
        public static Bitmap CropUnwantedBackground(Bitmap bmp)
        {
            var backColor = GetMatchedBackColor(bmp);
            if (backColor.HasValue)
            {
                var bounds = GetImageBounds(bmp, backColor);
                var diffX = bounds[1].X - bounds[0].X + 1;
                var diffY = bounds[1].Y - bounds[0].Y + 1;
                var croppedBmp = new Bitmap(diffX, diffY);
                var g = Graphics.FromImage(croppedBmp);
                var destRect = new Rectangle(0, 0, croppedBmp.Width, croppedBmp.Height);
                var srcRect = new Rectangle(bounds[0].X, bounds[0].Y, diffX, diffY);
                g.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
                return croppedBmp;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private Methods

        #region GetImageBounds
        private static Point[] GetImageBounds(Bitmap bmp, Color? backColor)
        {
            //--------------------------------------------------------------------
            // Finding the Bounds of Crop Area bu using Unsafe Code and Image Proccesing
            Color c;
            int width = bmp.Width, height = bmp.Height;
            bool upperLeftPointFounded = false;
            var bounds = new Point[2];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    c = bmp.GetPixel(x, y);
                    bool sameAsBackColor = ((c.R <= backColor.Value.R * 1.1 && c.R >= backColor.Value.R * 0.9) &&
                                            (c.G <= backColor.Value.G * 1.1 && c.G >= backColor.Value.G * 0.9) &&
                                            (c.B <= backColor.Value.B * 1.1 && c.B >= backColor.Value.B * 0.9));
                    if (!sameAsBackColor)
                    {
                        if (!upperLeftPointFounded)
                        {
                            bounds[0] = new Point(x, y);
                            bounds[1] = new Point(x, y);
                            upperLeftPointFounded = true;
                        }
                        else
                        {
                            if (x > bounds[1].X)
                                bounds[1].X = x;
                            else if (x < bounds[0].X)
                                bounds[0].X = x;
                            if (y >= bounds[1].Y)
                                bounds[1].Y = y;
                        }
                    }
                }
            }
            return bounds;
        }
        #endregion

        #region GetMatchedBackColor
        private static Color? GetMatchedBackColor(Bitmap bmp)
        {
            // Getting The Background Color by checking Corners of Original Image
            var corners = new Point[]{
            new Point(0, 0),
            new Point(0, bmp.Height - 1),
            new Point(bmp.Width - 1, 0),
            new Point(bmp.Width - 1, bmp.Height - 1)
        }; // four corners (Top, Left), (Top, Right), (Bottom, Left), (Bottom, Right)
            for (int i = 0; i < 4; i++)
            {
                var cornerMatched = 0;
                var backColor = bmp.GetPixel(corners[i].X, corners[i].Y);
                for (int j = 0; j < 4; j++)
                {
                    var cornerColor = bmp.GetPixel(corners[j].X, corners[j].Y);// Check RGB with some offset
                    if ((cornerColor.R <= backColor.R * 1.1 && cornerColor.R >= backColor.R * 0.9) &&
                        (cornerColor.G <= backColor.G * 1.1 && cornerColor.G >= backColor.G * 0.9) &&
                        (cornerColor.B <= backColor.B * 1.1 && cornerColor.B >= backColor.B * 0.9))
                    {
                        cornerMatched++;
                    }
                }
                if (cornerMatched > 2)
                {
                    return backColor;
                }
            }
            return null;
        }
        #endregion

        #endregion
    }
}
