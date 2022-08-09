using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{
    public float delay = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, delay);
    }

}
