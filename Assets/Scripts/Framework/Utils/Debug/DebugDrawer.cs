using UnityEngine;

namespace Framework.Utils.Debug
{
    public static class DebugDrawer
    {
        public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration = 0)
        {
            var lbb = center + rotation * ((-size) * 0.5f);
            var rbb = center + rotation * (new Vector3(size.x, -size.y, -size.z) * 0.5f);

            var lbf = center + rotation * (new Vector3(size.x, -size.y, size.z) * 0.5f);
            var rbf = center + rotation * (new Vector3(-size.x, -size.y, size.z) * 0.5f);

            var lub = center + rotation * (new Vector3(-size.x, size.y, -size.z) * 0.5f);
            var rub = center + rotation * (new Vector3(size.x, size.y, -size.z) * 0.5f);

            var luf = center + rotation * ((size) * 0.5f);
            var ruf = center + rotation * (new Vector3(-size.x, size.y, size.z) * 0.5f);

            UnityEngine.Debug.DrawLine(lbb, rbb, color, duration);
            UnityEngine.Debug.DrawLine(rbb, lbf, color, duration);
            UnityEngine.Debug.DrawLine(lbf, rbf, color, duration);
            UnityEngine.Debug.DrawLine(rbf, lbb, color, duration);

            UnityEngine.Debug.DrawLine(lub, rub, color, duration);
            UnityEngine.Debug.DrawLine(rub, luf, color, duration);
            UnityEngine.Debug.DrawLine(luf, ruf, color, duration);
            UnityEngine.Debug.DrawLine(ruf, lub, color, duration);

            UnityEngine.Debug.DrawLine(lbb, lub, color, duration);
            UnityEngine.Debug.DrawLine(rbb, rub, color, duration);
            UnityEngine.Debug.DrawLine(lbf, luf, color, duration);
            UnityEngine.Debug.DrawLine(rbf, ruf, color, duration);
        }

        public static void DrawCircle(Vector3 position, Vector3 upVector, float radius, Color color, float duration = 0)
        {
            var up = upVector.normalized * radius;
            var forward = Vector3.Slerp(up, -up, 0.5f);
            var right = Vector3.Cross(up, forward).normalized * radius;

            var matrix = new Matrix4x4();

            matrix[0] = right.x;
            matrix[1] = right.y;
            matrix[2] = right.z;

            matrix[4] = up.x;
            matrix[5] = up.y;
            matrix[6] = up.z;

            matrix[8] = forward.x;
            matrix[9] = forward.y;
            matrix[10] = forward.z;

            var lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            var nextPoint = Vector3.zero;

            for (var i = 0; i < 91; i++)
            {
                nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                nextPoint.y = 0;

                nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);

                UnityEngine.Debug.DrawLine(lastPoint, nextPoint, color, duration);
                lastPoint = nextPoint;
            }
        }

        public static void DrawWireSphere(Vector3 position, float radius, Color color, float duration = 0)
        {
            const float angle = 10.0f;

            var x = new Vector3(position.x, position.y + radius * Mathf.Sin(0), position.z + radius * Mathf.Cos(0));
            var y = new Vector3(position.x + radius * Mathf.Cos(0), position.y, position.z + radius * Mathf.Sin(0));
            var z = new Vector3(position.x + radius * Mathf.Cos(0), position.y + radius * Mathf.Sin(0), position.z);

            Vector3 newX;
            Vector3 newY;
            Vector3 newZ;

            for (int i = 1; i < 37; i++)
            {

                newX = new Vector3(position.x, position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad));
                newY = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y, position.z + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad));
                newZ = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z);

                UnityEngine.Debug.DrawLine(x, newX, color, duration);
                UnityEngine.Debug.DrawLine(y, newY, color, duration);
                UnityEngine.Debug.DrawLine(z, newZ, color, duration);

                x = newX;
                y = newY;
                z = newZ;
            }
        }
    }
}