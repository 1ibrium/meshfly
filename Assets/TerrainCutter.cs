using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace XY
{
    [CustomEditor(typeof(TerrainCutter))]
    public class TerrainCutterEditor:Editor
    {
        TerrainCutter main;
        private void OnEnable()
        {
            main = target as TerrainCutter;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("SeletPath"))
            {
                Debug.Log(Application.dataPath);
                main.savePath = EditorUtility.OpenFolderPanel("Save Folder", "","");
                main.savePath = main.savePath.Remove(0, Application.dataPath.Length - 6);
            }
            base.OnInspectorGUI();
            if (GUILayout.Button("Back MeshFilters"))
            {
                main.ReplaceStaticsMeshFilter();
            }

        }
    }

    public class TerrainCutter : MonoBehaviour
    {
      //  public MeshFilter mf;
        public Terrain terrain;
        public string savePath = "";
        public static int removed = 0;
        // Use this for initialization

        public void ReplaceStaticsMeshFilter()
        {
            removed = 0;
            MeshFilter[] mfs = FindObjectsOfType<MeshFilter>();
            List<MeshFilter> mfsStatic = new List<MeshFilter>();
            foreach(var mf in mfs)
            {
                if (mf.gameObject.isStatic)
                    mfsStatic.Add(mf);
            }

            foreach (var mf in mfsStatic)
            {
                Mesh mesh = mf.sharedMesh;
                MeshController mc = new MeshController(mesh, mf.transform, terrain);
                mf.sharedMesh = mc.Find(savePath);
            }
            Debug.Log(removed+" triangle remove");
        }

    }
}