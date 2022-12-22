using System;
using UnityEngine;

namespace Logic {
	public struct LevelGenerator
	{
		private const int InitialHP = 3;
		private Vector2Int InitialPlayer1Pos {get => new Vector2Int(0, 7);}
		private Vector2Int InitialPlayer2Pos {get => new Vector2Int(15, 7);}
		private const int InitialRocks = 40;
		private const int InitialWeeds = 40;

		/// <summary>
		/// Makes new grid with all static tiles and some weeds.
		/// </summary>
		/// <returns></returns>
		public LevelState InitialState(LevelSettings settings)
		{
            var grid = InitialGridState(settings);
            var player1 = InitialPlayer1State();
            var player2 = InitialPlayer2State();
            return new LevelState(grid, player1, player2);
        }

		private Grid InitialGridState(LevelSettings settings)
		{
            throw new NotImplementedException();
        }

        private Player InitialPlayer1State()
		{
            return new Player(InitialHP, InitialPlayer1Pos, Direction.Right);
        }

        private Player InitialPlayer2State()
        {
            return new Player(InitialHP, InitialPlayer2Pos, Direction.Left);
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