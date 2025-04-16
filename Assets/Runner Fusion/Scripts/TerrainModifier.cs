using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public Terrain terrain; // Reference to Terrain
    public BezierPathManager pathManager; // Reference to Bezier Path Manager
    public int textureIndex = 1; // Texture layer to paint
    public int brushSize = 5; // Width of the painted path
    public float raiseHeight = 2.0f; // Height increment

    void Start()
    {
        ModifyTerrainAlongCurves();
    }

    private void ModifyTerrainAlongCurves()
    {
        if (terrain == null || pathManager == null)
        {
            Debug.LogError("Terrain or BezierPathManager not assigned!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;
        float terrainHeight = terrainData.size.y; // Max terrain height
        float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        ProcessCurve(heights, alphamaps, pathManager.GetLeftPath(), terrainData, terrainHeight);
        ProcessCurve(heights, alphamaps, pathManager.GetRightPath(), terrainData, terrainHeight);

        // ?? **Apply Height Changes**
        terrainData.SetHeights(0, 0, heights);
        terrainData.SyncHeightmap(); // **Forces terrain update**

        // ?? **Apply Texture Painting**
        terrainData.SetAlphamaps(0, 0, alphamaps);

        Debug.Log("? Terrain modified successfully! Heights and textures updated.");
    }

    private void ProcessCurve(float[,] heights, float[,,] alphamaps, Vector3[] path, TerrainData terrainData, float terrainHeight)
    {
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;
        Vector3 terrainSize = terrainData.size;

        foreach (Vector3 point in path)
        {
            // **Convert world position to terrain heightmap coordinates**
            int x = Mathf.RoundToInt((point.x / terrainSize.x) * heightmapWidth);
            int z = Mathf.RoundToInt((point.z / terrainSize.z) * heightmapHeight);

            if (x < 0 || x >= heightmapWidth || z < 0 || z >= heightmapHeight)
                continue; // Prevent out-of-bounds errors

            for (int i = -brushSize; i <= brushSize; i++)
            {
                for (int j = -brushSize; j <= brushSize; j++)
                {
                    int newX = Mathf.Clamp(x + i, 0, heightmapWidth - 1);
                    int newZ = Mathf.Clamp(z + j, 0, heightmapHeight - 1);

                    // **Raise Terrain (Normalize Raise Height)**
                    heights[newZ, newX] = Mathf.Clamp(heights[newZ, newX] + (raiseHeight / terrainHeight), 0, 1);

                    // **Paint Texture**
                    int texX = Mathf.RoundToInt((point.x / terrainSize.x) * alphamapWidth);
                    int texZ = Mathf.RoundToInt((point.z / terrainSize.z) * alphamapHeight);
                    int texNewX = Mathf.Clamp(texX + i, 0, alphamapWidth - 1);
                    int texNewZ = Mathf.Clamp(texZ + j, 0, alphamapHeight - 1);

                    for (int t = 0; t < terrainData.alphamapLayers; t++)
                    {
                        alphamaps[texNewZ, texNewX, t] = (t == textureIndex) ? 1.0f : 0.0f;
                    }
                }
            }
        }
    }
}
