using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoopedObjects : MonoBehaviour
{
private Vector3 start_pos, end_pos;


    

[SerializeField] protected float start = -100, end = 100, direction = 1;

    [SerializeField] protected Transform[] targets;
    public virtual void Start()
    {
        start_pos = new Vector3(0, 0, start);
        end_pos = new Vector3(0, 0, end);
        if (targets == null|| targets.Length == 0)
            targets = new Transform[]{this.transform};
    }

    // Update is called once per frame
    public virtual void Update()
    {
        foreach (var target in targets){
        if (target.position.z * direction > end * direction ){
            target.position = start_pos;
        }
        if (target.position.z * direction < start * direction ){
            target.position = end_pos;
        }}
        
    }
}