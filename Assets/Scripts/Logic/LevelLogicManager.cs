using System;
using UnityEngine;

namespace Logic 
{
    public struct LevelLogicManager
    {
        private LevelSettings settings;

        public LevelLogicManager(LevelSettings levelSettings)
        {
            this.settings = levelSettings;
        }

        public LevelState UpdateLevel(PlayerInput player1Input,
            PlayerInput player2Input, LevelState currLevelState, 
            float deltaTime)
        {
            throw new NotImplementedException();   // TODO
        }

        private void HandleMovementInput(PlayerInput input, Player playerState)
        {
            if (input.moveDirection.HasValue)
            {
                // playerState. COPY STATE AND RETURN NEW (IN ANOTHER FUNC)
            }
        }

        private void Attack(Grid grid, Vector2Int playerPos, Direction direction, int range)
        {
            Vector2Int currIndex = playerPos + direction.Vector();
            Tile currTile;
            for (int i = 0; i < range; i++)
            {
                currTile = grid.GetTileByVectorIndex(currIndex);
                switch (currTile)
                {
                    case Rock:
                        return;
                    case BurnableTile:
                        // grid[i,j] = BurnTile((BurnableTile) currTile);
                        break;
                    default:
                        break;
                }
                currIndex += direction.Vector();
            }
        }

        public float PickupProgression(Player player)
        {
            // return float between 0 to 1, maybe use Math.lerp
            throw new NotImplementedException();   // TODO
        }

        public bool IsPlayerMoving(Player player)
        {
            return !player.TimeSinceLastMove.HasValue || player.TimeSinceLastMove < settings.PlayerTileMoveTime;
        }

        public bool IsPlayerInvincible(Player player)
        {
            throw new NotImplementedException(); // TODO
        }

        public bool IsTileBurning(BurnableTile tile)
        {
            return !tile.TimeSinceBurnStart.HasValue || tile.TimeSinceBurnStart <= tile.BurnTime(settings);
        }

        public bool IsGameOver(LevelState levelState)
        {
            return levelState.player1.HP == 0 || levelState.player2.HP == 0;
        }

        private bool CanAttack(Player player)
        {
            return player.Ammo > 0 && !IsPlayerOnCooldown(player);
        }

        private bool IsPlayerOnCooldown(Player player) 
        {
            return player.TimeSinceLastAttack < settings.AttackCooldown;
        }
        
        private BurnableTile BurnTile(BurnableTile tile)
        {
            tile.TimeSinceBurnStart = 0;
            throw new NotImplementedException(); // TODO
        }
    }
}
