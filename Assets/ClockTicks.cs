using UnityEngine;

public class ClockTicks : MonoBehaviour
{
    public int tickCount = 12;
    public float radius = 3.5f;
    public float tickLength = 0.3f;
    public float tickWidth = 0.1f;
    public float tickDepth = 0.1f;
    public float longTickScale = 1.8f;
    public Material normalMat;
    public Material longMat;

    void Start()
    {
        GenerateTicks();
    }

    void GenerateTicks()
    {
        for (int i = 0; i < tickCount; i++)
        {
            GameObject tick = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tick.name = "Tick_" + i;
            tick.transform.SetParent(transform);

            float angleDeg = i * (360f / tickCount);
            float angleRad = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Sin(angleRad) * radius;
            float y = Mathf.Cos(angleRad) * radius;
            tick.transform.localPosition = new Vector3(x, y, 0);

            tick.transform.localRotation = Quaternion.Euler(0, 0, -angleDeg);

            bool isLong = (i == 0);
            float len = isLong ? tickLength * longTickScale : tickLength;
            tick.transform.localScale = new Vector3(tickWidth, len, tickDepth);

            var renderer = tick.GetComponent<Renderer>();
            if (isLong && longMat != null)
                renderer.sharedMaterial = longMat;
            else if (normalMat != null)
                renderer.sharedMaterial = normalMat;
        }
    }
}