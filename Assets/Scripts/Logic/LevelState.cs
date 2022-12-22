using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic 
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct Player
    {
        public int HP;
        public int Ammo;
        public float? TimeSinceLastHit;
        public float? TimeSinceLastAttack;
        public float? TimeSinceLastMove;
        public Vector2Int position;
        public Direction orientation;
    }

    public interface Tile
    {
        bool IsWalkable { get; }
    }

    public struct Rock : Tile
    {
        public bool IsWalkable => false;
    }

    public struct Ground : Tile
    {
        public float? TimeSinceBurnStart;

        public bool IsWalkable => true;
    }

    public struct Weed: Tile
    {
        public float? TimeSinceBurnStart;

        public bool IsWalkable => true;
    }

    [Serializable]
    public struct LevelSettings
    {
        public float WeedInitialSpawnTime;
        public float WeedSpawnSpeedCurvePercent;
        public float WeedBurnTime;
        public float GroudBurnTime;
        public float WeedPickupTime;
        public float WeedCatchFireTime;
        public int FlamethrowerRange;
        public float InvincibiltyFramesTime;
        public float AttackCooldown;
        public float PlayerTileMoveTime;
    }

    public struct PlayerInput
    {
        public Direction moveDirection;
        public bool didAttack;
    }

    public struct Grid
    {
        public Tile[,] tiles;

        public Grid(Tile[,] tiles)
        {
            this.tiles = tiles;
        }

        public Grid(int width, int height)
        {
            tiles = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = new Ground();
                }
            }
        }
    }

    public struct LevelState
    {
        public Grid grid;
        public Player player1;
        public Player player2;

        public LevelState(Grid grid, Player player1, Player player2)
        {
            this.grid = grid;
            this.player1 = player1;
            this.player2 = player2;
        }
    }
}
