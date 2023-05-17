using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarManager : MonoBehaviour
{
    [SerializeField] private ProjectileManager projectileManager;
    
    [SerializeField] private PowerBar[] powerBars;


    private void OnEnable()
    {
        UpdateBars();
        ProjectileManager.OnLevelUp += UpdateBars;
    }

    private void OnDisable()
    {
        ProjectileManager.OnLevelUp -= UpdateBars;

    }

    private void UpdateBars()
    {
        var index = projectileManager.projectileIndex;

        for (int i = 0; i < powerBars.Length; i++)
        {
            if (i <= index)
                powerBars[i].Fill();
            else
                powerBars[i].DontFill();
            
        }
    }
}
