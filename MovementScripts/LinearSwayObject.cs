using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LinearSwayObject : LoopedBoardReader, ISway
{
    public override void Update(){
        base.Update();
        Sway();
    }
    public void Sway(){
        transform.position += new Vector3(0,0, BoardReading() * Time.deltaTime);

    }

}