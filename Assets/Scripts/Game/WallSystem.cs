using UnityEngine;
using UnityEngine.Serialization;

public class WallSystem : MonoBehaviour
{
    [FormerlySerializedAs("stageSystem")] [SerializeField] private StageManager stageManager;

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
        Invoke(nameof(StartWarningBars), 0);
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

    public void ResetEverything()
    {
        CancelInvoke();
        StopFireWalls();
        warningBar.CloseWarningBars();
    }
}