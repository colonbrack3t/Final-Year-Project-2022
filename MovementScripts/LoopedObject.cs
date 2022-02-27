using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoopedObject : MonoBehaviour
{
private Vector3 start_pos, end_pos;
[SerializeField] protected float start = -100, end = 100, direction = 1;
    public virtual void Start()
    {
        start_pos = new Vector3(0, 0, start);
        end_pos = new Vector3(0, 0, end);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (transform.position.z * direction > end * direction ){
            transform.position = start_pos;
        }
        if (transform.position.z * direction < start * direction ){
            transform.position = end_pos;
        }
        
    }
}