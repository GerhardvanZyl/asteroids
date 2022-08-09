using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //private Vector3 currentlScale;

    public float speed = 200f;
    public float growthRatio = 2f;
    public float lifetime = 1.5f;
    public float maxSize = 30f;
    public Rigidbody Rgdbody;

    public void Awake()
    {
        Rgdbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.5f, 0.1f, 2f);
        StartCoroutine(DestroyProjectileAfterDelay());
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Update()
    {
        if (this.isActiveAndEnabled)
        {
            var size = transform.localScale.z >= maxSize
                ? maxSize
                : transform.localScale.z * (1 + Time.deltaTime) * growthRatio;
            transform.localScale = new Vector3(0.5f, 0.1f, size);
        }
    }

    IEnumerator DestroyProjectileAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        SpawnManager.Instance.KillLazer(this);
  
    }
}
