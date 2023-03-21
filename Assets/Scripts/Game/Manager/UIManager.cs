using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stageText;

    private int score;
    private StageSystem stageSystem;

    private void Start()
    {
        stageSystem = FindObjectOfType<StageSystem>();

        scoreText.text = 0.ToString();
        stageText.text = stageSystem.GetStage.ToString();
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