using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RandomBitmap : MonoBehaviour
{
  /*
  The idea for this code was to use small pixel sized points on the unity canvas to use as our peripheral vision
  this was to reduce processing power of multiple 3d objects
  The idea was scrapped as these particles move with the headset, therefore do not feel as though they are in the peripheral vision
  but instead in the foreground
    
  */
  public UnityEngine.UI.Image img;
    // Start is called before the first frame update
    void Start()
    {
      img.sprite = CreateRandomBitmapCircle();

    }
//Creates a sprite which is 1024x1024 squares, each randomly either black or white
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
    //creates a sprite 1024x1024 squares each randomly black or white, but with a circle hole in the middle which the user will be able to see through
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
