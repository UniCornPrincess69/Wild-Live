using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoader : MonoBehaviour
{
    public static Dictionary<string, ScriptableValue> List { get; set; }

    public static void Init()
    {
        List = new Dictionary<string, ScriptableValue>()
        {
            {"Map Size", new SOInt(){Value = 100} }

        };
    }

    private void Awake()
    {
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
