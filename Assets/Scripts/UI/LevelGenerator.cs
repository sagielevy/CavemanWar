﻿using UnityEngine;
using System.Collections;
using Logic;
using System;

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

        public Tuple<PlayerController, PlayerController, Transform[,]> GenerateInitialLevel(Transform parent,
            LevelState levelState, LevelSettings settings)
        {
            var player1Position = new Vector3(levelState.player1.position.x,
                levelState.player1.position.y);
            var player1 = Instantiate(Player1Prefab, player1Position, Quaternion.identity, parent);
            var player2Position = new Vector3(levelState.player2.position.x,
                levelState.player2.position.y);
            var player2 = Instantiate(Player2Prefab, player2Position, Quaternion.identity, parent);

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
                            grid[i, j] = MakeWeed(index, parent);
                            break;
                        case Logic.Rock:
                            grid[i, j] = MakeRock(index, parent);
                            break;
                        default:
                            break;
                    }
                }
            }

            return new(player1, player2, grid);
        }

        public Transform MakeRock(Vector2Int index, Transform parent)
        {
            var rand = UnityEngine.Random.Range(0, 1);
            Transform rockPrefab;
            var position = new Vector3(index.x * UnitDistance,
                index.y * UnitDistance);

            if (rand < 0.5f)
            {
                rockPrefab = Rock1Prefab;
            }
            else
            {
                rockPrefab = Rock2Prefab;
            }

            return Instantiate(rockPrefab, position,
                Quaternion.identity, parent).transform;
        }

        public Transform MakeWeed(Vector2Int index, Transform parent)
        {
            var position = new Vector3(index.x * UnitDistance,
                index.y * UnitDistance);

            return Instantiate(WeedPrefab, position,
                Quaternion.identity, parent).transform;
        }
    }
}