using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("StartScreen")] [SerializeField]
    private GameObject StartScreen;

    [SerializeField] private TextMeshProUGUI startHighScoreText;


    [Header("GameScreen")] [SerializeField]
    private GameObject GameScreen;
    [SerializeField] private GameObject stageTextHolder;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stageText;


    [Header("EndGameScreen")] [SerializeField]
    private GameObject EndGameScreen;

    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TextMeshProUGUI endHighScoreText;

    //TODO daha sonra score systemi oluştur
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
        stageTextHolder.SetActive(false);
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
        OpenStageTextHolder();
    }

    public void OpenStartScreen()
    {
        CloseAllScreen();
        StartScreen.SetActive(true);
        startHighScoreText.text = SaveSystem.Instance.LoadFromJson().score.ToString();
    }

    public void OpenGameScreen()
    {
        CloseAllScreen();
        GameScreen.SetActive(true);
    }
    private void OpenStageTextHolder()
    {
        if(stageSystem.GetStage != 0)
            stageTextHolder.SetActive(true);
        else
            stageTextHolder.SetActive(false);
    }

    public void OpenEndGameScreen()
    {
        CloseAllScreen();
        endScoreText.text = CalculateScore().ToString();
        EndGameScreen.SetActive(true);
    }

    private void CloseAllScreen()
    {
        StartScreen.SetActive(false);
        GameScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    private int CalculateScore()
    {
        // Save And Load At the End
        double tempScore = 0;
        tempScore = score * (stageSystem.GetStage * 12.50/100);
        int finalScore = (int)(score + tempScore);
        
        var highScoreData = new HighScoreData(finalScore, 0);
        SaveSystem.Instance.SaveToJson(highScoreData);
        endHighScoreText.text = SaveSystem.Instance.LoadFromJson().score.ToString();

        return finalScore;
    }
}