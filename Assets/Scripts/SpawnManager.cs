using Assets.Scripts.Configuration;
using Assets.Scripts.ConstantsAndEnums;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public delegate void SpawnDelegate();
    public static SpawnManager Instance;

    [SerializeField] float spawnInterval = 20f;
    [SerializeField] float spawnIntervalAdjustment = 1f;
    [SerializeField] GameObject asteroid;
    [SerializeField] GameObject player;
    
    [SerializeField] ProjectileController lazerPrefab;
    private ObjectPool<ProjectileController> lazerPool;

    [SerializeField] ParticleSystem explosionPrefab;
    private ObjectPool<ParticleSystem> explosionPool;

    private GameManager game;
    private float ttSpawn;
    private float currentSpawnInterval;
    private Vector3 spawnLocation1 = new Vector3(Config.XMin + 10, 1, Config.ZMin + 10);
    private Vector3 spawnLocation2 = new Vector3(Config.XMax - 10, 1, Config.ZMax - 10);
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        game = GameManager.Instance;
        ttSpawn = spawnInterval;

        initPools();
    }

    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        GameManager.OnGameStarted -= OnGameStarted;
    }

    void OnGameStarted()
    {
        var asteroids = GameObject.FindGameObjectsWithTag(Tags.ASTEROID);

        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }

        currentSpawnInterval = spawnInterval;
        Instantiate(asteroid);
    }

    void OnGameOverConfirmed()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State == GameState.Running)
        {
            ttSpawn -= Time.deltaTime;

            if (ttSpawn <= 0)
            {
                currentSpawnInterval -= spawnIntervalAdjustment;
                ttSpawn = currentSpawnInterval;

                SpawnAsteroid();
            }
        }
    }

    public void FireLazer()
    {
        var lazer = lazerPool.Get();
    }

    public void KillLazer(ProjectileController lazerInstance)
    {
        lazerPool.Release(lazerInstance);
    }

    public void SpawnExplosion(GameObject asteroid)
    {
        var explosion = explosionPool.Get();

    }

    public void KillExplosion(ParticleSystem explosion)
    {
        explosionPool.Release(explosion);
    }

    private void initPools()
    {
        lazerPool = new ObjectPool<ProjectileController>(() => {
            return Instantiate(lazerPrefab);
        },
        lazer => {
            lazer.gameObject.SetActive(true);
            lazer.transform.position = player.transform.position;
            lazer.transform.rotation = player.transform.rotation;
        },
        lazer => {
            lazer.gameObject.SetActive(false);
            lazer.Rgdbody.velocity = Vector3.zero;
            lazer.Rgdbody.angularVelocity = Vector3.zero;
        }, lazer => {
            Destroy(lazer.gameObject);
        }, false, 4, 4);

        explosionPool = new ObjectPool<ParticleSystem>(() => {
            return Instantiate(explosionPrefab);
        },
        explosion => {
            explosion.gameObject.SetActive(true);
        },
        explosion => {
            explosion.gameObject.SetActive(false);
        }, explosion => {
            Destroy(explosion);
        }, false, 1, 2);
    }

    private void SpawnAsteroid()
    {
        // initial spawn
        var spawnLocation = new Vector3(Random.Range(Config.XMin, Config.XMax), 1, Random.Range(Config.ZMin, Config.ZMax));

        // Check if it's close to the player
        var overlaps = Physics.OverlapSphere(spawnLocation, 20f);

        if (overlaps.Any(x => x.gameObject.layer == (int)Layers.Player))
        {
            // Then try the two hardcoded 
            spawnLocation = spawnLocation1;

            overlaps = Physics.OverlapSphere(spawnLocation, 20f);
            if (overlaps.Any(x => x.gameObject.layer == (int)Layers.Player))
            {
                spawnLocation1 = spawnLocation2;
            }
        }

        Instantiate(asteroid, spawnLocation, transform.rotation);
    }


}
