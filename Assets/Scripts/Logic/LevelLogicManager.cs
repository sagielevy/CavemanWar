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

        public LevelState UpdateLevel(LevelGenerator levelGenerator, PlayerInput player1Input,
            PlayerInput player2Input, LevelState currLevelState, 
            float deltaTime)
        {
            var grid = CloneGrid(currLevelState.grid);
            var player1 = HandleMovementInput(player1Input, currLevelState.player1, grid);
            var player2 = HandleMovementInput(player2Input, currLevelState.player2, grid);

            player1 = HandleWeedPickup(player1, grid);
            player2 = HandleWeedPickup(player2, grid);

            UpdateGrid(grid, deltaTime);

            player1 = HandleAttackInput(player1Input, player1, grid);
            player2 = HandleAttackInput(player2Input, player2, grid);

            player1 = HandlePlayerHit(player1, grid);
            player2 = HandlePlayerHit(player2, grid);

            player1 = UpdatePlayerCounters(player1, deltaTime);
            player2 = UpdatePlayerCounters(player2, deltaTime);

            var newLevelState = new LevelState(grid, player1, player2,
                currLevelState.currentWeedSpawnRate, currLevelState.timeSinceLastSpawn);

            return HandleWeedSpawn(newLevelState, levelGenerator, deltaTime);
        }

        private Grid CloneGrid(Grid origin)
        {
            var width = origin.tiles.GetLength(0);
            var height = origin.tiles.GetLength(1);
            var result = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = origin.tiles[i, j];
                }
            }

            return new(result);
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
        
        private Player HandleWeedPickup(Player player, Grid grid)
        {
            if (WeedPickupProgression(player, grid) == 1)
            {
                player.Ammo += 1;
                grid.tiles[player.position.x, player.position.y] = new Ground();
            }
            return player;
        }

        private void UpdateGrid(Grid grid, float deltaTime)
        {
            Tile currTile;
            for (int i = 0; i < grid.tiles.GetLength(0); i++)
			{
                for (int j = 0; j < grid.tiles.GetLength(1); j++)
                {
                    currTile = grid.tiles[i, j];

                    if (currTile is BurnableTile burnable) 
                    {
                        if (!IsTileBurning(burnable) && burnable.TimeSinceBurnStart.HasValue) 
                        {
                            grid.tiles[i, j] = new Ground(null);
                            currTile = grid.tiles[i, j];
                        }
                        else if (!burnable.TimeSinceBurnStart.HasValue && burnable is Weed weed && 
                            GetMaxTimeSinceBurnNeighbours(grid, i, j) > settings.WeedCatchFireTime)
                        {
                            weed.TimeSinceBurnStart = 0;
                            grid.tiles[i , j] = weed;
                            currTile = weed;
                        }
                    }

                    switch(currTile)
                    {
                        case Weed weed:
                            weed.TimeSinceBurnStart += deltaTime;
                            weed.TimeSinceSpawn += deltaTime;
                            grid.tiles[i, j] = weed;
                            break;
                        case Ground ground:
                            ground.TimeSinceBurnStart += deltaTime;
                            grid.tiles[i, j] = ground;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private float GetMaxTimeSinceBurnNeighbours(Grid grid, int x, int y) 
        {
            var maxTimeSinceBurn = float.MinValue;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || i >= settings.GridWidth || j < 0 || j >= settings.GridHeight ||
                        grid.tiles[i, j] is not Weed weedTile) { continue; }
                    
                    var neighbourTime = weedTile.TimeSinceBurnStart ?? float.MinValue;
                    maxTimeSinceBurn = Mathf.Max(maxTimeSinceBurn, neighbourTime);
                }
            }

            return maxTimeSinceBurn;
        }

        private Player HandleAttackInput(PlayerInput input, Player playerState, Grid grid)
        {
            if (input.didTryAttack && CanAttack(playerState))
            {
                playerState.Ammo -= 1;
                return Attack(grid, playerState, settings.FlamethrowerRange);
            }

            return playerState;
        }

        private Player HandlePlayerHit(Player player, Grid grid)
        {
            if (!IsPlayerInvincible(player) &&
                grid.tiles[player.position.x, player.position.y] is BurnableTile burnableTile &&
                IsTileBurning(burnableTile))
            {
                player.HP -= 1;
                player.TimeSinceLastHit = 0;
            }

            return player;
        }

        private LevelState HandleWeedSpawn(LevelState levelState, LevelGenerator levelGenerator,
            float deltaTime)
        {
            var (hasSpawned, newGrid) = levelGenerator.SpawnNewWeeds(levelState, settings, deltaTime);
            var currentWeedSpawnRate = Mathf.Max(levelState.currentWeedSpawnRate -
                settings.WeedSpawnSpeedCurvePercent * deltaTime,
                settings.WeedMinimalSpawnTime);

            var timeSinceLastSpawn = hasSpawned ? 0 : levelState.timeSinceLastSpawn + deltaTime;

            return new LevelState()
            {
                currentWeedSpawnRate = currentWeedSpawnRate,
                timeSinceLastSpawn = timeSinceLastSpawn,
                grid = newGrid,
                player1 = levelState.player1,
                player2 = levelState.player2
            };
        }

        private Player UpdatePlayerCounters(Player player, float deltaTime)
        {
            player.TimeSinceLastAttack += deltaTime;
            player.TimeSinceLastHit += deltaTime;
            player.TimeSinceLastMove += deltaTime;
            return player;
        }

        private Player Attack(Grid grid, Player player, int range)
        {
            player.TimeSinceLastAttack = 0;
            Vector2Int currIndex = player.position;
            Tile currTile;
            for (int i = 0; i < range; i++)
            {
                currIndex += player.orientation.Vector();
                if (currIndex.x < 0 || currIndex.x >= settings.GridWidth || currIndex.y < 0 || currIndex.y >= settings.GridHeight)
                {
                    return player;
                }
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
            return player.TimeSinceLastHit < settings.InvincibiltyFramesTime;
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
