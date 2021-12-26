using Assets.Scripts.ConstantsAndEnums;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public List<Light> lights = new List<Light>();
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.OnAccelerationStart += ShowAcceleration;
        PlayerController.OnAccelerationEnd += HideAcceleration;
    }

    void ShowAcceleration()
    {
        foreach (var light in lights)
        {
            light.enabled = true;
        }

        foreach (var particle in particles)
        {
            particle.Play();
        }
    }

    void HideAcceleration()
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }

        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameState.Running)
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
    }
}
