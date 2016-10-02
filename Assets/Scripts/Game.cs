using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum EGameState
    {
        None,
        Idle,
        InGame,
        FinishGame,
        GameSuccess,
        GameFailure,
        StartNewGame,
    }

    public enum EGameDifficulty
    {
        Easy,
        Hard
    }

    public static Game Instance {get; private set;}

    public event System.Action<EGameState> GameStateChangeEvent;
    public event System.Action<Symbol> ActualSymbolChangeEvent;

    public GameObject SymbolPrefab;
    public EGameDifficulty GameDifficulty { get { return Difficulty; } }

    private SymbolsHolder Symbols = new SymbolsHolder();
    private EGameState ActualState;
    private EGameDifficulty Difficulty;

    void Awake()
    {
        Instance = this;
        Symbols.OnFullHitSymbol = OnFullHitSymbol;
        Symbols.OnHittingSymbol = OnHittingSymbol;
        Symbols.OnActualSymbolChanged = OnActualSymbolChanged;
        Symbols.SetSymbolPrefab(SymbolPrefab);
    }

    void Start()
    {
        SetState(EGameState.Idle);
    }

    public void StartNew(EGameDifficulty difficulty)
    {
        Difficulty = difficulty;
        SetState(EGameState.StartNewGame);
    }

    public void Finish()
    {
        SetState(EGameState.FinishGame);
    }

    public void SetIdleMode()
    {
        SetState(EGameState.Idle);
    }

    private void SetState(EGameState newState)
    {
        EGameState? nextState = null;
        switch (newState)
        {
            case EGameState.Idle:
                break;
            case EGameState.StartNewGame:
                PrepareNewGame();
                nextState = EGameState.InGame;
                break;
            case EGameState.InGame:
                break;
            case EGameState.FinishGame:
                nextState = Symbols.Count == 0
                    ? EGameState.GameSuccess
                    : EGameState.GameFailure;
                Symbols.RemoveAll();
                break;
            default:
                break;
        }

        if (newState != ActualState)
        {
            ActualState = newState;
            if (GameStateChangeEvent != null)
            {
                GameStateChangeEvent(newState);
            }
        }

        if (nextState.HasValue)
        {
            SetState(nextState.Value);
        }
    }

    private void PrepareNewGame()
    {
        Symbols.FillWithNewSymbols(Difficulty);
    }

    private void OnHittingSymbol(Symbol symbol, float lapsedNormalized)
    {
        if (Symbols.ActualSymbol != symbol)
            return;

        symbol.PlayParticles();

        Handheld.Vibrate();

        symbol.SetAlpha(1 - lapsedNormalized);
    }

    private void OnFullHitSymbol(Symbol symbol)
    {
        if (Symbols.ActualSymbol != symbol)
            return;

        Symbols.Remove(symbol);

        if (Symbols.Count == 0)
        {
            SetState(EGameState.FinishGame);
        }
        else
        {
            Symbols.SetNextSymbolAsActual();
        }
    }

    private void OnActualSymbolChanged(Symbol symbol)
    {
        if (ActualSymbolChangeEvent != null)
            ActualSymbolChangeEvent(symbol);
    }
}

   
