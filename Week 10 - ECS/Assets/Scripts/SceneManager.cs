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
        cubePositions = new Vector3[gridDimension * gridDimension]; // enough elements for all the cubes
        uint cubeIndex = 0; // counter for the cube index

        if (usingDataOrientedDesign)
        {
            for (int i = (-gridDimension / 2); i < (gridDimension / 2); i++)
            {
                for (int j = 0; j < gridDimension; j++)
                {
                    cubePositions[cubeIndex++] = new Vector3(i * gridSpacing, 0, j * gridSpacing);
                }
            }

            mesh = cubePrefabOOP.GetComponent<MeshFilter>().sharedMesh;
            rp = new RenderParams(cubeMaterial);
        }
        else
        {
            for (int i = -(gridDimension / 2); i < (gridDimension / 2); i++)
            {
                for(int j = 0; j < gridDimension; j++)
                {
                    Instantiate(cubePrefabOOP, new Vector3(i * gridSpacing, 0, j * gridSpacing), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (usingDataOrientedDesign)
        {
            foreach (var position in cubePositions)
            {
                Vector3 thisPosition = position;
                float y = Mathf.Sin(position.x * resolution + Time.fixedTime) +
                          Mathf.Sin(position.z * resolution + Time.fixedTime);
                thisPosition.y = y;
                Graphics.RenderMesh(rp, mesh, 0, Matrix4x4.Translate(thisPosition));
            }
        }
    }
}
