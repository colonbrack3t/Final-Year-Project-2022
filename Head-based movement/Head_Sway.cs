using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Sway : MonoBehaviour {
[SerializeField] float head_rot_modifier = -0.1f, head_pos_modifier = 1f;
[SerializeField] Transform camera;

void Update(){

    transform.rotation =  Quaternion.Euler(camera.localRotation.eulerAngles * head_rot_modifier);
    //transform.position = camera.localPosition * head_pos_modifier;
}
}