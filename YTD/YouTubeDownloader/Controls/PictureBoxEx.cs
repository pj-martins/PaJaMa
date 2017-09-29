// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.PictureBoxEx
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Controls
{
    public class PictureBoxEx : PictureBox
    {
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.BackgroundImage == null)
                return;
            Rectangle rect = this.ImageRectangleFromSizeMode(this.BackgroundImage, this.SizeMode);
            pevent.Graphics.DrawImage(this.BackgroundImage, rect);
        }

        private Rectangle ImageRectangleFromSizeMode(Image image, PictureBoxSizeMode mode)
        {
            Rectangle rectangle = PictureBoxEx.DeflateRect(this.ClientRectangle, this.Padding);
            if (image != null)
            {
                switch (mode)
                {
                    case PictureBoxSizeMode.Normal:
                    case PictureBoxSizeMode.AutoSize:
                        rectangle.Size = image.Size;
                        return rectangle;
                    case PictureBoxSizeMode.StretchImage:
                        return rectangle;
                    case PictureBoxSizeMode.CenterImage:
                        rectangle.X += (rectangle.Width - image.Width) / 2;
                        rectangle.Y += (rectangle.Height - image.Height) / 2;
                        rectangle.Size = image.Size;
                        return rectangle;
                    case PictureBoxSizeMode.Zoom:
                        Size size = image.Size;
                        double num1 = (double)this.ClientRectangle.Width / (double)size.Width;
                        Rectangle clientRectangle = this.ClientRectangle;
                        double num2 = (double)clientRectangle.Height / (double)size.Height;
                        float num3 = Math.Min((float)num1, (float)num2);
                        rectangle.Width = (int)((double)size.Width * (double)num3);
                        rectangle.Height = (int)((double)size.Height * (double)num3);
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        Rectangle local1 = @rectangle;
                        clientRectangle = this.ClientRectangle;
                        int num4 = (clientRectangle.Width - rectangle.Width) / 2;
                        // ISSUE: explicit reference operation
                        (local1).X = num4;
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        Rectangle local2 = @rectangle;
                        clientRectangle = this.ClientRectangle;
                        int num5 = (clientRectangle.Height - rectangle.Height) / 2;
                        // ISSUE: explicit reference operation
                        (local2).Y = num5;
                        return rectangle;
                }
            }
            return rectangle;
        }

        public static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        public static Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
            {
                ColorMatrix newColorMatrix = new ColorMatrix()
                {
                    Matrix33 = opacity
                };
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            return (Image)bitmap;
        }
    }
}
