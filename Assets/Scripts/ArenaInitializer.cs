using System.Collections.Generic;
using UnityEngine;

public class ArenaInitializer : MonoBehaviour
{
    public float[] ringRadii;
    public List<List<Vector3>> tilePositions = new List<List<Vector3>>();
    public int tilesPerRing = 24;

    void Awake()
    {
        InitializeTilePositions(); 
    }

    void InitializeTilePositions()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < ringRadii.Length; i++)
        {
            List<Vector3> ringPositions = new List<Vector3>();
            int numberOfTiles = tilesPerRing;
            float sliceAngle = 360f / numberOfTiles;
            for (int j = 0; j < numberOfTiles; j++)
            {
                Vector3 tilePosition = CalculateTilePosition(center, ringRadii[i], sliceAngle, j);
                ringPositions.Add(tilePosition);
            }
            tilePositions.Add(ringPositions);
        }
    }

    Vector3 CalculateTilePosition(Vector3 center, float radius, float sliceAngle, int tileIndex)
    {
        float angleDegrees = sliceAngle * tileIndex;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        Vector3 position = new Vector3(
            center.x + radius * Mathf.Cos(angleRadians),
            center.y,
            center.z + radius * Mathf.Sin(angleRadians)
        );
        return position;
    }

    public string GetTileNameByPosition(Vector3 position)
    {
        float closestDistanceSqr = Mathf.Infinity;
        string closestTileName = "";
        for (int ringIndex = 0; ringIndex < tilePositions.Count; ringIndex++)
        {
            for (int tileIndex = 0; tileIndex < tilePositions[ringIndex].Count; tileIndex++)
            {
                Vector3 tilePosition = tilePositions[ringIndex][tileIndex];
                float distanceSqr = (position - tilePosition).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    if (tileIndex <= 8)
                    {
                        closestTileName = $"R{ringIndex + 1}_0{tileIndex + 1}";
                    }
                    else
                    {
                        closestTileName = $"R{ringIndex + 1}_{tileIndex + 1}";
                    }
                }
            }
        }
        return closestTileName;
    }
}


public class Tile : MonoBehaviour
{
    public Vector3 centerPoint;
    public int ringNumber;
}
