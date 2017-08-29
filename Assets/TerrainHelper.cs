using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace XY
{ 
    /// <summary>
    /// calculate terrain height 
    /// </summary>
    public class TerrainHelper
    {
        //public Transform pivotTransform;
        //public Terrain terrain;

        private static bool IsInTerrainXYPlane(Terrain inTerr, int x, int y)
        {
            Vector2 size = inTerr.terrainData.size;
            return
                (x - inTerr.transform.position.x) < size.x && (x - inTerr.transform.position.x) > 0 &&
                (y - inTerr.transform.position.y) < size.y && (y - inTerr.transform.position.x) > 0;
        }

        public static float GetHeight(Terrain inTerr, Vector3 pos)
        {
            if (IsInTerrainXYPlane(inTerr, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)))
                return inTerr.terrainData.GetHeight(Mathf.RoundToInt(pos.x - inTerr.transform.position.x), Mathf.RoundToInt(pos.z - inTerr.transform.position.y));
            else
                return float.NaN;
        }

        public static bool IsVertexUnderTerrain(Terrain inTerr, Vector3 pos)
        {
            float height = GetHeight(inTerr, pos);
            if (float.IsNaN(height))
            {
                return false;
            }
            else
            {
                return height > pos.y ? true : false;
            }
        }
    }

}