using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private StageSystem stageSystem;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stageText;

    private int score;

    private void Start()
    {
        scoreText.text = 0.ToString();
        stageText.text = 0.ToString();

        stageSystem = FindObjectOfType<StageSystem>();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += ScoreChanged;
        StageSystem.OnStageChanged += StageChanges;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= ScoreChanged;
        StageSystem.OnStageChanged -= StageChanges;
    }


    private void ScoreChanged()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private void StageChanges()
    {
        stageText.text = stageSystem.GetStage.ToString();
    }
}