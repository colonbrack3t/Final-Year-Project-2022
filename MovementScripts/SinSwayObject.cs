using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SinSwayObject: LoopedBoardReader, ISway
{
    [SerializeField] float movespeed = 2f, sinwidth = 0.2f, sinspeed = 1;
    private float t;
    public override void Update(){
        t += Time.deltaTime;
        base.Update();
        Sway();
    }
    public void Sway(){

        float  displacement = BoardReading(); // normalise
        float  sinwave = movespeed + (sinwidth * Mathf.Sin( sinspeed * t ));
        Debug.Log ("Displacelemnt : " + displacement);
        float sin_displacement = displacement * sinwave;
        Debug.Log("movement overall speed : "+ sin_displacement);
        transform.position += new Vector3( 0,0 , sin_displacement * Time.deltaTime);
    }
}