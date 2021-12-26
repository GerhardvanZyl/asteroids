using Assets.Scripts.ConstantsAndEnums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidController : MonoBehaviour
{
    private Rigidbody asteroidRb;
    private List<MeshCollider> childColliders = new List<MeshCollider>();
    private float asteroidRadius;
    private float initialY = 1f;
    private ParticleSystem dustParticles;


    public delegate void AsteroidDelegate();
    public static event AsteroidDelegate OnAsteroidDestroyed;

    public GameObject smallerAsteroid;
    public int smallerAsteroidCount = 4;
    public AudioClip[] explosionClips;
    public float maxDriftSpeed = 3f;
    public Collider[] collidesWith;
    public ParticleSystem dustParticlesType;

    // Start is called before the first frame update
    void Start()
    {
        asteroidRb = GetComponent<Rigidbody>();

        var sphereCollider = GetComponent<SphereCollider>();

        if (sphereCollider != null)
        {
            asteroidRadius = sphereCollider.radius;
        }
        else // box collider
        {
            var boxCollider = GetComponent<BoxCollider>();
            var size = boxCollider.size;

            float longestEdgeLength = size.x > size.y ? size.x : size.y;
            longestEdgeLength = longestEdgeLength > size.z ? longestEdgeLength : size.z;

            asteroidRadius = longestEdgeLength / 2;
        }

        // radius etc will be local, so we need to multiply by scale.
        asteroidRadius *= transform.localScale.x;

        initialY = transform.position.y;

        InitDrift();
    }

    // Update is called once per frame
    void Update()
    {
        // just make sure not vertical movement.
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        var colliders = Physics.OverlapSphere(transform.position, asteroidRadius);

        collidesWith = colliders;

        bool overlapsAsteroids = colliders.Any(x => x.gameObject.CompareTag(Tags.ASTEROID) && gameObject != x.gameObject);

        if (!overlapsAsteroids && gameObject.layer != (int)Layers.Asteroids)
        {
            gameObject.layer = (int)Layers.Asteroids;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.LAZER))
        {
            Instantiate(dustParticlesType, transform.position, transform.rotation);
            OnAsteroidDestroyed();

            //var randomPitch = Random.Range(-0.02f, 0.02f);
            var randomClipIndex = Random.Range(0, explosionClips.Length);

            var explosionAudio = explosionClips[randomClipIndex];
            
            AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 0.35f);
            BreakApart();
            Destroy(other.gameObject);
        }
    }

    private void BreakApart()
    {
        // If the next one smaller isn't specified, just destroy
        if (smallerAsteroid != null)
        {
            for (var i = 0; i < smallerAsteroidCount; i++)
            {
                childColliders.Add(
                    Instantiate(smallerAsteroid, transform.position, transform.rotation )
                    .GetComponent<MeshCollider>()
                    );
            }
        }

        Destroy(gameObject);
    }

    private void InitDrift()
    {
        var driftXSpeed = Random.Range(-maxDriftSpeed, maxDriftSpeed) * asteroidRb.mass;
        var driftZSpeed = Random.Range(-maxDriftSpeed, maxDriftSpeed) * asteroidRb.mass;

        var driftForce = new Vector3(driftXSpeed, 0, driftZSpeed);

        asteroidRb.AddRelativeForce(driftForce, ForceMode.Impulse);
    }
}
