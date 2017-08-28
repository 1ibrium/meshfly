using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XY
{

    public class TerrainCutter : MonoBehaviour
    {
        public MeshFilter mf;
        public Terrain terrain;
        // Use this for initialization
        void Start()
        {
            //Debug
            Mesh mesh = mf.sharedMesh;
            MeshController mc = new MeshController(mesh, mf.transform, terrain);
            mc.Find();
        }
            
    }
}