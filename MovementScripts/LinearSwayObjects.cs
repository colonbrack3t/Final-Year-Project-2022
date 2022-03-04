using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LinearSwayObjects : LoopedBoardReader, ISway
{

    
    [SerializeField] float speed = 5f;


    public override void Update(){
        base.Update();
        Sway();
    }
    public void Sway(){
        foreach(var target in targets)
            target.position += new Vector3(0,0, BoardReading() * speed * Time.deltaTime);

    }

}