using UnityEngine;

namespace TD.Utils
{
    public static class Extensions
    {
        public static int Distance(this Vector2Int vector1, Vector2Int vector2)
        {
            return Mathf.Abs((vector1.x - vector2.x)) +
                       Mathf.Abs((vector1.y - vector2.y));
        }
    }
}