using System;

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
            throw new NotImplementedException();
        }

        private Player InitialPlayer2State()
        {
            throw new NotImplementedException();
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