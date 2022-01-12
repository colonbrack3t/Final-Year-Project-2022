using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RandomBitmap : MonoBehaviour
{
    public UnityEngine.UI.Image img;
    // Start is called before the first frame update
    void Start()
    {
img.sprite = CreateRandomBitmapCircle();

        
    }
    void FixedUpdate()
    {
        
    }
    Sprite CreateRandomBitmap()
    {
        int x_len = 1024, y_len = 1024;
        Texture2D bitmap = new Texture2D(x_len, y_len);
        bitmap.filterMode = FilterMode.Point;
        for (int i = 0; i < x_len; i++)
        {
            for (int j = 0; j < y_len; j++)
            {
                bitmap.SetPixel(i, j, UnityEngine.Random.Range(0f, 100f) > 50f ? Color.black : Color.white);
            }
        }
        bitmap.Apply();

        return Sprite.Create(bitmap, new Rect(0.0f, 0.0f, x_len, y_len), new Vector2(0, 0));

    }
    Sprite CreateRandomBitmapCircle()
    {
        int x_len = 1024, y_len = 1024;
        int centery = y_len / 2, centerx = x_len / 2;
        int r_squared = (int)Math.Pow(300,2);
        Texture2D bitmap = new Texture2D(x_len, y_len);
        bitmap.filterMode = FilterMode.Point;
        for (int i = 0; i < x_len; i++)
        {
            for (int j = 0; j < y_len; j++)
            {
                int x = i - centerx;
                int y = j - centery;

                if (Math.Pow(y,2) + Math.Pow(x,2) < r_squared)
                {
                    bitmap.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
                else
                {
                    bitmap.SetPixel(i, j, UnityEngine.Random.Range(0f, 100f) > 50f ? Color.black : Color.white);
                }
            }
        }



        bitmap.Apply();

        return Sprite.Create(bitmap, new Rect(0.0f, 0.0f, x_len, y_len), new Vector2(0, 0));

    }

}
