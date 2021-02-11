using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class TriangleGenerator : MonoBehaviour
{
    private List<Material> materialList;

    [SerializeField] Vector3 size;

    MeshGenerator mg;

    MeshFilter mf;

    MeshRenderer mr;

   
    void Start()
    {
        size = Vector3.one;
        mf = this.GetComponent<MeshFilter>();
        mr = this.GetComponent<MeshRenderer>();
    }

   
    void Update()
    {
        mg = new MeshGenerator(1);

        Vector3 p0 = new Vector3(size.x, size.y, -size.z);
        Vector3 p1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 p2 = new Vector3(-size.x, size.y, size.z);

        mg.generateTriangle(p0, p1, p2, 0);
        mf.mesh = mg.generateMesh();

        addMaterials();
        mr.materials = materialList.ToArray();  
    }

    void addMaterials()
    {
        Material redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = Color.red;

        materialList = new List<Material>();
        materialList.Add(new Material(Shader.Find("Specular")));
        materialList.Add(redMaterial);
    }
}
