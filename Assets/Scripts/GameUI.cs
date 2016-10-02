using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Timer Timer;

    public GameObject InGameUI;
    public GameObject FinishGameUI;
    public GameObject IdleGameUI;
    public GameObject GyroError;
    public Text ActualSymbolText;
    public Text TimerText;
    public Text FinishGameText;

    private Game Game;

    void Awake()
    {
        Game = Game.Instance;

        if (!SystemInfo.supportsGyroscope)
        {
            GyroError.SetActive(true);
        }

        Game.GameStateChangeEvent += OnGameStateChangeEvent;
        Game.ActualSymbolChangeEvent += OnActualSymbolChangeEvent;
        Timer.TimeChangeEvent += OnTimeChangeEvent;
        Timer.OutOfTimeEvent += OnOutOfTimeEvent;
    }


    void Destroy()
    {
        Game.GameStateChangeEvent -= OnGameStateChangeEvent;
        Game.ActualSymbolChangeEvent -= OnActualSymbolChangeEvent;
        Timer.TimeChangeEvent -= OnTimeChangeEvent;
        Timer.OutOfTimeEvent -= OnOutOfTimeEvent;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SetIdleGameMode();
            return;
        }
    }

    public void StartHardGame()
    {
        Game.StartNew(Game.EGameDifficulty.Hard);
    }

    public void StartEasyGame()
    {
        Game.StartNew(Game.EGameDifficulty.Easy);
    }

    public void FinishGame()
    {
        Game.Finish();
    }

    public void SetIdleGameMode()
    {
        Game.SetIdleMode();
    }

    private void OnOutOfTimeEvent()
    {
        FinishGame();
    }

    private void OnTimeChangeEvent(float time)
    {
        var min = Mathf.FloorToInt(time / 60);
        var sec = Mathf.FloorToInt(time) % 60;
        TimerText.text = string.Format("{0:d2}:{1:d2}", min, sec);

        if (time < 20)
            TimerText.color = Color.red;
        else
            TimerText.color = Color.white;
    }

    private void OnActualSymbolChangeEvent(Symbol newSymbol)
    {
        ActualSymbolText.text = "Destroy symbol: " + newSymbol.Index.ToString();
    }

    private void OnGameStateChangeEvent(Game.EGameState state)
    {
        InGameUI.SetActive(false);
        FinishGameUI.SetActive(false);
        IdleGameUI.SetActive(false);

        switch (state)
        {
            case Game.EGameState.Idle:
                IdleGameUI.SetActive(true);
                break;
            case Game.EGameState.StartNewGame:
                Timer.StartTimer();
                break;
            case Game.EGameState.InGame:
                InGameUI.SetActive(true);
                break;
            case Game.EGameState.FinishGame:
                Timer.StopTimer();
                break;
            case Game.EGameState.GameSuccess:
                FinishGameUI.SetActive(true);
                FinishGameText.text = "You are awesome!";
                break;
            case Game.EGameState.GameFailure:
                FinishGameUI.SetActive(true);
                FinishGameText.text = "Time is over..";
                break;
            default:
                break;
        }
    }
}
