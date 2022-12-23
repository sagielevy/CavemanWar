using System;
using System.Linq;
using UnityEngine;

namespace Logic
{
    public static class ExtensionMethods
    {
        public static Vector2Int Vector(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Vector2Int(0, 1),
                Direction.Down => new Vector2Int(0, -1),
                Direction.Right => new Vector2Int(1, 0),
                Direction.Left => new Vector2Int(-1, 0)
            };
        }

        public readonly static Vector2Int[] AllDirectionVectors = Enum.GetValues(typeof(Direction))
            .Cast<Direction>().Select(x => x.Vector()).ToArray();
    }
}