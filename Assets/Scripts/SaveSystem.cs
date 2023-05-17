using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }


    public void SaveToJson(HighScoreData highScore)
    {
        if (LoadFromJson().score > highScore.score) return;
        
        PlayerPrefs.SetInt("HighScore", highScore.score);
    }

    public HighScoreData LoadFromJson()
    {
        HighScoreData loadData = new HighScoreData(PlayerPrefs.GetInt("HighScore"), 0);

        return loadData;
    }

    [ContextMenu("Reset HighScore")]
    private void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }
    
}