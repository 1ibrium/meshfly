using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace XY
{
    /// <summary>
    /// class for mesh vertex 
    /// </summary>
    public class MeshController
    {
        public struct VectorData
        {
            public Vector3 vertex;
            public int index;
            public bool isUnderTerrain;
            public VectorData(Vector3 vertex, int index, bool isUnderTerrain)
            {
                this.vertex = vertex;
                this.index = index;
                this.isUnderTerrain = isUnderTerrain;
            }
        }

        public Mesh mesh;
        public Mesh newMesh;
        public Transform meshTransform;
        public List<VectorData> vertexGlobalPos;

        private Matrix4x4 lToWMaxtrix;
        public MeshController(Mesh mesh, Transform meshTransform, Terrain terrain)
        {
            newMesh = Mesh.Instantiate<Mesh>(mesh);
            newMesh.name = mesh.name + "_CUTTED";
            this.mesh = mesh;
            this.meshTransform = meshTransform;
            lToWMaxtrix = meshTransform.localToWorldMatrix;

            vertexGlobalPos = new List<VectorData>();
            Vector3[] originVertice = mesh.vertices;
            for (int i = 0; i < originVertice.Length; i++)
            {
                Vector3 wPos = meshTransform.TransformPoint(originVertice[i]);

                vertexGlobalPos.Add
                (
                    new VectorData
                    (
                        wPos,
                        i,
                        TerrainHelper.IsVertexUnderTerrain(terrain, wPos)
                     )
                );

            }
        }

        private bool IsTriangleUnder(int pos1, int pos2, int pos3)
        {
            VectorData pos1V3 = vertexGlobalPos.Find((VectorData a) => { return a.index == pos1; });
            VectorData pos2V3 = vertexGlobalPos.Find((VectorData a) => { return a.index == pos2; });
            VectorData pos3V3 = vertexGlobalPos.Find((VectorData a) => { return a.index == pos3; });

            return pos1V3.isUnderTerrain && pos2V3.isUnderTerrain && pos3V3.isUnderTerrain;
        }

        public Mesh Find(string savePath)
        {
            //find useless triangle
            int[] indices = mesh.GetIndices(0);
            List<int> singleDogTriangleIndex = new List<int>();
            List<int> savedNewTriangleIndex = new List<int>();
            //step1 find on rubbish triangle
            for (int i = 0; i <  indices.Length; i= i+3)
            {
                bool needRemove = IsTriangleUnder(indices[i], indices[i+1], indices[i+2]);
                if (needRemove)
                {
                    singleDogTriangleIndex.Add(i);
                    TerrainCutter.removed++;
                }
                else
                {
                    savedNewTriangleIndex.Add(indices[i]);
                    savedNewTriangleIndex.Add(indices[i+1]);
                    savedNewTriangleIndex.Add(indices[i+2]);
                }
            }

            //reform index
            int[] newIndices;
            ListToArray(out newIndices, savedNewTriangleIndex);
            newMesh.SetIndices(newIndices, newMesh.GetTopology(0),0);
            if (!string.IsNullOrEmpty(savePath))
            {
                savePath = savePath + '/'+ newMesh.name +'_' +newIndices.GetHashCode()+".asset";
                Debug.Log(savePath);
                AssetDatabase.CreateAsset(newMesh, savePath);
                AssetDatabase.ImportAsset(savePath);
                return AssetDatabase.LoadAssetAtPath<Mesh>(savePath);
            }
            return newMesh;
        }




        public static void ListToArray<T>(out T[] output, List<T> origin)
        {
            output = new T[origin.Count];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = origin[i];
            }
        }
    }
 }
