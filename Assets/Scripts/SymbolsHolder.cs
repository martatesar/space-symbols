using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SymbolsHolder
{
    public event System.Action<Symbol> ActualSymbolChangedEvent;

    Symbol _actualSymbol;
    public Symbol ActualSymbol
    {
        private set
        {
            if (_actualSymbol != value)
            {
                _actualSymbol = value;
                if (ActualSymbolChangedEvent != null)
                    ActualSymbolChangedEvent(_actualSymbol);
            }
        }
        get { return _actualSymbol; }
    }
    public int Count { get { return symbolsList.Count; } }

    List<Symbol> symbolsList = new List<Symbol>();

    public void FillWithNewSymbols(GameObject symbolPrefab, Game.EGameDifficulty difficulty)
    {
        RemoveAll();

        var positions = SymbolsPositionGenerator.GetSymbolPositionsForDifficulty(difficulty);

        var index = 0;
        foreach (var position in positions)
        {
            // create gameobject
            var newSymbolObject = GameObject.Instantiate(symbolPrefab);
            newSymbolObject.transform.position = position;
            newSymbolObject.transform.LookAt(Vector3.zero);
            newSymbolObject.SetActive(true);

            // set data
            var newSymbol = newSymbolObject.GetComponent<Symbol>();
            newSymbol.SetIndex(index++);

            symbolsList.Add(newSymbol);
        }

        ActualSymbol = symbolsList.FirstOrDefault(s => s.Index == 0);
    }

    public void SetNextSymbolAsActual()
    {
        if (ActualSymbol == null)
            return;

        ActualSymbol = symbolsList.FirstOrDefault(s => s.Index == ActualSymbol.Index + 1);
    }

    public void Remove(Symbol symbol)
    {
        symbol.Kill();
        symbolsList.Remove(symbol);
    }

    public void RemoveAll()
    {
        while (symbolsList.Count > 0)
            Remove(symbolsList[0]);
    }

}
