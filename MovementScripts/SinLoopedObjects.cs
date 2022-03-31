using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SinLoopedObjects: LoopedObjects
{
    [SerializeField] float movespeed = 2f, sinwidth = 0.2f, sinspeed = 1;
    [SerializeField] Text text;
    private float t;




    public override void Update(){
        base.Update();
        
        sin_move();
    }
    private void sin_move(){
        t += Time.deltaTime;
        //copmute sin equation
        float sinwave = movespeed + (sinwidth * Mathf.Sin( sinspeed * t ));
        

        //ddebug outputs
        Debug.Log("Sinwave: " + sinwave);
        
        text.text = "movespeed : " + movespeed + "\nsinwidth : " + sinwidth + "\nsinspeed : " + sinspeed;
        
        //update position
        Vector3 move = new Vector3(0,0, direction * sinwave) * Time.deltaTime; 
        foreach (var target in targets)
        target.position += move;
    }

}