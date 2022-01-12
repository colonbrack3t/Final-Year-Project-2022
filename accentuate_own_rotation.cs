using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accentuate_own_rotation : MonoBehaviour
{
    Vector3 prev = new Vector3(0, 0, 0);
    public float factor = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles != prev)
        {
            transform.eulerAngles*= factor;
            prev = transform.eulerAngles;
        }

    }
}
