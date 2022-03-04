using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SinSwayObjects: LoopedBoardReaders, ISway
{
    [SerializeField] float movespeed = 2f, sinwidth = 0.2f, sinspeed = 1;
    private float t;



    public override void Update(){
        t += Time.deltaTime;
        base.Update();
        Sway();
    }
    public async void Sway(){
        
        float  displacement = BoardReading(); // normalised
        //Todo: change from measuring where centre of mass is, to measure movement of centre of mass
        float  sinwave = movespeed + (sinwidth * Mathf.Sin( sinspeed * t ));
        Debug.Log ("Displacelemnt : " + displacement);
        float sin_displacement = displacement * sinwave;
        Debug.Log("movement overall speed : "+ sin_displacement);

        foreach (var target in targets)
        {
            target.position += new Vector3( 0,0 , sin_displacement ); // Time.deltaTime? 
        }
    }
}