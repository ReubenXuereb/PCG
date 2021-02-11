using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScript : MonoBehaviour
{

   // CubeGenerator cg;

    public GameObject mazeGenerator;

    // Start is called before the first frame update
    void Start()
    {
        Borders();
        Horizontals();
        Vertical();
    }

    //placment of cubes to form the border
    void Borders()
    {
        var border1 = new GameObject().AddComponent<CubeGenerator>();
        border1.transform.position = new Vector3(39, 5, 0);
        border1.transform.Rotate(90, 0, 0);
        border1.transform.localScale = new Vector3(1, 20, 5);
        border1.transform.parent = mazeGenerator.transform;
        border1.name = "Border 1";

        var border2 = new GameObject().AddComponent<CubeGenerator>();
        border2.transform.position = new Vector3(10, 5, -19);
        border2.transform.Rotate(90, 90, 0);
        border2.transform.localScale = new Vector3(1, 10, 5);
        border2.transform.parent = mazeGenerator.transform;
        border2.name = "Border 2";

        var border3 = new GameObject().AddComponent<CubeGenerator>();
        border3.transform.position = new Vector3(35, 5, -19);
        border3.transform.Rotate(90, 90, 0);
        border3.transform.localScale = new Vector3(1, 5, 5);
        border3.transform.parent = mazeGenerator.transform;
        border3.name = "Border 3";

        var border4 = new GameObject().AddComponent<CubeGenerator>();
        border4.transform.position = new Vector3(30, 5, 19);
        border4.transform.Rotate(90, 90, 0);
        border4.transform.localScale = new Vector3(1, 10, 5);
        border4.transform.parent = mazeGenerator.transform;
        border4.name = "Border 4";

        var border5 = new GameObject().AddComponent<CubeGenerator>();
        border5.transform.position = new Vector3(1, 5, 0);
        border5.transform.Rotate(90, 0, 0);
        border5.transform.localScale = new Vector3(1, 20, 5);
        border5.transform.parent = mazeGenerator.transform;
        border5.name = "Border 5";

        var border6 = new GameObject().AddComponent<CubeGenerator>();
        border6.transform.position = new Vector3(5, 5, 19);
        border6.transform.Rotate(90, 90, 0);
        border6.transform.localScale = new Vector3(1, 5, 5);
        border6.transform.parent = mazeGenerator.transform;
        border6.name = "Border 6";
    }

    //placment of cubes to form the horizontal walls
    void Horizontals()
    {
        var horizontal1 = new GameObject().AddComponent<CubeGenerator>();
        horizontal1.transform.position = new Vector3(5, 5, -9);
        horizontal1.transform.Rotate(90, 90, 0);
        horizontal1.transform.localScale = new Vector3(1, 5, 5);
        horizontal1.transform.parent = mazeGenerator.transform;
        horizontal1.name = "Horizontal 1";

        var horizontal2 = new GameObject().AddComponent<CubeGenerator>();
        horizontal2.transform.position = new Vector3(25, 5, -9);
        horizontal2.transform.Rotate(90, 90, 0);
        horizontal2.transform.localScale = new Vector3(1, 5, 5);
        horizontal2.transform.parent = mazeGenerator.transform;
        horizontal2.name = "Horizontal 2";

        var horizontal3 = new GameObject().AddComponent<CubeGenerator>();
        horizontal3.transform.position = new Vector3(20, 5, 9);
        horizontal3.transform.Rotate(90, 90, 0);
        horizontal3.transform.localScale = new Vector3(1, 10, 5);
        horizontal3.transform.parent = mazeGenerator.transform;
        horizontal3.name = "Horizontal 3";

        var horizontal4 = new GameObject().AddComponent<CubeGenerator>();
        horizontal4.transform.position = new Vector3(15, 5, 1);
        horizontal4.transform.Rotate(90, 90, 0);
        horizontal4.transform.localScale = new Vector3(1, 5, 5);
        horizontal4.transform.parent = mazeGenerator.transform;
        horizontal4.name = "Horizontal 4";


    }

    //placment of cubes to form the verical walls
    void Vertical()
    {
        var vertical1 = new GameObject().AddComponent<CubeGenerator>();
        vertical1.transform.position = new Vector3(31, 5, 5);
        vertical1.transform.Rotate(90, 0, 0);
        vertical1.transform.localScale = new Vector3(1, 5, 5);
        vertical1.transform.parent = mazeGenerator.transform;
        vertical1.name = "Vertical 1";

        var vertical2 = new GameObject().AddComponent<CubeGenerator>();
        vertical2.transform.position = new Vector3(19, 5, 15);
        vertical2.transform.Rotate(90, 0, 0);
        vertical2.transform.localScale = new Vector3(1, 5, 5);
        vertical2.transform.parent = mazeGenerator.transform;
        vertical2.name = "Vertical 2";

        var vertical3 = new GameObject().AddComponent<CubeGenerator>();
        vertical3.transform.position = new Vector3(11, 5, 5);
        vertical3.transform.Rotate(90, 0, 0);
        vertical3.transform.localScale = new Vector3(1, 5, 5);
        vertical3.transform.parent = mazeGenerator.transform;
        vertical3.name = "Vertical 3";

        var vertical4 = new GameObject().AddComponent<CubeGenerator>();
        vertical4.transform.position = new Vector3(21, 5, -4);
        vertical4.transform.Rotate(90, 0, 0);
        vertical4.transform.localScale = new Vector3(1, 6, 5);
        vertical4.transform.parent = mazeGenerator.transform;
        vertical4.name = "Vertical 4";
    }

}
