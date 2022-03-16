using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Simple script that generates random particles in different shapes
*/
public class GenerateParticles : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject Tunnel;
    [SerializeField] int num_of_particles;
    public Transform camera;
    [SerializeField] float min_radius, max_radius, tunnel_min_length, tunnel_max_length;
    [SerializeField] Shape shape = Shape.Tunnel;
    //enum for different formable shapes
    enum Shape{Tunnel, Sphere, Cloud, Grid}
  
    void Start()
    {
        switch(shape){
            case Shape.Tunnel:
             makeTunnel();
             break;
            case Shape.Sphere:
             makeSphere();
             break;
            case Shape.Cloud:
             makeCloud();
             break;
            
        }
    }
    //generic delegate for finding coords
    delegate Vector3 find_valid_coords();

    // loop through and instantiate particles at given coords, set hideball params
    void generateParticles(find_valid_coords coords){
        for(int i = 0; i < num_of_particles; i++){
            GameObject p = Instantiate(particle, Tunnel.transform);
             
            p.transform.localPosition = coords();
            p.GetComponent<hideball>().camera= camera;
        }
    }
    
    void makeCloud(){
        generateParticles(find_valid_Cloud_coords);
    }
    void makeSphere(){
        generateParticles(find_valid_Sphere_coords);
    }
    void makeTunnel(){
        generateParticles(find_valid_Tunnel_coords);
    }
    //simple random particles, outside of min and within max radius
    Vector3 find_valid_Cloud_coords (){
        Vector3 xyz;
        float x = 0, y = 0, z = 0;
        do {
            x = Random.Range(-max_radius, max_radius);
            y = Random.Range(-max_radius, max_radius);
            z = Random.Range(-max_radius, max_radius);
            xyz = new Vector3(x , y ,z);
        }while(xyz.magnitude < min_radius );
        return xyz;
    }
    // spawns particles in a hollow cylinder 
    Vector3 find_valid_Tunnel_coords (){
            float radius = Random.Range(min_radius, max_radius);
            float angle = Random.Range(0, 2*Mathf.PI);
            float y = radius * Mathf.Sin(angle);
            float x = radius * Mathf.Cos(angle);
            float z = Random.Range(tunnel_min_length, tunnel_max_length);
            return new Vector3(x,y,z);
    }
    // spawns particles in a hollow sphere (can also be a sliced hollow sphere- like a donut)
   Vector3 find_valid_Sphere_coords (){
        
            
            float z = Random.Range(-max_radius, max_radius)/2;
            float newRmin = 0;
            if (z < min_radius && z > -min_radius) newRmin = Mathf.Sqrt((min_radius*min_radius)- (z*z));
        
            float newRmax =  Mathf.Sqrt((max_radius*max_radius)-(z*z));
            float radius = Random.Range(newRmin, newRmax);
            float angle = Random.Range(0, 2*Mathf.PI);
            float y = radius * Mathf.Sin(angle);
            float x = radius * Mathf.Cos(angle);
            return new Vector3(x,y,z);
            
        
    }

    
}
