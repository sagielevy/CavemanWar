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
            return new LevelState(grid, player1, player2);
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
				Vector2Int pos = GenerateRandomPosInGrid(settings.GridWidth, settings.GridHeight);
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
				Vector2Int pos = GenerateRandomPosInGrid(settings.GridWidth, settings.GridHeight);
				if (pos != settings.InitialPlayer1Pos && pos != settings.InitialPlayer2Pos)
				{
					grid.tiles[pos.x, pos.y] = new Weed();
				}
			}
		}

		private Vector2Int GenerateRandomPosInGrid(int width, int height)
		{
			return new Vector2Int(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height));
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
        public Grid SpawnNewWeeds(Grid previousState, LevelSettings settings,
			float deltaTime)
		{
			throw new NotImplementedException();
		}
	}
}