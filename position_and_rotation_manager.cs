using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position_and_rotation_manager : SwayBaseClass
{
    [SerializeField] Transform real_world_input;
    
    Quaternion prev_rot;
    Vector3 prev_pos;
    public bool gaining = false;
 

    // Update is called once per frame
    void Update()
    {
        Gain();
        prev_pos = real_world_input.position;
        prev_rot = real_world_input.localRotation;
        
    }
    void Gain(){
        
        //rotation
        //get change in rotation
        var change_in_direction = Quaternion.Inverse(prev_rot) * real_world_input.localRotation;
        //get axis angle representation
        change_in_direction.ToAngleAxis(out var change_angle, out var change_axis);
        // apply gain and update rotation 
        transform.rotation *= Quaternion.AngleAxis(change_angle * (1 + sensitivity), change_axis);


        //get absolute change in position 
        var displacement = real_world_input.position - prev_pos;
        // convert into vector relative to real world directions 
        // ie if camera move in the direction it was facing, this would be (0,0,1)
        var unrotated_displacement = Quaternion.Inverse(real_world_input.localRotation) * displacement; 
        //apply simulated camera rotation and gain
        transform.position += (transform.rotation * unrotated_displacement)*(1 + sensitivity);
            

    }
    Vector3 get_axis(Quaternion q){
        return new Vector3(q.x, q.y, q.z);
    }
   
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gain_in_position_and_rotation : MonoBehaviour
{
    [SerializeField] Transform camera, p;
    [SerializeField] float gain;
    Quaternion prev_rot;
    Vector3 prev_pos;
    Quaternion origin = new Quaternion(0,0.707f,0,0.707f);
    public bool gaining = false;
 void Start(){
     origin = Quaternion.AngleAxis(180, Vector3.up);
 }
    void start_pendulum(){
        gaining = true;
        prev_pos = camera.position;
        prev_rot = camera.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(p.rotation.x + " , " + p.rotation.y + " , " + p.rotation.z + " , " + p.rotation.w);
        if (gaining){
            Gain();
        }
    }
    void Gain(){
        //gain in position
        //transform.position += (camera.position - prev_pos) * gain;
        //transform.rotation *=  (prev_rot * Quaternion.Inverse(camera.rotation));
        
        //
        Vector3 quart_axis = Vector3.Cross( get_axis(origin), get_axis(camera.localRotation));
        float r = Vector3.SignedAngle(get_axis(camera.localRotation), origin, quart_axis);
        
        Quaternion relative = camera.localRotation * Quaternion.Inverse(prev_rot);
        transform.rotation = relative * camera.localRotation;
        prev_pos = camera.position;
        prev_rot = camera.localRotation;
        
    }
    Vector3 get_axis(Quaternion q){
        return new Vector3(q.x,q.y,q.z);
    }
    Quaternion get_quart(Vector3 v , float a){
        return new Quaternion(v.x,v.y,v.z,a);
    }
}
*/