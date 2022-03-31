using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StroopTest : MonoBehaviour
{
    // Start is called before the first frame update
    string[] color_strings = new string[] { "Red", "Blue", "Yellow", "Purple", "Green", "Black" };
    Color[] colors = new Color[] { Color.red, Color.blue, Color.yellow, Color.magenta, Color.green, Color.black };
    [SerializeField] Text user_text, timer_text;
    [SerializeField] Image stroop_answer;
    public bool test_running = false;
  
    private float timer = 10000f;
    // Update is called once per frame

    void FixedUpdate()
    {
        if (test_running)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {

                test_running = false;
                user_text.text = "Test is over!";
            }
            timer_text.text = timer + "s";

        }
    }
    public void GenerateStroopTest()
    {
        if (test_running)
        {
            user_text.text = color_strings[Random.Range(0, 6)];
            user_text.color = colors[Random.Range(0, 6)];
            stroop_answer.color = user_text.color;
        }
    }
}
