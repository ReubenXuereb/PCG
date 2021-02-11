using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    private List<Vector3> vertices = new List<Vector3>(); 
    private List<int> indices = new List<int>(); 
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int>[] submeshIndices = new List<int>[] { };
  


    public MeshGenerator(int meshCounter)
    {
        submeshIndices = new List<int>[meshCounter];

        for (int i = 0; i < meshCounter; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    public void generateTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int meshCount)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        generateTrueTriangle(p0, p1, p2, normal, meshCount);
    }

    public void generateTrueTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int meshCount)
    {

        //Getting index of each vertex within the list of vertices
        int p0PosInList = vertices.Count;
        int p1PosInList = vertices.Count + 1;
        int p2IPosInList = vertices.Count + 2;

        // Add the index of each vertex to the indices
        indices.Add(p0PosInList);
        indices.Add(p1PosInList);
        indices.Add(p2IPosInList);
        submeshIndices[meshCount].Add(p0PosInList);
        submeshIndices[meshCount].Add(p1PosInList);
        submeshIndices[meshCount].Add(p2IPosInList);

        //Add each point to our vertices List
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

        //Add normals to our normals List
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        //Add each UV coordinates to our uvs List
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }

    public Mesh generateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        return mesh;
    }


}
