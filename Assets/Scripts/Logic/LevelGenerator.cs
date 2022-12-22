using System;
using UnityEngine;

namespace Logic {
	public struct LevelGenerator
	{
		/// <summary>
		/// Makes new grid with all static tiles and some weeds.
		/// </summary>
		/// <returns></returns>
		public LevelState InitialState(LevelSettings settings)
		{
            var grid = InitialGridState(settings);
            var player1 = InitialPlayer1State(settings);
            var player2 = InitialPlayer2State(settings);
            return new LevelState(grid, player1, player2, settings.WeedInitialSpawnTime, 0);
        }

		private Grid InitialGridState(LevelSettings settings)
		{
			var grid = new Grid(settings.GridWidth, settings.GridHeight);
			GenerateRocksInGrid(grid, settings);
			GenerateWeedsInGrid(grid, settings);
			return grid;
		}

		private void GenerateRocksInGrid(Grid grid, LevelSettings settings)
		{
			for (int i = 0; i <= settings.InitialRocks; i++)
			{
				Vector2Int pos = GenerateRandomPosInGrid(grid, settings.GridWidth, settings.GridHeight);
				if (pos != settings.InitialPlayer1Pos && pos != settings.InitialPlayer2Pos)
				{
					grid.tiles[pos.x, pos.y] = new Rock();
				}
			}
		}

		private void GenerateWeedsInGrid(Grid grid, LevelSettings settings)
		{
			for (int i = 0; i <= settings.InitialWeeds; i++)
			{
				Vector2Int pos = GenerateRandomPosInGrid(grid, settings.GridWidth, settings.GridHeight);
				if (pos != settings.InitialPlayer1Pos && pos != settings.InitialPlayer2Pos)
				{
					grid.tiles[pos.x, pos.y] = new Weed();
				}
			}
		}

		private Vector2Int GenerateRandomPosInGrid(Grid grid, int width, int height)
		{
			var index = new Vector2Int(UnityEngine.Random.Range(0, width),
				UnityEngine.Random.Range(0, height));
			var tileAtIndex = grid.tiles[index.x, index.y];

            while (tileAtIndex is Weed || tileAtIndex is Rock)
			{
                index = new Vector2Int(UnityEngine.Random.Range(0, width),
					UnityEngine.Random.Range(0, height));
                tileAtIndex = grid.tiles[index.x, index.y];
            }

			return index;
		}

        private Player InitialPlayer1State(LevelSettings settings)
		{
            return new Player(settings.InitialHP, settings.InitialPlayer1Pos, Direction.Right);
        }

        private Player InitialPlayer2State(LevelSettings settings)
        {
            return new Player(settings.InitialHP, settings.InitialPlayer2Pos, Direction.Left);
        }

        /// <summary>
        /// Generate new weeds.
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public Grid SpawnNewWeeds(LevelState previousState, LevelSettings settings,
			float deltaTime)
		{
			throw new NotImplementedException();
		}
	}
}