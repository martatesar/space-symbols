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
    public event System.Action<Symbol> ActualSymbolChangedEvent
    {
        add { symbols.ActualSymbolChangedEvent += value;  }
        remove { symbols.ActualSymbolChangedEvent -= value; }
    }

    public GameObject SymbolPrefab;
    public EGameDifficulty GameDifficulty { get { return difficulty; } }

    private SymbolsHolder symbols = new SymbolsHolder();
    private EGameState actualState;
    private EGameDifficulty difficulty;

    public Symbol ActualSymbol
    {
        get { return symbols.ActualSymbol; }
    }

    void Awake()
    {
        Instance = this;

        Symbol.FullHitSymbolEvent += Symbol_FullHitSymbolEvent;
    }

    void Start()
    {
        SetState(EGameState.Idle);
    }

    public void StartNew(EGameDifficulty difficulty)
    {
        this.difficulty = difficulty;
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
                nextState = symbols.Count == 0
                    ? EGameState.GameSuccess
                    : EGameState.GameFailure;
                symbols.RemoveAll();
                break;
            default:
                break;
        }

        if (newState != actualState)
        {
            actualState = newState;
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
        symbols.FillWithNewSymbols(SymbolPrefab, difficulty);
    }

    private void Symbol_FullHitSymbolEvent(Symbol symbol)
    {
        if(symbols.ActualSymbol != symbol)
            return;

        symbols.Remove(symbol);

        if (symbols.Count == 0)
        {
            SetState(EGameState.FinishGame);
        }
        else
        {
            symbols.SetNextSymbolAsActual();
        }
    }
}

   
