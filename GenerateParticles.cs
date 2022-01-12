using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParticles : MonoBehaviour
{
    public GameObject particle;
    public GameObject Tunnel;
    public int num_of_particles;
    public int min_radius, max_radius, tunnel_min_length, tunnel_max_length;
    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
