using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PlaneGenerator : MonoBehaviour
{

    [SerializeField] private float cellSize = 4f;
    [SerializeField] private int width = 30;
    [SerializeField] private int height = 30;
    
    private List<Material> materialList;

    MeshGenerator mg;

    MeshFilter mf;

    MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mf = this.GetComponent<MeshFilter>();
        mr = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mg = new MeshGenerator(6);

        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(cellSize * x, Mathf.PingPong(x, 10), cellSize * y);
            }
        }

        int meshCount = 0;

        //Create the quads
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                mg.generateTriangle(bl, tr, tl, meshCount % 6);
                mg.generateTriangle(bl, br, tr, meshCount % 6);
            }
            meshCount++;
        }


        mf.mesh = mg.generateMesh();

        addMaterials();
        mr.materials = materialList.ToArray();
    }

    void addMaterials()
    {
        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        Material blueMat = new Material(Shader.Find("Specular"));
        blueMat.color = Color.blue;

        Material greenMat = new Material(Shader.Find("Specular"));
        greenMat.color = Color.green;

        Material orangeMat = new Material(Shader.Find("Specular"));
        orangeMat.color = Color.magenta;

        Material yellowMat = new Material(Shader.Find("Specular"));
        yellowMat.color = Color.yellow;

        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        materialList = new List<Material>();
        materialList.Add(redMat);
        materialList.Add(blueMat);
        materialList.Add(greenMat);
        materialList.Add(orangeMat);
        materialList.Add(yellowMat);
        materialList.Add(blackMat);
    }
}
