using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Configuration;
using UnityEngine;
using UnityEngine.Advertisements;

public class DisplayAd : MonoBehaviour
{
    public bool isAdStarted;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(Config.GameId);
    }

    // Update is called once per frame
    void Update()
    {
        if(Advertisement.isInitialized && Advertisement.IsReady("Interstitial_Android") && !isAdStarted)
        {
            Advertisement.Show("Interstitial_Android");
            isAdStarted = true;
        }
    }
}
