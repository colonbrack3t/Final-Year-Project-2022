using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopedmovement : MonoBehaviour
{
    [SerializeField] float movespeed = 0.1f, start = -100, end = 100, direction = 1;
    private Vector3 start_pos;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = new Vector3(0, 0, start);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.z * direction > end * direction ){
            transform.position = start_pos;
        }
        Vector3 move = new Vector3(0,0, direction* movespeed)*Time.deltaTime; 
        transform.position += move;
    }
    public void increaseMovespeed(){
        movespeed+=0.1f;
    }
    public void decreaseMovespeed(){
        movespeed-=0.1f;
        if (movespeed < 0)
         movespeed = 0;
    }
}
