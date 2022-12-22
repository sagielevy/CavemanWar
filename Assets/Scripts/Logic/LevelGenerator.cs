using System;

namespace Logic {
	public struct LevelGenerator
	{
		/// <summary>
		/// Makes new grid with all static tiles and some weeds.
		/// </summary>
		/// <returns></returns>
		public Grid InitialState(LevelSettings settings)
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