using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StroopTest : MonoBehaviour
{
    // Start is called before the first frame update
    string[] color_strings = new string[]{"Red", "Blue", "Yellow", "Purple", "Green", "Black"};
    Color[] colors = new Color[]{Color.red, Color.blue, Color.yellow, Color.magenta, Color.green, Color.black};
    [SerializeField] Text user_text;
    [SerializeField] Image stroop_answer;
    // Update is called once per frame
    public void GenerateStroopTest(){
        user_text.text = color_strings[Random.Range(0, 6)];
        user_text.color = colors[Random.Range(0, 6)];
        stroop_answer.color = user_text.color;
    }
}
