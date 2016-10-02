using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SymbolsHolder
{
    // delegates
    public System.Action<Symbol, float> OnHittingSymbol { get; set; }
    public System.Action<Symbol> OnFullHitSymbol { get; set; }
    public System.Action<Symbol> OnActualSymbolChanged { get; set; }

    public int Count { get { return SymbolsList.Count; } }

    private Symbol _actualSymbol;
    public Symbol ActualSymbol
    {
        private set
        {
            if(_actualSymbol != value)
            {
                _actualSymbol = value;
                if (OnActualSymbolChanged != null)
                    OnActualSymbolChanged(_actualSymbol);
            }

        }
        get { return _actualSymbol; }
    }

    private List<Symbol> SymbolsList = new List<Symbol>();
    private GameObject SymbolPrefab;

    public void SetSymbolPrefab(GameObject symbolPrefab)
    {
        SymbolPrefab = symbolPrefab;
    }

    public void FillWithNewSymbols(Game.EGameDifficulty difficulty)
    {
        if (SymbolsList.Count > 0)
        {
            RemoveAll();
        }

        var positions = SymbolsPositionGenerator.GetSymbolPositionsForDifficulty(difficulty);

        var index = 0;
        foreach (var position in positions)
        {
            // create gameobject
            var newSymbolObject = GameObject.Instantiate(SymbolPrefab);
            newSymbolObject.transform.position = position;
            newSymbolObject.transform.LookAt(Vector3.zero);
            newSymbolObject.SetActive(true);

            // set data
            var newSymbol = newSymbolObject.GetComponent<Symbol>();
            newSymbol.SetIndex(index++);
            RegisterSymbolEvents(newSymbol);

            SymbolsList.Add(newSymbol);
        }

        ActualSymbol = SymbolsList.FirstOrDefault(s => s.Index == 0);
    }


    public void SetNextSymbolAsActual()
    {
        if (ActualSymbol == null)
            return;

        ActualSymbol = SymbolsList.FirstOrDefault(s => s.Index == ActualSymbol.Index + 1);
    }

    public void Remove(Symbol symbol)
    {
        UnRegisterSymbolEvents(symbol);
        Object.Destroy(symbol.gameObject);
        SymbolsList.Remove(symbol);
    }

    public void RemoveAll()
    {
        foreach (var symbol in SymbolsList)
        {
            UnRegisterSymbolEvents(symbol);
            Object.Destroy(symbol.gameObject);
        }

        SymbolsList.Clear();
    }

    private void RegisterSymbolEvents(Symbol symbol)
    {
        symbol.FullHitSymbolEvent += OnFullHitSymbol;
        symbol.HittingSymbolEvent += OnHittingSymbol;
    }

    private void UnRegisterSymbolEvents(Symbol symbol)
    {
        symbol.FullHitSymbolEvent -= OnFullHitSymbol;
        symbol.HittingSymbolEvent -= OnHittingSymbol;
    }
}
