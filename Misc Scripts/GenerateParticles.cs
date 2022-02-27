using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParticles : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject Tunnel;
    [SerializeField] int num_of_particles;
    [SerializeField] int min_radius, max_radius, tunnel_min_length, tunnel_max_length;
    [SerializeField] bool sphere = false;
    // Start is called before the first frame update
    void Start()
    {
        if(sphere) makeSphere(); 
        else makeTunnel();
    }

    void makeTunnel(){
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
