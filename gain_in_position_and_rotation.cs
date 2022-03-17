using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gain_in_position_and_rotation : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] float gain;
    Quaternion prev_rot;
    Vector3 prev_pos;
    public bool gaining = false;
 
    void start_pendulum(){
        gaining = true;
        prev_pos = camera.position;
        prev_rot = camera.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (gaining){
            Gain();
            
        }
    }
    void Gain(){
        //gain in position
        transform.position += (camera.position - prev_pos) * gain;
        //transform.rotation *=  (prev_rot * Quaternion.Inverse(camera.rotation));
        Vector3 q1 = new Vector3(prev_rot.x,prev_rot.y,prev_rot.z);
        Vector3 q2 = new Vector3(camera.rotation.x,camera.rotation.y,camera.rotation.z);
        Vector3 axis = Vector3.Cross(q1,q2);
        float rotation = Vector3.SignedAngle(q1 , q2, axis);
        transform.RotateAround(transform.position, axis, rotation * gain);

        prev_pos = camera.position;
        prev_rot = camera.rotation;
    }
}
