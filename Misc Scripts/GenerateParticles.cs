using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParticles : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject Tunnel;
    [SerializeField] int num_of_particles;
        public Transform camera;
    [SerializeField] int min_radius, max_radius, tunnel_min_length, tunnel_max_length;
    [SerializeField] Shape shape = Shape.Tunnel;
    enum Shape{Tunnel, Sphere, Cloud, Grid}
    // Start is called before the first frame update
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
 
    void makeCloud(){
        for(int i = 0; i < num_of_particles; i++){
            GameObject p = Instantiate(particle, Tunnel.transform);
             
            p.transform.localPosition = find_valid_coords(min_radius, max_radius);
            p.GetComponent<hideball>().camera= camera;
        }
    }
    Vector3 find_valid_coords (float min, float max){
        float x = 0, y = 0, z = 0;
        do {
            x = Random.Range(-max, max);
            y = Random.Range(-max, max);
            z = Random.Range(-max, max);
        }while(Mathf.Abs(x) < min && Mathf.Abs(y) < min && Mathf.Abs(z) < min);
        return new Vector3(x , y ,z);
    }
    public void makeTunnel(){
        for(int i = 0; i < num_of_particles; i++){
            GameObject p = Instantiate(particle, Tunnel.transform);
            float radius = Random.Range(min_radius, max_radius);
            float angle = Random.Range(0, 2*Mathf.PI);
            float y = radius * Mathf.Sin(angle);
            float x = radius * Mathf.Cos(angle);
            float z = Random.Range(tunnel_min_length, tunnel_max_length);
            p.transform.localPosition = new Vector3(x,y,z);
        }
    }
    void makeSphere(){
        for(int i = 0; i < num_of_particles; i++){
            GameObject p = Instantiate(particle, Tunnel.transform);
            float z = Random.Range(-max_radius, max_radius)/2;
            float newRmin = 0;
            if (z < min_radius && z > -min_radius) newRmin = Mathf.Sqrt((min_radius*min_radius)- (z*z));
        
            float newRmax =  Mathf.Sqrt((max_radius*max_radius)-(z*z));
            float radius = Random.Range(newRmin, newRmax);
            float angle = Random.Range(0, 2*Mathf.PI);
            float y = radius * Mathf.Sin(angle);
            float x = radius * Mathf.Cos(angle);
            p.transform.localPosition = new Vector3(x,y,z);
        }
        
    }
}
