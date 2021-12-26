using Assets.Scripts.Configuration;
using UnityEngine;

public class WrapAroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var x = transform.position.x;
        var z = transform.position.z;

        if (x < Config.XMin || x > Config.XMax
            || z < Config.ZMin || z > Config.ZMax)
        {

            float newX = x;
            float newZ = z;

            // We use an if instead of a switch, because we want a no-op if it is not over the edge
            if (x < Config.XMin)
            {
                newX = Config.XMax;
            }
            else if (x > Config.XMax)
            {
                newX = Config.XMin;
            }

            if (z < Config.ZMin)
            {
                newZ = Config.ZMax;
            }
            else if (z > Config.ZMax)
            {
                newZ = Config.ZMin;
            }

            transform.position = new Vector3(newX, transform.position.y, newZ);
        }
    }
}
