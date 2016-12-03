using UnityEngine;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static Quaternion RotateToTarget(Vector3 currentPosition, Vector3 targetVector)
        {
            var vectorToTarget = targetVector - currentPosition;
            var angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
