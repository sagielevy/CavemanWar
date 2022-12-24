using UnityEngine;
using System.Collections;
using Logic;
using System;

namespace UI
{
	public class LevelGenerator : MonoBehaviour
	{
        [SerializeField] private GroundFire GroundFirePrefab;
        [SerializeField] private Weed WeedPrefab;
        [SerializeField] private Transform Rock1Prefab;
        [SerializeField] private Transform Rock2Prefab;
        [SerializeField] private Transform Rock3Prefab;
        [SerializeField] private PlayerController Player1Prefab;
        [SerializeField] private PlayerController Player2Prefab;

        public Tuple<PlayerController, PlayerController, Transform[,]> GenerateInitialLevel(
            Transform parent, LevelState levelState, LevelSettings settings)
        {
            var player1RelativePos = levelState.player1.position - BoardCenter(settings);
            var player1Position = new Vector3(player1RelativePos.x,
                player1RelativePos.y, -2);
            var player1 = Instantiate(Player1Prefab, player1Position, Quaternion.identity, parent);

            var hearts = GameObject.FindWithTag("Hearts1").GetComponentsInChildren<Heart>();
            player1.Setup(hearts, levelState.player1.orientation);

            var player2RelativePos = levelState.player2.position - BoardCenter(settings);
            var player2Position = new Vector3(player2RelativePos.x,
                player2RelativePos.y, -2);
            var player2 = Instantiate(Player2Prefab, player2Position, Quaternion.identity, parent);

            hearts = GameObject.FindWithTag("Hearts2").GetComponentsInChildren<Heart>();
            player2.Setup(hearts, levelState.player2.orientation);

            var width = levelState.grid.tiles.GetLength(0);
            var height = levelState.grid.tiles.GetLength(1);
            var grid = new Transform[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var tile = levelState.grid.tiles[i, j];
                    var index = new Vector2Int(i, j);

                    switch (tile)
                    {
                        case Logic.Weed:
                            grid[i, j] = MakeWeed(index, parent, settings, 0);
                            break;
                        case Logic.Rock:
                            grid[i, j] = MakeRock(index, parent, settings);
                            break;
                        case Logic.Ground:
                            grid[i, j] = MakeGroundFire(index, parent, settings);
                            break;
                    }
                }
            }

            return new(player1, player2, grid);
        }

        public Transform MakeRock(Vector2Int index, Transform parent,
            LevelSettings settings)
        {
            var rand = UnityEngine.Random.Range(0.0f, 1.0f);
            Transform rockPrefab;

            var boardCenter = BoardCenter(settings);
            var position = new Vector3(index.x - boardCenter.x,
                index.y - boardCenter.y);

            if (rand < 0.33f)
            {
                rockPrefab = Rock1Prefab;
            }
            else if (rand < 0.66f)
            {
                rockPrefab = Rock2Prefab;
            }
            else
            {
                rockPrefab = Rock3Prefab;
            }

            return Instantiate(rockPrefab, position,
                Quaternion.identity, parent).transform;
        }

        public Transform MakeWeed(Vector2Int index, Transform parent,
            LevelSettings settings, float fadeInDuration)
        {
            var boardCenter = BoardCenter(settings);
            var position = new Vector3(index.x - boardCenter.x,
                index.y - boardCenter.y);

            var go = Instantiate(WeedPrefab, position,
                Quaternion.identity, parent);
            var fadeTile = go.GetComponent<FadeableTile>();

            fadeTile.SetOpacity(0);
            fadeTile.Fade(1, fadeInDuration);

            return go.transform;
        }

        public Transform MakeGroundFire(Vector2Int index, Transform parent,
            LevelSettings settings)
        {
            var boardCenter = BoardCenter(settings);
            var position = new Vector3(index.x - boardCenter.x,
                index.y - boardCenter.y);

            return Instantiate(GroundFirePrefab, position,
                Quaternion.identity, parent).transform;
        }

        public void ResetLevel(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child == parent) { continue; }

                Destroy(child.gameObject);
            }
        }

        private Vector2 BoardCenter(LevelSettings settings)
        {
            return new Vector2(settings.GridWidth / 2.0f,
                settings.GridHeight / 2.0f);
        }
    }
}