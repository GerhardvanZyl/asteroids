using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Vector3 currentlScale;

    public float speed = 200f;
    public float growthRatio = 2f;
    public float lifetime = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Grow();

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        { 
            Destroy(gameObject);
        }
    }

    private void Grow()
    {
        var size = transform.localScale.z >= 30f ? 30f : transform.localScale.z * (1 + Time.deltaTime) * growthRatio;
        transform.localScale = new Vector3(0.5f, 0.1f, size);
    }
}
