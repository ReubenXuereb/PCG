using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class CubeGenerator : MonoBehaviour
{
    [SerializeField] Vector3 size;
    private List<Material> materialList;

    MeshGenerator mg;

    MeshFilter mf;

    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        size = Vector3.one;
        mf = this.GetComponent<MeshFilter>();
        mr = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mg = new MeshGenerator(6);

        //top vertices
        Vector3 t0 = new Vector3(size.x, size.y, -size.z);
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 t2 = new Vector3(-size.x, size.y, size.z);
        Vector3 t3 = new Vector3(size.x, size.y, size.z);

        //low vertices
        Vector3 b0 = new Vector3(size.x, -size.y, -size.z);
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z);
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z);
        Vector3 b3 = new Vector3(size.x, -size.y, size.z);




        mg.generateTriangle(b2, b1, b0, 0);
        mg.generateTriangle(b3, b2, b0, 0);

        //top square
        mg.generateTriangle(t0, t1, t2, 1);
        mg.generateTriangle(t0, t2, t3, 1);

        //back square
        mg.generateTriangle(b0, t1, t0, 2);
        mg.generateTriangle(b0, b1, t1, 2);

        //left-side square
        mg.generateTriangle(b1, t2, t1, 3);
        mg.generateTriangle(b1, b2, t2, 3);

        //right-side square
        mg.generateTriangle(b2, t3, t2, 4);
        mg.generateTriangle(b2, b3, t3, 4);

        //front-side square
        mg.generateTriangle(b3, t0, t3, 5);
        mg.generateTriangle(b3, b0, t0, 5);



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
