using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Stages
{
    Intro,
    Game,
    Outro
}

public class GameManager : MonoBehaviour
{
    public Stages currentStage;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        currentStage = Stages.Intro;
        UIManager.Instance.OpenStartScreen();

        Application.targetFrameRate = 120;
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerDeath += PlayerIsDead;
        PlayerManager.OnPlayerStarted += SetStageToGame;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDeath -= PlayerIsDead;
        PlayerManager.OnPlayerStarted -= SetStageToGame;
    }

    private void PlayerIsDead()
    {
        currentStage = Stages.Outro;
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        UIManager.Instance.OpenEndGameScreen();
    }

    private void SetStageToGame()
    {
        UIManager.Instance.OpenGameScreen();
        currentStage = Stages.Game;
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}