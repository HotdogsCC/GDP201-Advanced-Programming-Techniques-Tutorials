using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager manager = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (manager == null)
            manager = this;
        else
            Destroy(gameObject);
    }

    public TextMeshProUGUI countdown;
    public TextMeshProUGUI levels;

    // *** Step 13

    public static void SetCountdown(float t)
    {
        // *** Step 13
    }
}
