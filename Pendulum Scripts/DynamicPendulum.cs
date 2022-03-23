using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPendulum : MonoBehaviour
{   // headset camera
    [SerializeField] Transform camera;
    [SerializeField] Transform pivot_ball;
    //tracking of previous position
    Vector3[] prev_cam_pos = new Vector3[3];
    
    [SerializeField] bool start = false;


    //sensitivity - 1 = 100% extra rotation (ie doubling percieved motion)
    [SerializeField] public float sensitivity = 1;
    // Start is called before the first frame update
    async void Start()
    {
        for (int i = 0 ; i < prev_cam_pos.Length; i++){
            prev_cam_pos[i] = camera.position + new Vector3(1,1, (i));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(start){
        //find origin of sphere 
        Vector3 pivot = SphereOrigin(prev_cam_pos, camera.position);
        //Construct vectors for new and last position
        Vector3 head_v = pivot - camera.position, prev_v = pivot - prev_cam_pos[2];
        pivot_ball.position = pivot;
        //find perpendicular axis
        Vector3 axis = Vector3.Cross(head_v, prev_v);
        //find amount of rotation relative to axis
        float rotation = Vector3.SignedAngle(prev_v, head_v, axis);
        //rotate object around axis, relative to head rotation and sensitivity
        transform.RotateAround(transform.position, axis, rotation * sensitivity);    }
        CirculatePrevs(camera.position);
        
    }
    Vector3 SphereOrigin(Vector3[] points, Vector3 shift){
        Debug.Log(points[0]);
        Debug.Log(points[1]);
        Debug.Log(points[2]);
        Debug.Log(shift);
        for(int i = 0; i < 3; i ++){
            points[i] -= shift;
        }
        Debug.Log(points[0]);
        Debug.Log(points[1]);
        Debug.Log(points[2]);
        Vector3 ansatz = Vector3.Cross(points[0].sqrMagnitude * points[1], points[2] ) + 
                Vector3.Cross(points[1].sqrMagnitude * points[2], points[0] ) +
                Vector3.Cross(points[2].sqrMagnitude * points[0], points[1] );
        Debug.Log(ansatz);
        float coefficient = 1f/(Vector3.Dot(2f * points[0],Vector3.Cross(points[1], points[2]) ));
        Debug.Log(coefficient);
        Vector3 shifted_centre = coefficient * ansatz;
        Vector3 centre = shifted_centre + shift;
    
        Debug.Log(centre);
        Debug.Log(shifted_centre);
      
        return centre;


    }
    async void CirculatePrevs(Vector3 cam){
            for (int i = 1 ; i < prev_cam_pos.Length; i++){
                prev_cam_pos[i-1] = prev_cam_pos[i];
            }
            prev_cam_pos[2] = cam;
    }
    
}
