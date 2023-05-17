using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("StartScreen")]
    [SerializeField] private GameObject StartScreen;

    
    [Header("GameScreen")]
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stageText;

    
    [Header("EndGameScreen")]
    [SerializeField] private GameObject EndGameScreen;
    [SerializeField] private TextMeshProUGUI endSoreText;


    
    private int score;
    private StageSystem stageSystem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

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
        if (GameManager.Instance.currentStage != Stages.Game) return;

        score++;
        scoreText.text = score.ToString();
    }

    private void StageChanges()
    {
        stageText.text = stageSystem.GetStage.ToString();
    }

    public void OpenStartScreen()
    {
        CloseAllScreen();
        StartScreen.SetActive(true);
    }
    public void OpenGameScreen()
    {
        CloseAllScreen();
        GameScreen.SetActive(true);
    }
    public void OpenEndGameScreen()
    {
        CloseAllScreen();
        endSoreText.text = score.ToString();
        EndGameScreen.SetActive(true);
    }

    private void CloseAllScreen()
    {
        StartScreen.SetActive(false);
        GameScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }
}