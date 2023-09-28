using System.Collections;
using UnityEngine;

public class PowerBarManager : MonoBehaviour
{
    [SerializeField] private ProjectileManager projectileManager;

    [SerializeField] private PowerBar[] powerBars;


    private void OnEnable()
    {
        UpdateBars();
        ProjectileManager.OnIndexChange += UpdateBars;
        ProjectileManager.OnLaserFired += StartBlinking;
        ProjectileManager.OnLaserStopped += StopBlinking;
        StageSystem.OnStageChanged += UpdateBars;
    }

    private void OnDisable()
    {
        ProjectileManager.OnIndexChange -= UpdateBars;
        ProjectileManager.OnLaserFired -= StartBlinking;
        ProjectileManager.OnLaserStopped -= StopBlinking;
        StageSystem.OnStageChanged -= UpdateBars;
    }

    private void UpdateBars()
    {
        var index = projectileManager.currentProjectileIndex;

        for (var i = 0; i < powerBars.Length; i++)

            if (i < index)
            {
                if (powerBars[i].isFilled)
                    continue;
        
                powerBars[i].Fill();
            }
            else
            {
                if (!powerBars[i].isFilled)
                    continue;
                
                powerBars[i].DontFill();
            }
    }

    private void StartBlinking()
    {
        StartCoroutine(BlinkingBarsCo());
    }
    
    private void StopBlinking()
    {
        StopAllCoroutines();
        StopCoroutine(BlinkingBarsCo());
        UpdateBars();
    }
    
    IEnumerator BlinkingBarsCo()
    {
        while (true)
        {
            foreach (var powerBar in powerBars)
            {
                powerBar.DontFill();
            }

            yield return new WaitForSeconds(0.1f);
            
            foreach (var powerBar in powerBars)
            {
                powerBar.Fill();
            }
            
            yield return new WaitForSeconds(0.1f);

        }
    }
}