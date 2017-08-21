using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainHelper))]
public class TerrainHelperEditor : Editor
{
    TerrainHelper main;
    private void OnEnable()
    {
        main = target as TerrainHelper;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if( GUILayout.Button("Give Hight"))
        {
            Debug.Log(TerrainHelper.GetHeight(main.terrain, main.pivotTransform.position));
        }
    }
}

/// <summary>
/// calculate terrain height 
/// </summary>
public class TerrainHelper : MonoBehaviour {
    public Transform pivotTransform;
    public Terrain terrain;

    public static bool IsInTerrainXYPlane(Terrain inTerr, int x, int y)
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
            return -1;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
