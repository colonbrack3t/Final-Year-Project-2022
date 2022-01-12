using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwayCube : MonoBehaviour
{
    public Text HUD;

    public Transform head;
    public float head_modifier = 20f, board_modifier = 0.5f, maxVelocity =5;
    public Transform ankleJoint;
    public BalanceBoardSensor bbs;
    Vector3 startingPosition;
    float sqrMaxVelocity;
    Rigidbody r;
    // Start is called before the first frame update
    void Start()
    {sqrMaxVelocity = maxVelocity*maxVelocity;
        r = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //float  angle = modifier * Vector3.SignedAngle(head.position-ankleJoint.position, ankleJoint.up, Vector3.right);
        //Debug.Log(startingPosition);
        float dist = (head.position.z - ankleJoint.position.z);
        
        double top = bbs.rwTopLeft + bbs.rwTopRight;
        double btm = bbs.rwBottomLeft + bbs.rwBottomRight;
        double displacement = top - btm;
        dist *= head_modifier;
        displacement *= board_modifier;
        transform.position = startingPosition +  new Vector3(0,0,(float)(dist + displacement));
        
        HUD.text = "dist : " + dist/head_modifier + "\nboard : " + displacement/board_modifier + "\n mod vals: "+ (float)(dist + displacement);
    }
}
