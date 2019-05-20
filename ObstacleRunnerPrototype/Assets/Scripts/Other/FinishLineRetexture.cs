using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleRunner.Other
{
    /// <summary>
    /// Component that creates Finsih Line Mesh on Runtime
    /// </summary>
    public class FinishLineRetexture : MonoBehaviour
    {
        [SerializeField]
        //FinishLine square X length
        private float boxX;
        [SerializeField]
        //FinishLine square Y length;
        private float boxY;

        //Materials Used
        [SerializeField]
        private Material blackMat;
        [SerializeField]
        private Material whiteMat;

        [SerializeField]
        private GameObject path;

        private int boxCountX;      //Generated
        private int boxCountY;      //Generated

        //Mesh Width, generated based on path GameObject
        private float meshWidth;
        //Mesh Height, generated base on path GameObject
        private float meshHeight;

        private void Start()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            Bounds b = path.GetComponent<MeshFilter>().mesh.bounds;

            //Calculate width and height
            meshWidth = b.size.x;
            meshHeight = b.size.y * 0.2f;

            //Create and Assign Mesh
            meshFilter.mesh = CreateMesh();
            //Assign Materials foreach SubMesh
            meshRenderer.materials = new Material[] { whiteMat, blackMat };

            //Fine tune Mesh positions incase not the whole mesh is coverd with squares
            float xoffset = (meshWidth - (boxCountX * boxX)) / 2;
            float yoffset = (meshHeight - (boxCountY * boxY));

            //Set Objects Position to match Path Object
            transform.position = path.transform.position + new Vector3(b.max.x + xoffset, 0.01f, b.max.y + yoffset)
                - new Vector3(meshWidth, 0, meshHeight);
        }

        /// <summary>
        /// Creates FinishLine Mesh
        /// </summary>
        /// <returns></returns>
        private Mesh CreateMesh()
        {
            //Calculate square counts per row/ column
            boxCountX = (int)(meshWidth / boxX);
            boxCountY = (int)(meshHeight / boxY);
            //total
            int boxCount = boxCountX * boxCountY;

            Vector3[] vertices = new Vector3[4 * boxCount];
            int[] triangles = new int[2 * 3 * boxCount];
            Vector3[] normals = new Vector3[4 * boxCount];
            Vector2[] uv = new Vector2[4 * boxCount];

            List<int> subMeshTriangles1 = new List<int>();
            List<int> subMeshTriangles2 = new List<int>();

            Mesh mesh = new Mesh();

            int subMeshIndex = 0;
            int rowCount = -1;

            //create a square (with two triangles and four vertices) on each iteration and assign it to proper Submesh
            for (int i = 0; i < boxCount; i++)
            {
                float currentX = (i % boxCountX) * boxX;
                if ((i % boxCountX) == 0)
                    rowCount++;

                if (boxCountX % 2 == 0)         //ensure we don't start with the same color each row
                {
                    if ((i % (boxCountX)) == 0 && i != 0)
                    {
                        subMeshIndex = (subMeshIndex + 1) % 2;
                    }
                }

                float currentY = rowCount * boxY;

                vertices[i * 4] = new Vector3(currentX, 0, currentY);
                vertices[i * 4 + 1] = new Vector3(currentX + boxX, 0, currentY);
                vertices[i * 4 + 2] = new Vector3(currentX, 0, currentY + boxY);
                vertices[i * 4 + 3] = new Vector3(currentX + boxX, 0, currentY + boxY);

                triangles[i * 4] = i * 4;
                triangles[i * 4 + 1] = i * 4 + 3;
                triangles[i * 4 + 2] = i * 4 + 1;

                triangles[i * 4 + 3] = i * 4;
                triangles[i * 4 + 4] = i * 4 + 2;
                triangles[i * 4 + 5] = i * 4 + 3;

                normals[i * 4] = Vector3.up;
                normals[i * 4 + 1] = Vector3.up;
                normals[i * 4 + 2] = Vector3.up;
                normals[i * 4 + 3] = Vector3.up;

                uv[i * 4] = new Vector2(0, 0);
                uv[i * 4 + 1] = new Vector2(0, 0);
                uv[i * 4 + 2] = new Vector2(0, 0);
                uv[i * 4 + 3] = new Vector2(0, 0);

                for (int j = i * 4; j < i * 4 + 6; j++)
                {
                    if (subMeshIndex == 0)
                        subMeshTriangles1.Add(triangles[j]);
                    else
                        subMeshTriangles2.Add(triangles[j]);
                }

                subMeshIndex = (subMeshIndex + 1) % 2;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.subMeshCount = 2;
            mesh.SetTriangles(subMeshTriangles1, 0);
            mesh.SetTriangles(subMeshTriangles2, 1);

            return mesh;
        }

    }
}
