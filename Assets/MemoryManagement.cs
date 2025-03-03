// all code samples from the tutorial are included 
// use the appropriate #define statement to select the code you want to run


 #define ORIGINAL
// #define STRINGBUILDER
// #define OPTIMIZE_ARRAY
// #define ARRAY_PASS_BY_REFERENCE


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if ORIGINAL
public class MemoryManagement : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        ConcatExample();
    }

    int[] GetArray()
    {
        int[] intArray = new int[50];
        //init array with random numbers
        for (int i = 0; i < intArray.Length; i++)
        {
            intArray[i] = Random.Range(0, 100);
        }
        return intArray;
    }

    string ConcatExample()
    {
        int[] intArray = GetArray();

        string line = intArray[0].ToString();

        for (int i = 0; i < intArray.Length; i++)
        {
            line += ", " + intArray[i].ToString();
        }

        return line;
    }
}
#endif

#if STRINGBUILDER
public class MemoryManagement : MonoBehaviour
{
    void Update()
    {
        ConcatExample();
    }

    int[] GetArray()
    {
        int[] intArray = new int[50];
        //init array with random numbers
        for (int i = 0; i < intArray.Length; i++)
        {
            intArray[i] = Random.Range(0, 100);
        }
        return intArray;
    }

    string ConcatExample()
    {
        // no need to call GetArray() here, access intArray directly
        int[] intArray = GetArray();

        StringBuilder line = new StringBuilder(intArray[0].ToString());

        for (int i = 1; i < intArray.Length; i++)
        {
            line.Append(", ").Append(intArray[i]);
        }

        return line.ToString();
    }
}
#endif

#if OPTIMIZE_ARRAY
public class MemoryManagement : MonoBehaviour
{ 
    private int[] intArray;

    void Start()
    {
        // Initialize the array once when the game starts
        intArray = GetArray();
    }

    void Update()
    {
        // Use the cached array in Update
        ConcatExample();
    }

    int[] GetArray()
    {
        int[] tempArray = new int[50];
        // Init array with random numbers
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = Random.Range(0, 100);
        }
        return tempArray;
    }

    string ConcatExample()
    {
        StringBuilder line = new StringBuilder(intArray[0].ToString());

        for (int i = 1; i < intArray.Length; i++)
        {
            line.Append(", ").Append(intArray[i]);
        }

        return line.ToString();
    }
}
#endif

#if ARRAY_PASS_BY_REFERENCE
public class MemoryManagement : MonoBehaviour
{ 
    private int[] intArray;

    void Start()
    {
        // Initialize the array once when the game starts
        intArray = new int[50];
        GetArray(intArray);
    }

    void Update()
    {
        // Use the cached array in Update
        ConcatExample();
    }

    void GetArray(int[] arrayToFill)
    {        
        // Init array with random numbers
        for (int i = 0; i < arrayToFill.Length; i++)
        {
            arrayToFill[i] = Random.Range(0, 100);
        }
    }

    string ConcatExample()
    {
        StringBuilder line = new StringBuilder(intArray[0].ToString());

        for (int i = 1; i < intArray.Length; i++)
        {
            line.Append(", ").Append(intArray[i]);
        }

        return line.ToString();
    }
}
#endif