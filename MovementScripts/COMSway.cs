using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class COMSway: LoopedCOM, ISway{
  public override void Update(){
        //t += Time.deltaTime;
        base.Update();
        Sway();
    }
    public async void Sway() {
        Vector3 vec = HeadReading() + BoardReading();
        
        foreach (var target in targets)
        {
            target.position += vec;
        }
    }
}