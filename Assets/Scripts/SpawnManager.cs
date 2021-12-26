using Assets.Scripts.Configuration;
using Assets.Scripts.ConstantsAndEnums;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnInterval = 20f;
    public float spawnIntervalAdjustment = 1f;
    public GameObject asteroid;

    private GameManager game;
    private float ttSpawn;
    private float currentSpawnInterval;
    private Vector3 spawnLocation1 = new Vector3(Config.XMin + 10, 1, Config.ZMin + 10);
    private Vector3 spawnLocation2 = new Vector3(Config.XMax - 10, 1, Config.ZMax - 10);

    // Start is called before the first frame update
    void Start()
    {
        game = GameManager.Instance;
        ttSpawn = spawnInterval;
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
