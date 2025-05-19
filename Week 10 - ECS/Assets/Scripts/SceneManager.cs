using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public bool usingDataOrientedDesign;
    public GameObject cubePrefabOOP;
    public Material cubeMaterial;
    public int gridDimension = 100;
    public float gridSpacing = 1.0f;

    public float yMultiplier = 1.0f;
    public float resolution = 0.2f;
    Mesh mesh;
    RenderParams rp;
    Vector3[] cubePositions;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = -(gridDimension / 2); i < (gridDimension / 2); i++)
        {
            for(int j = 0; j < gridDimension; j++)
            {
                Instantiate(cubePrefabOOP, new Vector3(i * gridSpacing, 0, j * gridSpacing), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
