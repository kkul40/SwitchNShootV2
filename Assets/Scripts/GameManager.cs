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
    }

    private void Start()
    {
        currentStage = Stages.Intro;
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += PlayerIsDead;
        Player.OnPlayerStarted += SetStageToGame;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= PlayerIsDead;
        Player.OnPlayerStarted -= SetStageToGame;
    }

    private void PlayerIsDead()
    {
        currentStage = Stages.Outro;
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        ResetScene();
    }

    private void SetStageToGame()
    {
        currentStage = Stages.Game;
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
    }

    private void ResetScene()
    {
        StartCoroutine(ResetSceneCO());
    }


    private IEnumerator ResetSceneCO()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}