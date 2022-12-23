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

        private void UpdateGrid(Grid grid, float deltaTime)
        {
            throw new NotImplementedException();
        }

        private Player HandleAttackInput(PlayerInput input, Player playerState, Grid grid)
        {
            throw new NotImplementedException();
        }

        private Player HandlePlayerHit(Player player, Grid grid)
        {
            throw new NotImplementedException();
        }

        private void HandleWeedSpawn(Grid grid, float currentWeedSpawnRate, 
            float timeSinceLastSpawn, LevelGenerator levelGenerator)
        {
            throw new NotImplementedException();
        }

        private Player UpdatePlayerCounters(Player player, float deltaTime)
        {
            throw new NotImplementedException();
        }

        private Player Attack(Grid grid, Player player, int range)
        {
            player.TimeSinceLastAttack = 0;
            Vector2Int currIndex = player.position + player.orientation.Vector();
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
                currIndex += player.orientation.Vector();
            }
            return player;
        }

        // returns a float between 0 to 1. if player is not even on weed - return 0. if can pick - return 1.
        public float WeedPickupProgression(Player player, Grid grid)
        {
            Weed? tile = IsPlayerOnWeed(player, grid);
            if (tile is not Weed weed) return 0;
            if (!player.TimeSinceLastMove.HasValue)
            {
                return Mathf.Clamp01(weed.TimeSinceSpawn / settings.WeedPickupTime);
            } else {
                float shorterTime = Mathf.Min((float) player.TimeSinceLastMove, weed.TimeSinceSpawn);
                return Mathf.Clamp01(shorterTime);
            }
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

        private Weed? IsPlayerOnWeed(Player player, Grid grid)
        {
            Tile tile = grid.tiles[player.position.x, player.position.y];
            return tile is Weed ? (Weed) tile : null;
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
            player.orientation = direction;
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
