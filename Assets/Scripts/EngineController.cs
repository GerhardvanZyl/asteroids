using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    

    public List<Light> lights = new List<Light>();
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            foreach (var particle in particles)
            {
                particle.Stop();
            }

            foreach (var light in lights)
            {
                light.enabled = false;
            }

            return;
        }

        if(Input.GetAxis("Vertical") > 0)
        {
            foreach(var light in lights)
            {
                light.enabled = true;
            }
        } else
        {
            foreach (var light in lights)
            {
                light.enabled = false;
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }
        else
        {
            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }
    }
}
