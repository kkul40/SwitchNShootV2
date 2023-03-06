using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSystem : MonoBehaviour
{
    [SerializeField] private WarningBar warningBar;
    [SerializeField] private FireWalls fireWalls;
    [SerializeField] private Fades fades;

    [SerializeField] private float warningBarDelay;
    [SerializeField] private bool isFireWallActive;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        Projectiles.OnLaserFired += StartFireWallSequence;
    }


    private void StartFireWallSequence()
    {
        if (isFireWallActive)
            return;

        Invoke("StartWarningBars", 0);
    }


    // fire wall stages 
    private void StartWarningBars()
    {
        isFireWallActive = true;

        warningBar.OpenWarningBars();

        Invoke("StartFireWalls", warningBarDelay);
    }

    private void StartFireWalls()
    {
        warningBar.CloseWarningBars();
        //fades.CloseFades();
        fireWalls.OpenFireWalls();

        int laserFireDuration = 5;
        Invoke("StopFireWalls", laserFireDuration);
    }


    private void StopFireWalls()
    {
        fireWalls.CloseFireWalls();
        //fades.OpenFades();

        isFireWallActive = false;
    }

    private void OnDestroy()
    {
        Debug.LogError("wall system destroy");
    }

}
