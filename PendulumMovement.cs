using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMovement : MonoBehaviour
{

    [SerializeField] Transform camera;
    Vector3 prev_cam_pos = new Vector3(0,0,0);
    [SerializeField] Vector3 pivot = new Vector3(0,0,0);
    [SerializeField] float sensitivity = 1;
    // Start is called before the first frame update
    void Start()
    {
        prev_cam_pos = camera.position;
    }
    float getAngleChangeOnZAxis(){
        Debug.Log(prev_cam_pos);
         Debug.Log(camera.position);
        return Vector3.SignedAngle(pivot - prev_cam_pos, pivot - camera.position, Vector3.forward);
    }
    float getAngleChangeOnXAxis(){
        return Vector3.SignedAngle(pivot - prev_cam_pos, pivot - camera.position, Vector3.right);
    }
    // Update is called once per frame

    float getOverallAngleXAxis(){
        return Vector3.SignedAngle(Vector3.up, camera.position, Vector3.right);
    }
    float getOverallAngleZAxis(){
        return Vector3.SignedAngle(Vector3.up, camera.position, Vector3.forward);
    }
    void FixedUpdate()
    {   //pivot = camera.position/2f;
        Vector3 head_v = pivot - camera.position, prev_v = pivot - prev_cam_pos;
        Debug.Log( camera.position);
        Vector3 axis = Vector3.Cross(head_v, prev_v);
        float rotation = Vector3.SignedAngle(prev_v, head_v, axis);
        
        transform.RotateAround(transform.position, axis, rotation * sensitivity);
        prev_cam_pos = camera.position;
        /*float z_angle = getAngleChangeOnZAxis();
        float x_angle = getAngleChangeOnXAxis();

        transform.RotateAround(transform.position, Vector3.forward, z_angle * sensitivity);
       // transform.RotateAround(transform.position, Vector3.right, x_angle * sensitivity);
        prev_cam_pos = camera.position;
    */}
}
