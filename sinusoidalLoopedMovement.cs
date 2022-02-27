using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sinusoidalLoopedMovement : MonoBehaviour
{
    [SerializeField] float movespeed = 2f, sinwidth = 0.2f, sinspeed = 1, start = -100, end = 100, direction = 1;
    [SerializeField] Text text;
    private Vector3 start_pos;
    private float t = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        start_pos = new Vector3(0, 0, start);
    }

    // Update is called once per frame
    void Update()
    {
        //use time as x value for sin
        t += Time.deltaTime;
        
        // loop tunnel position
        if (transform.position.z * direction > end * direction ){
            transform.position = start_pos;
        }
        //float sinwave = movespeed + 0.5f * Mathf.Pow( Mathf.Sqrt(0.7f) + (Mathf.Sqrt(sinwidth) * Mathf.Sin( t )),2);
        
        //copmute sin equation
        float sinwave = movespeed + (sinwidth * Mathf.Sin( sinspeed * t ));
        

        //ddebug outputs
        Debug.Log(sinwave);
        
        text.text = "movespeed : " + movespeed + "\nsinwidth : " + sinwidth + "\nsinspeed : " + sinspeed;
        
        //update position
        Vector3 move = new Vector3(0,0, direction * sinwave) * Time.deltaTime; 
        transform.position += move;
    }
    // Set values in realtime
    public void increaseBaseSpeed(){
        movespeed+=0.1f;
    }
    public void decreaseBaseSpeed(){
        movespeed-=0.1f;
        if (movespeed < 0)
         movespeed = 0;
    }
    public void increaseSinWidth(){
        sinwidth+=0.1f;
    }
    public void decreaseSinWidth(){
        sinwidth-=0.1f;
        if (sinwidth < 0)
         sinwidth = 0;
    }
    public void increaseSinspeed(){
        sinspeed+=0.1f;
    }
    public void decreaseSinspeed(){
        sinspeed-=0.1f;
        if (sinspeed < 0)
         sinspeed = 0;
    }
}

