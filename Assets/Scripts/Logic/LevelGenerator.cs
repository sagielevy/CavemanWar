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
            throw new NotImplementedException();
        }

		private Vector2Int GenerateRandomPosInGrid(Grid grid)
		{
			// return new Vector2Int(UnityEngine.Random.RandomRange(0, grid.))
            throw new NotImplementedException();
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