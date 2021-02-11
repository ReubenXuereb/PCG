using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureDataForSecondTask
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeDataForSecondTask
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}


[ExecuteInEditMode]
public class TerrainGeneratorForSecondTask : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    //options in editor
    [SerializeField] private bool generateTerrain = true;

    [SerializeField] private bool generatePerlinNoiseTerrain = false;

    [SerializeField] private bool flattenTerrain = false;

    [SerializeField] private bool addTexture = false;

    [SerializeField] private bool removeTexture = false;

    [SerializeField] private bool addTree = false;

    [SerializeField] private bool addWater = false;
    [SerializeField] private bool removeWater = false;

    [SerializeField] private bool addCloud = false;

    [SerializeField] private bool addRain = false;




    //variables for generating terrain using random values
    [SerializeField]
    private float minRandomHeightRange = 0f;

    [SerializeField]
    private float maxRandomHeightRange = 0.01f;

    //variables for generating terrain using perlin noise
    [SerializeField]
    private float perlinNoiseWidthScale = 0.01f;

    [SerializeField]
    private float perlinNoiseHeightScale = 0.01f;

    /*[SerializeField]
    private float perlinNoiseOffsetWidth = 1, perlinNosieOffsetHeight = 1;*/

    //variables for adding textures to our terrain
    [SerializeField]
    private List<TerrainTextureData> terrainTexturesList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [SerializeField]
    private List<TreeData> treeDataList;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treeSpacing = 10;

    [SerializeField] private float randomXRange = 5f;

    [SerializeField] private float randomZRange = 5f;

    //sends the lenght of our raycast
    RaycastHit raycastHit;
    [SerializeField] private int terrainLayerIndex = 8;

    [SerializeField] private GameObject water;

    [SerializeField] private float waterHeight = 0.2f;

    [SerializeField] private GameObject cloud;
        

    [SerializeField] private GameObject rain;



    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;
       

        CreateTerrain();
    }

    void Update()
    {

    }
    void initialise()
    {
#if UNITY_EDITOR

        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }
#endif
    }


    private void OnValidate()
    {
        initialise();

        if (flattenTerrain)
        {
            generateTerrain = false;
            generatePerlinNoiseTerrain = false;
        }

        if (generateTerrain || flattenTerrain || generatePerlinNoiseTerrain)
        {
            CreateTerrain();
        }

        if (removeTexture)
        {
            addTexture = false;
        }

        if (addTexture || removeTexture)
        {
            TerrainTexture();
        }

        if (addTree)
        {
            generateTree();
        }

        if (addWater)
        {
            generateWater();
        }

        if (removeWater)
        {
            Destroy(GameObject.Find("water"));
        }

        if (addCloud == true)
        {
            generateClouds();
        }

        if (addRain == true)
        {
            generateRain();
        }

    }


    void CreateTerrain()
    {
        //creates a new empty 2D array of float based on the dimensions of heightmap resolution set in the settings
        //float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        ////gets the height map data that already exists in the terrain and loads it into a 2D array
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (generateTerrain)
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }

                //for loop here
                if (generatePerlinNoiseTerrain)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }

                if (flattenTerrain)
                {
                    heightMap[width, height] = 0;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    //this method is going to add textures to the terrain
    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTexturesList.Count];

        for (int i = 0; i < terrainTexturesList.Count; i++)
        {
            if (addTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTexturesList[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTexturesList[i].tileSize;
            }
            else if (removeTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;


        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTexturesList.Count; i++)
                {
                    float minHeight = terrainTexturesList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTexturesList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTexturesList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }


    void NormaliseSplatMap(float[] splatmap)
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
    }

    //Task2.1
    void generateTree()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }
        terrainData.treePrototypes = trees;


        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if (addTree)
        {
            //looping through terrain
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for (int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {
                        if (treeInstanceList.Count < maxTrees)
                        {
                            float currHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if (currHeight >= treeDataList[treeIndex].minHeight && currHeight <= treeDataList[treeIndex].maxHeight)
                            {
                                float randomXco = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                                float randomZco = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                                TreeInstance treeInstance = new TreeInstance();
                                treeInstance.position = new Vector3(randomXco, currHeight, randomZco);
                                Vector3 treePos = new Vector3(randomXco * terrainData.size.x, currHeight * terrainData.size.y, randomZco * terrainData.size.z) + this.transform.position;


                                //treeInstance.position = treePos;
                                int layerMask = 1 << terrainLayerIndex;
                                if (Physics.Raycast(treePos, -Vector3.down, out raycastHit, 100, layerMask) || Physics.Raycast(treePos, -Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;
                                    treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);
                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 1f;
                                    treeInstance.widthScale = 1f;

                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            }
        }

        terrainData.treeInstances = treeInstanceList.ToArray();

    }

    //Task2.2
    void generateWater()
    {
        GameObject waterGameObject = GameObject.Find("water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y, terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(1008.24548f, 1, 1021.3067f);
    }

    //Task2.2
    void generateClouds()
    {
        GameObject cloudGameObject = GameObject.Find("cloud");
        GameObject CloudHolder = GameObject.Find("Clouds");
       

        if (!cloudGameObject)
        {
            for (int i = 0; i <= 30; i++)
            {

                cloudGameObject = Instantiate(cloud, this.transform.position, this.transform.rotation);
                cloudGameObject.transform.position = new Vector3(Random.Range(0, 2000), 1390, Random.Range(0, 2000));
                cloudGameObject.name = "cloud";
                cloud.transform.parent = CloudHolder.transform;
            }
        }     
    }

    void generateRain()
    {
        GameObject rainGameObject = GameObject.Find("rain");

        if (!rainGameObject)
        {
            rainGameObject = Instantiate(rain, this.transform.position, this.transform.rotation);
            rainGameObject.transform.position = new Vector3(Random.Range(0, 5000), 1390, Random.Range(0, 5000));
            rainGameObject.name = "rain";
        }
    }
}
