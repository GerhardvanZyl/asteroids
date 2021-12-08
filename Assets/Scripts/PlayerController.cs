using Assets.Scripts.Configuration;
using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;

    public float thrustMultiplier = 10f;
    public float rotationMultiplier = 1f;
    public GameObject lazerPrefab;
    public float fireRate = 0.5f;
    public AudioClip lazerSound;

    /// <summary>
    /// DPad script
    /// </summary>
    [Tooltip("Fingers DPad Script")]
    public FingersDPadScript DPadScript;

    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private float ttLazer = 0f;
    private Quaternion initialRotation;

    private TapGestureRecognizer tapGesture;

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

    private void Awake()
    {
        DPadScript.DPadItemTapped = DPadTapped;
        DPadScript.DPadItemPanned = DPadPanned;
    }

    private void DPadTapped(FingersDPadScript script, FingersDPadItem item, TapGestureRecognizer gesture)
    {
        //if((item & FingersDPadItem.Center) != FingersDPadItem.None)
        //{
        //    // Center tap - do nothing
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        initialRotation = playerRb.rotation;

        // setup gestures
        tapGesture = new TapGestureRecognizer { MaximumNumberOfTouchesToTrack = 1 };
        tapGesture.StateUpdated += TapGesture_StateUpdated;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void TapGesture_StateUpdated(GestureRecognizer gesture)
    {
        //Debug.LogFormat("Single tap state: {0}", gesture.State);
        //if (gesture.State == GestureRecognizerState.Ended)
        //{
        //    string msg = string.Format("Single tap at {0},{1}", gesture.FocusX, gesture.FocusY);
        //    Debug.Log(msg);

        //    FireWeapon();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        HandleMovement(horizontalInput, verticalInput);

        ttLazer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Handle weapons - Space pressed");
            FireWeapon();
        }
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

    /// <summary>
    /// Callback for tap gesture
    /// </summary>
    /// <param name="gesture">Tap gesture</param>
    public void FireButtonTapped(DigitalRubyShared.GestureRecognizer gesture)
    {
        Debug.LogFormat("Single tap state: {0}", gesture.State);
        if (gesture.State == GestureRecognizerState.Ended)
        {
            string msg = string.Format("Single tap at {0},{1}", gesture.FocusX, gesture.FocusY);
            Debug.Log(msg);

            FireWeapon();
        }
    }

    private void DPadPanned(FingersDPadScript script, FingersDPadItem item, PanGestureRecognizer gesture)
    {
        Debug.Log($"DPad panned: {(FingersDPadItem)item}");
        float horizontalInput = 0;
        float verticalInput = 0;

        if((item & FingersDPadItem.Right) != FingersDPadItem.None)
        {
            horizontalInput = 1;
        }

        if ((item & FingersDPadItem.Left) != FingersDPadItem.None)
        {
            horizontalInput = -1;
        }

        if ((item & FingersDPadItem.Up) != FingersDPadItem.None)
        {
            verticalInput = 1;
        }

        if ((item & FingersDPadItem.Down) != FingersDPadItem.None)
        {
            verticalInput = -1;
        }

        Debug.Log($"Calling Handle Movement. H: {horizontalInput} V: {verticalInput} ");
        HandleMovement(horizontalInput, verticalInput);
    }

    private void HandleMovement(float horizontalInput, float verticalInput)
    {
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

    private void FireWeapon()
    {
        if (ttLazer <= 0)
        {
            Instantiate(lazerPrefab, transform.position, transform.rotation);
            ttLazer = fireRate;
            playerAudio.PlayOneShot(lazerSound, 0.5f);
        }
    }
}
