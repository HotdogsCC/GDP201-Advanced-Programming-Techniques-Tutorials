using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestingFlags : MonoBehaviour
{
    //Used to see if functions fire
    //General usecase: set false, try something that changes it to true, after checking for true turn it to false
    public static bool passFlag = false;

    public void Passed()
    {
        passFlag = true;
    }

}
