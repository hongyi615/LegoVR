using UnityEngine;

public static class GridManager
{

    /// <summary>
    /// 计算任意点在网格中的最近格点坐标
    /// </summary>
    public static Vector3 GetNearestGridPoint(Vector3 position)
    {
        // 对x、y、z轴分别取最近的网格点（四舍五入到gridStep的整数倍）
        float x = Mathf.Round(position.x / 0.079f) * 0.079f;
        float y = Mathf.Round(position.y / 0.016f) * 0.016f;
        float z = Mathf.Round(position.z / 0.079f) * 0.079f;
        return new Vector3(x, y, z);
    }
    public static Quaternion GetNearestDirection(Quaternion currentRotation)
    {
        Vector3 euler = currentRotation.eulerAngles;
        float snappedY = RoundToNearestStep(euler.y, 90f);
        return Quaternion.Euler(0, snappedY, 0);
    }

    private static float RoundToNearestStep(float value, float step)
    {
        value = Mathf.Repeat(value, 360f);
        return Mathf.Round(value / step) * step;
    }
}