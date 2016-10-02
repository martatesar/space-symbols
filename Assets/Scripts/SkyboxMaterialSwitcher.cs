using UnityEngine;
using System.Collections;

public class SkyboxMaterialSwitcher : MonoBehaviour {

    public Material HardGameMaterial;
    public Material EasyGameMaterial;

    private Game Game;

    void Awake()
    {
        Game = Game.Instance;
        Game.GameStateChangeEvent += OnGameStateChangeEvent;
    }

    void Destroy()
    {
        Game.GameStateChangeEvent -= OnGameStateChangeEvent;
    }

    private void OnGameStateChangeEvent(Game.EGameState state)
    {
        if (state == Game.EGameState.StartNewGame)
        {
            var skyBox = Camera.main.GetComponent<Skybox>();
            skyBox.material = Game.GameDifficulty == Game.EGameDifficulty.Easy
                ? EasyGameMaterial
                : HardGameMaterial;
        }
    }
}
