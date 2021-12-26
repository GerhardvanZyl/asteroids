using Assets.Scripts.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour
{
    [SerializeField] bool _testMode = true;
    private string _gameId;
    public bool isAdStarted;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? Config.GameId // No plans for ios yet, so this code is just here for reference
            : Config.GameId;
        Advertisement.Initialize(_gameId, _testMode);

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
