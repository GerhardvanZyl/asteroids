using Assets.Scripts.Configuration;
using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;

    public float thrustMultiplier = 10f;
    public float rotationMultiplier = 1f;
    public GameObject lazerPrefab;
    public float fireRate = 0.5f;
    public AudioClip lazerSound;

    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private float ttLazer = 0f;
    private Quaternion initialRotation;

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
        transform.position = Config.StartPosition;
        playerRb.velocity = Vector3.zero;
        playerRb.rotation = initialRotation;
    }

    void OnGameOverConfirmed()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        initialRotation = playerRb.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        HandleMovement();
        HandleWeapons();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag(Tags.ASTEROID))
        {
            OnPlayerDied();

            // Render explosion
            // Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        // fix any accidental rotation caused by collisions
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        if (horizontalInput != 0f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotationMultiplier * horizontalInput);
        }

        if (verticalInput != 0f)
        {
            playerRb.AddRelativeForce(Vector3.forward * thrustMultiplier * verticalInput);
        }
    }

    private void HandleWeapons()
    {
        ttLazer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && ttLazer <= 0)
        {
            Instantiate(lazerPrefab, transform.position, transform.rotation );
            ttLazer = fireRate;
            playerAudio.PlayOneShot(lazerSound, 0.5f);
        }
    }
}
