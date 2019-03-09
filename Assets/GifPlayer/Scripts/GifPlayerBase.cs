//============================================
//
//Copyright (C) Bathur Lu All rights reserved.
//
//Author:   Bathur Lu
//Date:     2019.3.9
//Website:  http://bathur.cn/
//
//============================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

public static class GifPlayerBase
{
    public static void Gif2Texture2DWithFrameDelay(Image gifImage,out List<Texture2D> texture2D,out float[] frameDelay)
    {
        if (gifImage != null)
        {
            FrameDimension frameDimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            int frameCount = gifImage.GetFrameCount(frameDimension);

            texture2D = new List<Texture2D>();
            frameDelay = new float[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                gifImage.SelectActiveFrame(frameDimension, i);

                Bitmap framBitmap = new Bitmap(gifImage.Width, gifImage.Height);
                using (System.Drawing.Graphics newGraphics = System.Drawing.Graphics.FromImage(framBitmap))
                {
                    newGraphics.DrawImage(gifImage, Point.Empty);
                }
                Texture2D frameTexture2D = new Texture2D(framBitmap.Width, framBitmap.Height, TextureFormat.ARGB32, true);
                frameTexture2D.LoadImage(Bitmap2Byte(framBitmap));
                texture2D.Add(frameTexture2D);

                for (int j = 0; j < gifImage.PropertyIdList.Length; j++)
                {
                    if ((int)gifImage.PropertyIdList.GetValue(j) == 0x5100)
                    {
                        PropertyItem pItem = (PropertyItem)gifImage.PropertyItems.GetValue(j);
                        byte[] delayByte = new byte[4];
                        delayByte[0] = pItem.Value[i * 4];
                        delayByte[1] = pItem.Value[1 + i * 4];
                        delayByte[2] = pItem.Value[2 + i * 4];
                        delayByte[3] = pItem.Value[3 + i * 4];
                        int delayInMilliseconds = BitConverter.ToInt32(delayByte, 0) * 10;
                        frameDelay[i] = delayInMilliseconds / 1000f;
                        break;
                    }
                }
            }
        }
        else
        {
            texture2D = null;
            frameDelay = null;
        }
    }

    public static byte[] Bitmap2Byte(Bitmap bitmap)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            bitmap.Save(stream, ImageFormat.Png);
            byte[] data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            return data;
        }
    }
}
