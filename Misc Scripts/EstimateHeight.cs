using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EstimateHeight : MonoBehaviour
{
    public Vector3[] points = new Vector3[4];
    int pointer = 0;
    List<Vector3> collected_data= new List<Vector3>();
    public float maxTime = 4f, collecting = 4f;
    public Transform camera, controller;
    public GameObject wiiboard;

    [SerializeField] appmanager AppManager;
    public void collectData(){
        if (pointer < 4){collecting = 0;}
    }
    public void setHeightToController(){
       SetBoardAndAnkle(controller.position);
   }
   public void lockInHeighToController(){
        AppManager.stage = appmanager.Stage.StartGetWeight;
   }
    void Update(){
        if (collecting<maxTime){
            collecting+=Time.deltaTime;
            collected_data.Add(camera.position);
            if (collecting>maxTime){
                points[pointer] = Sum(collected_data)/collected_data.Count();
                pointer++;
                collected_data = new List<Vector3>();
                if (pointer == 4){
                    
                    SetBoardAndAnkle(estimateSphere());
                    }
            }
        }   
    }
    Vector3 Sum(List<Vector3> s){
        Vector3 o = new Vector3(0,0,0);
        foreach (Vector3 item in s)
        {
            o+= item;
        }
        return o;
    }
    void SetBoardAndAnkle(Vector3 p){
        wiiboard.transform.position = p;
    }
    Vector3 estimateSphere(){
        Vector3 shift = points[0];
        for(int i = 0; i < 4; i ++){
            points[i] -= shift;
        }
        Vector3 ansatz = Vector3.Cross(points[1].sqrMagnitude * points[2], points[3] ) + 
                Vector3.Cross(points[2].sqrMagnitude * points[3], points[1] ) +
                Vector3.Cross(points[3].sqrMagnitude * points[1], points[2] );
        float coefficient = 1/(Vector3.Dot(2 * points[1],Vector3.Cross(points[2], points[3]) ));
        Vector3 shifted_centre = coefficient * ansatz;
        Vector3 centre = shifted_centre + shift;
        Debug.Log(centre);
        Debug.Log(shifted_centre);
        return centre;
    }
   

}
