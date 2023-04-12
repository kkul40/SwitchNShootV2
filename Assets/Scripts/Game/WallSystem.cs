using UnityEngine;

public class WallSystem : MonoBehaviour
{
    [SerializeField] private StageSystem stageSystem;

    [SerializeField] private WarningBar warningBar;
    [SerializeField] private FireWalls fireWalls;
    [SerializeField] private Fades fades;

    [SerializeField] private float warningBarDelay;
    [SerializeField] private bool isFireWallActive;

    private void OnEnable()
    {
        Projectiles.OnLaserFired += StartFireWallSequence;
        Projectiles.OnLaserStopped += StopFireWalls;
    }

    private void OnDisable()
    {
        Projectiles.OnLaserFired -= StartFireWallSequence;
        Projectiles.OnLaserStopped += StopFireWalls;
    }

    private void StartFireWallSequence()
    {
        if (isFireWallActive) return;

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
        if (isFireWallActive) return;

        isFireWallActive = true;
        warningBar.CloseWarningBars();
        //fades.CloseFades();
        if (stageSystem.GetLaserFireCount % 2 == 0)
            fireWalls.OpenFireWalls(true);
        else
            fireWalls.OpenFireWalls(false);

        var laserFireDuration = 5;
        Invoke("StopFireWalls", laserFireDuration);
    }

    private void StopFireWalls()
    {
        fireWalls.CloseFireWalls();
        //fades.OpenFades();

        isFireWallActive = false;
    }
}