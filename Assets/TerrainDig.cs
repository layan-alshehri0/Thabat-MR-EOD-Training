using UnityEngine;

public class TerrainDig : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    public int digRadius = 5;      // نصف قطر الحفر
    public float digDepth = 0.001f; // عمق الحفر لكل ضغطة

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // للحين مجرد اختبار بالماوس
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == terrain.gameObject)
                {
                    DigAtPoint(hit.point);
                }
            }
        }
    }

    void DigAtPoint(Vector3 point)
    {
        int xRes = terrainData.heightmapResolution;
        int yRes = terrainData.heightmapResolution;

        int x = (int)((point.x / terrainData.size.x) * xRes);
        int y = (int)((point.z / terrainData.size.z) * yRes);

        float[,] heights = terrainData.GetHeights(x - digRadius, y - digRadius, digRadius * 2, digRadius * 2);

        for (int i = 0; i < digRadius * 2; i++)
        {
            for (int j = 0; j < digRadius * 2; j++)
            {
                heights[i, j] -= digDepth;
                if (heights[i, j] < 0) heights[i, j] = 0; // ما ينزل تحت الصفر
            }
        }

        terrainData.SetHeights(x - digRadius, y - digRadius, heights);
    }
}

