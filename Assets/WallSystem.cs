using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSystem : MonoBehaviour
{
    [SerializeField] private WarningBar warningBar;
    [SerializeField] private FireWalls fireWalls;
    [SerializeField] private Fades fades;

    [SerializeField] private float warningBarDelay;

    private void OnEnable()
    {
        Projectiles.OnLaserFired += StartFireWallSequence;
    }


    private void StartFireWallSequence()
    {
        Invoke("StartWarningBars", 0);
    }


    // fire wall stages 
    private void StartWarningBars()
    {
        warningBar.OpenWarningBars();

        Invoke("StartFireWalls", warningBarDelay);
    }

    private void StartFireWalls()
    {
        warningBar.CloseWarningBars();
        fades.CloseFades();
        fireWalls.OpenFireWalls();

        Invoke("StopFireWalls", 5);
    }


    private void StopFireWalls()
    {
        fireWalls.CloseFireWalls();
        fades.OpenFades();
    }
    
}
