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
            currLevelState.player1 = HandleMovementInput(player1Input, currLevelState.player1, currLevelState.grid);
            currLevelState.player2 = HandleMovementInput(player2Input, currLevelState.player2, currLevelState.grid);
    
            return currLevelState;
        }

        private Player HandleMovementInput(PlayerInput input, Player playerState, Grid grid)
        {
            if (input.moveDirection is not Direction moveDirection || IsPlayerMoving(playerState) || 
                IsPlayerFrozenAfterAttack(playerState)) {
                return playerState;
            }
            Vector2Int nextTilePos = playerState.position + moveDirection.Vector();
            if (CanPlayerWalkThere(nextTilePos, grid))
            {
                return MovePlayer(playerState, moveDirection);
            }
            return playerState;
        }
        
        private Player TryWeedPickup(Player player, Grid grid)
        {
            throw new NotImplementedException();
        }

        private void UpdateGrid(Grid grid)
        {
            throw new NotImplementedException();
        }

        private Player HandleAttackInput(PlayerInput input, Player playerState, Grid grid)
        {
            throw new NotImplementedException();
        }


        private Player Attack(Grid grid, Player player, Direction direction, int range)
        {
            player.TimeSinceLastAttack = 0;
            Vector2Int currIndex = player.position + direction.Vector();
            Tile currTile;
            for (int i = 0; i < range; i++)
            {
                currTile = grid.GetTileByVectorIndex(currIndex);
                switch (currTile)
                {
                    case Rock:
                        return player;
                    case BurnableTile:
                        grid.tiles[currIndex.x, currIndex.y] = BurnTile((BurnableTile) currTile);
                        break;
                    default:
                        break;
                }
                currIndex += direction.Vector();
            }
            return player;
        }

        // returns a float between 0 to 1, if player is not even on weed - return 0
        public float WeedPickupProgression(Player player, Grid grid)
        {
            if (!IsPlayerOnWeed(player, grid)) return 0;
            // if (player.TimeSinceLastMove / settings.WeedPickupTime;
            throw new NotImplementedException();
        }

        public bool IsPlayerMoving(Player player)
        {
            return player.TimeSinceLastMove < settings.PlayerTileMoveTime;
        }

        public bool IsPlayerInvincible(Player player)
        {
            return player.TimeSinceLastAttack < settings.InvincibiltyFramesTime;
        }

        public bool IsTileBurning(BurnableTile tile)
        {
            return tile.TimeSinceBurnStart <= tile.BurnTime(settings);
        }

        public bool IsGameOver(LevelState levelState)
        {
            return levelState.player1.HP == 0 || levelState.player2.HP == 0;
        }

        private bool IsPlayerOnWeed(Player player, Grid grid)
        {
            return grid.tiles[player.position.x, player.position.y] is Weed;
        }

        private bool CanAttack(Player player)
        {
            return player.Ammo > 0 && !IsPlayerOnCooldown(player);
        }

        private bool IsPlayerOnCooldown(Player player) 
        {
            return player.TimeSinceLastAttack < settings.AttackCooldown;
        }

        private bool IsPlayerFrozenAfterAttack(Player player)
        {
            return player.TimeSinceLastAttack <= settings.PlayerAttackTime;
        }
        
        private BurnableTile BurnTile(BurnableTile tile)
        {
            tile.TimeSinceBurnStart = 0;
            return tile;
        }

        private Player MovePlayer(Player player, Direction direction)
        {
            player.position += direction.Vector();
            player.TimeSinceLastMove = 0;
            return player;
        }

        private bool CanPlayerWalkThere(Vector2Int dstPos, Grid grid)
        {
            if (dstPos.x >= settings.GridWidth || dstPos.x < 0 || dstPos.y >= settings.GridHeight || dstPos.y < 0)
            {
                return false;
            }   
            Tile nextTile = grid.tiles[dstPos.x, dstPos.y];
            return nextTile.IsWalkable;
        }
    }
}
