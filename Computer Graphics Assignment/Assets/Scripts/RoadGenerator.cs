using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoadGenerator : MonoBehaviour
{
    [SerializeField]
    public float radius = 30f; // this defines the radius of the path

    [SerializeField]
    private float segments = 300f;

    [SerializeField]
    private float lineWidth = 0.3f; // middle white line road marker

    [SerializeField]
    private float roadWidth = 8f; // width of the road on each side of the line

    [SerializeField]
    private float edgeWidth = 1f; // widht of our road barrier at the edge of our road

    [SerializeField]
    private float edgeHeight = 1f;

    [SerializeField]
    private int meshSize = 6;

    [SerializeField]
    private float wavyness = 5f;

    [SerializeField]
    private float waveScale = 0.1f;

    [SerializeField]
    private Vector2 waveOffset;

    [SerializeField]
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    [SerializeField]
    private bool stripeCheck = true;

    [SerializeField]
    private GameObject car;

    MeshGenerator mg;

    MeshFilter mf;

    MeshRenderer mr;

    MeshCollider mc;

    private List<Material> materialList;

    void Start()
    {
         mf = this.GetComponent<MeshFilter>();

         mc = this.GetComponent<MeshCollider>();

        mr = this.GetComponent<MeshRenderer>();

        mg = new MeshGenerator(meshSize);


        //1. Divide the circular race track into segments denoted in degrees and each point is defined by each segment
        //Create the points and store them in a list. This is to create different shapes for the track
        float segmentDegrees = 360f / segments;

        List<Vector3> points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
        }

        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            //this is to eliminate having sharp turns on our road. This ensures the given values are within range.
            float smoothJoining = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * smoothJoining;
        }

        //2. function to define the path - the path is defined by each segment
        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            generateRoad(mg, pPrev, pCurr, pNext);
        }

        car.transform.position = points[0];
        car.transform.LookAt(points[1]);



        mf.mesh = mg.generateMesh();

        mc.sharedMesh = mf.mesh;

        addMaterial();
        mr.materials = materialList.ToArray();
    }

    //3. This method will used to create the different segments for each segment we are going to draw the road marker 
    //   (i.e. white line in the middle), draw the road on each side of the line, draw the edges - all these are going 
    //   to be placed in different positions
    private void generateRoad(MeshGenerator mg, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {
        //Road Line
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;

        generateRoadQuad(mg, pPrev, pCurr, pNext, offset, targetOffset, 0);

        //Road
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        generateRoadQuad(mg, pPrev, pCurr, pNext, offset, targetOffset, 1);

        //to create 
        int stripeSubmesh = 2;

        if (stripeCheck)
        {
            stripeSubmesh = 3;
        }

        stripeCheck = !stripeCheck;

        //Increasing the barrier height for the outer part of the track
        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;

        generateRoadQuad(mg, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Increasing the width of the barriers
        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;

        generateRoadQuad(mg, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Increasing the barrier height for the inner part of the track
        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;

        generateRoadQuad(mg, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

    }

    //4. Create each quad
    private void generateRoadQuad(MeshGenerator mg, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        //Build Outer Track
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        mg.generateTriangle(topLeft, topRight, bottomLeft, submesh);
        mg.generateTriangle(topRight,bottomRight,bottomLeft, submesh);

        //Build Inner Track
        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        mg.generateTriangle(bottomLeft, bottomRight, topLeft, submesh);
        mg.generateTriangle(bottomRight, topRight, topLeft, submesh);
    }

    void addMaterial()
    {
        //White material for the middle line and barrier
        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        //Black material for the tarmak
        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        //Red material for the barrier
        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;


        materialList = new List<Material>();
        materialList.Add(whiteMat);
        materialList.Add(blackMat);
        materialList.Add(redMat);
        materialList.Add(whiteMat);
    }
}
