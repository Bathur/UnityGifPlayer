//============================================
//
//Copyright (C) Bathur Lu All rights reserved.
//
//Author:   Bathur Lu
//Date:     2019.3.9
//Website:  http://bathur.cn/
//
//============================================

using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityImage = UnityEngine.UI.Image;
using DrawingImage = System.Drawing.Image;

public class SingleGifPlayer : MonoBehaviour
{
    public enum RenderMode
    {
        UGUIImage,
        MaterialOverride
    }

    #region Member Variables
    public string gifPath;
    public RenderMode renderMode;
    public UnityImage targetImage;
    public Renderer targetRenderer;

    private List<Texture2D> texture2DList;
    private float[] frameDelay;
    private Sprite[] sprites;
    private Bitmap bitmap;

    private float time = 0;
    private int index = 0;
    #endregion

    void Start()
    {
        DrawingImage image = DrawingImage.FromFile(Application.streamingAssetsPath + gifPath);
        GifPlayerBase.Gif2Texture2DWithFrameDelay(image, out texture2DList, out frameDelay);
        if (renderMode == RenderMode.UGUIImage)
        {
            sprites = new Sprite[texture2DList.Count];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i]= Sprite.Create(
                        texture2DList[i],
                        new Rect(0, 0, texture2DList[i].width, texture2DList[i].height),
                        new Vector2(0.5f, 0.5f));
            }
        }
    }

    void Update()
    {
        if (texture2DList.Count > 0)
        {
            time += Time.deltaTime;
            if (time > frameDelay[index])
            {
                time = 0;
                index = index + 1 >= frameDelay.Length ? 0 : index + 1;
            }

            switch (renderMode)
            {
                case RenderMode.MaterialOverride:
                    targetRenderer.material.mainTexture = texture2DList[index];
                    break;
                case RenderMode.UGUIImage:
                    targetImage.sprite = sprites[index];
                    break;
            }
        }
    }
}
