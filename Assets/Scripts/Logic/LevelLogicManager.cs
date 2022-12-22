using System;

namespace Logic {
    public struct LevelLogicManager
    {
        private LevelSettings levelSettings;

        public LevelLogicManager(LevelSettings levelSettings)
        {
            this.levelSettings = levelSettings;
        }

        public LevelState UpdateLevel(PlayerInput player1Input,
            PlayerInput player2Input, LevelState currLevelState, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerMoving(Player playerState)
        {
            throw new NotImplementedException();
        }

        public bool IsTileBurning(Tile tileState) 
        {
            throw new NotImplementedException();
        }
    }
}
