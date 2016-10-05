using UnityEngine;
using System.Collections;

public class SkyboxMaterialSwitcher : MonoBehaviour {

    public Material HardGameMaterial;
    public Material EasyGameMaterial;

    private Game game;

    void Awake()
    {
        game = Game.Instance;
        game.GameStateChangeEvent += OnGameStateChangeEvent;
    }

    void Destroy()
    {
        game.GameStateChangeEvent -= OnGameStateChangeEvent;
    }

    private void OnGameStateChangeEvent(Game.EGameState state)
    {
        if (state == Game.EGameState.StartNewGame)
        {
            var skyBox = Camera.main.GetComponent<Skybox>();
            skyBox.material = game.GameDifficulty == Game.EGameDifficulty.Easy
                ? EasyGameMaterial
                : HardGameMaterial;
        }
    }
}
