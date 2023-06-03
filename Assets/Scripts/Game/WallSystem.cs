using UnityEngine;

public class WallSystem : MonoBehaviour
{
    [SerializeField] private StageSystem stageSystem;

    [SerializeField] private WarningBar warningBar;
    [SerializeField] private FireWalls fireWalls;
    [SerializeField] private Fades fades;

    [SerializeField] private float warningBarDelay;

    private void OnEnable()
    {
        ProjectileManager.OnLaserFired += StartFireWallSequence;
    }

    private void OnDisable()
    {
        ProjectileManager.OnLaserFired -= StartFireWallSequence;

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
        
        fireWalls.OpenFireWalls();
    }

    public void StopFireWalls()
    {
        fireWalls.ResetFireWalls();
    }
}