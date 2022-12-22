using UnityEngine;
using System.Collections;
using Logic;

namespace UI
{
	public class LevelGenerator : MonoBehaviour
	{
        [SerializeField] private float UnitDistance = 1;
		[SerializeField] private Weed WeedPrefab;
        [SerializeField] private Transform Rock1Prefab;
        [SerializeField] private Transform Rock2Prefab;
        [SerializeField] private PlayerController Player1Prefab;
        [SerializeField] private PlayerController Player2Prefab;

        public void GenerateInitialLevel(Transform parent,
            LevelState levelState, LevelSettings settings)
        {
            var player1Position = new Vector3(levelState.player1.position.x,
                levelState.player1.position.y);
            Instantiate(Player1Prefab, player1Position, Quaternion.identity, parent);
            var player2Position = new Vector3(levelState.player2.position.x,
                levelState.player2.position.y);
            Instantiate(Player2Prefab, player2Position, Quaternion.identity, parent);

            for (int i = 0; i < levelState.grid.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < levelState.grid.tiles.GetLength(1); j++)
                {
                    var tile = levelState.grid.tiles[i, j];
                    var position = new Vector3(i * UnitDistance, j * UnitDistance);

                    switch (tile)
                    {
                        case Logic.Weed:
                            Instantiate(WeedPrefab, position, Quaternion.identity, parent);
                            break;
                        case Logic.Rock:
                            var rand = Random.Range(0, 1);
                            Transform rockPrefab;

                            if (rand < 0.5f)
                            {
                                rockPrefab = Rock1Prefab;
                            }
                            else
                            {
                                rockPrefab = Rock2Prefab;
                            }

                            Instantiate(rockPrefab, position, Quaternion.identity, parent);
                            break;
                    }
                }
            }
        }
    }
}