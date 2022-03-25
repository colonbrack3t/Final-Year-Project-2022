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
    enum Shape{Tunnel, Sphere, Cloud, Grid, UniformCloud}
  
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
            case Shape.UniformCloud:
                makeUniformCloud();
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
    async void makeUniformCloud(){
         int p_row = (int)Mathf.Round((Mathf.Pow(num_of_particles, 1f / 3)));
         float range = (max_radius)/p_row;
         int p_side = (int) (p_row * p_row);
         
         for(int i = 0; i < num_of_particles; i++){
            
            //decide x y z ranges 
            int zorigin = (int) (i / p_side);
            int zmod = (int) (i % p_side);
            int yorigin = zmod == 0 ? 0 : (int) (zmod / p_row);
            int xorigin = (int) (i % p_row);
            
            float x = 0, y = 0, z = 0;
            x = (xorigin - p_row/2f)/ (p_row/2f);
            y = (yorigin - p_row/2f)/ (p_row/2f);
            z = (zorigin - p_row/2f)/ (p_row/2f);
            
            
            
            x = (x * (max_radius));
            y = (y * (max_radius));
            z = (z * (max_radius));
            
            x = Random.Range(x , x + range);
            y = Random.Range(y , y + range);
            z = Random.Range(z , z + range);
            if(Mathf.Abs(x) < min_radius &&Mathf.Abs(y) < min_radius &&Mathf.Abs(z) < min_radius)
                continue;
            GameObject p = Instantiate(particle, Tunnel.transform);
            Vector3 xyz = new Vector3(x , y ,z);
            
            p.transform.localPosition = xyz;

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
