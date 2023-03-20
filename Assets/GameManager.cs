using UnityEngine;

public enum Stages
{
    Intro,
    Game,
    Outro
}

public class GameManager : MonoBehaviour
{
    public Stages currentStage;
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
        Player.OnGameStarted += SetStageToGame;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= PlayerIsDead;
        Player.OnGameStarted += SetStageToGame;
    }

    private void PlayerIsDead()
    {
        currentStage = Stages.Outro;
    }

    private void SetStageToGame()
    {
        currentStage = Stages.Game;
    }
}