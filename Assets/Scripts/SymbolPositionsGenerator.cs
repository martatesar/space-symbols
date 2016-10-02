using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SymbolsPositionGenerator
{
    public static Vector3[] SymbolPositions =
    {
        new Vector3(0, 0, 10),
        new Vector3(0, 10, 0),
        new Vector3(10, 0, 0),
        new Vector3(-10, 0, 0),
        new Vector3(0, -10, 0),
        new Vector3(0, 0, -10),
        new Vector3(0, -10, -10),
        new Vector3(-10, 0, -10),
        new Vector3(-10, -10, 0),
        new Vector3(0, 10, 10),
        new Vector3(10, 0, 10),
        new Vector3(10, 10, 0),
        new Vector3(0, -10, 10),
        new Vector3(0, 10, -10),
        new Vector3(-10, 0, 10),
        new Vector3(10, 0, -10),
        new Vector3(-10, 10, 0),
        new Vector3(10, -10, 0),
        new Vector3(10, 10, 10),
        new Vector3(-10, -10, -10),
        new Vector3(-10, -10, 10),
        new Vector3(-10, 10, -10),
        new Vector3(10, -10, -10),
        new Vector3(-10, 10, 10),
        new Vector3(10, -10, 10),
        new Vector3(10, 10, -10),
    };

    public static IEnumerable<Vector3> GetSymbolPositionsForDifficulty(Game.EGameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case Game.EGameDifficulty.Easy:
                return RandomSymbolPositions().Take(5);
            case Game.EGameDifficulty.Hard:
                return RandomSymbolPositions();
            default:
                return RandomSymbolPositions();
        }
    }

    private static IEnumerable<Vector3> RandomSymbolPositions()
    {
        var list = SymbolPositions.ToList();
        while (list.Count > 0)
        {
            var idx = Random.Range(0, list.Count);
            yield return list[idx];
            list.RemoveAt(idx);
        }
    }
}


