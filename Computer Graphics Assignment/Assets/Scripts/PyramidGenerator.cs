using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidGenerator : MonoBehaviour
{

    [SerializeField] float pyramidSize = 5f;
    private List<Material> materialList;

    MeshGenerator mg;

    MeshFilter mf;

    MeshRenderer mr;
  
    void Start()
    {
        mf = this.GetComponent<MeshFilter>();
        mr = this.GetComponent<MeshRenderer>();
    }

   
    void Update()
    {

        mg = new MeshGenerator(4);

        //Add points
        Vector3 top = new Vector3(0, pyramidSize, 0);

        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;

        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;

        Vector3 base2 = Quaternion.AngleAxis(1200f, Vector3.up) * Vector3.forward * pyramidSize;


        //Build the triangles for our pyramid
        mg.generateTriangle(base0, base1, base2, 0);

        mg.generateTriangle(base1, base0, top, 1);

        mg.generateTriangle(base2, top, base0, 2);

        mg.generateTriangle(top, base2, base1, 3);


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

        materialList = new List<Material>();
        materialList.Add(redMat);
        materialList.Add(blueMat);
        materialList.Add(greenMat);
        materialList.Add(orangeMat);
    }
}
