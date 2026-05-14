using UnityEngine;

public class DigManager : MonoBehaviour
{
    public Terrain terrain;
    public float digRadius = 0.5f;    // نصف قطر الحفر بالمتر
    public float digStrength = 0.001f; // عمق الحفر

    private TerrainData terrainData;
    private int heightmapWidth;
    private int heightmapHeight;

    void Start()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
        }

        terrainData = terrain.terrainData;
        heightmapWidth = terrainData.heightmapResolution;
        heightmapHeight = terrainData.heightmapResolution;
    }

    public void DigAtPoint(Vector3 worldPos)
    {
        Vector3 terrainPos = worldPos - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        int posX = (int)(normalizedPos.x * heightmapWidth);
        int posZ = (int)(normalizedPos.z * heightmapHeight);

        float metersPerPoint = terrainData.size.x / (float)heightmapWidth;
        int radius = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint));

        int startX = Mathf.Clamp(posX - radius, 0, heightmapWidth - 1);
        int startZ = Mathf.Clamp(posZ - radius, 0, heightmapHeight - 1);
        int width = Mathf.Clamp(radius * 2, 1, heightmapWidth - startX);
        int height = Mathf.Clamp(radius * 2, 1, heightmapHeight - startZ);

        float[,] heights = terrainData.GetHeights(startX, startZ, width, height);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float dx = x - radius;
                float dz = z - radius;
                float distance = Mathf.Sqrt(dx * dx + dz * dz);
                if (distance < radius)
                {
                    float strength = 1f - (distance / radius);
                    heights[z, x] -= digStrength * strength;
                    if (heights[z, x] < 0) heights[z, x] = 0;
                }
            }
        }

        terrainData.SetHeights(startX, startZ, heights);
    }
}