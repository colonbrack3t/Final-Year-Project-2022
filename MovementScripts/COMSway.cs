using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class COMSway: LoopedCOM, ISway{

    [SerializeField] float movespeed = 2f, sinwidth = 0.2f, sinspeed = 1;
    private float t;
    public float sinwave_position = 0;
  public override void Update(){
        t += Time.deltaTime;
        base.Update();
        Sway();
    }
    public async void Sway() {
        Vector2 board_vec = BoardReading();
        
        Vector3 vec = new Vector3(board_vec.x, 0 , board_vec.y);
        vec += HeadReading();
        float  sinwave = movespeed + (sinwidth * Mathf.Sin( sinspeed * t ));
        sinwave_position = sinwave;
        vec *= sinwave;
        vec = Vector3.Scale(vec, (new Vector3(1 , 0 , 1)));
        foreach (var target in targets)
        {
            target.position += vec;
        }
    }
    public void recentre (){
        foreach (var target in targets)
        {
            target.position = head.position;
        }
    }
}