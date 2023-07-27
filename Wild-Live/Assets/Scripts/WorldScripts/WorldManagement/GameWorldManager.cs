using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldManager : MonoBehaviour
{
    //TODO: Changing back the script execution order. Setting this script back to 0!
    
    [field: SerializeField]
    public static GameWorldManager Instance { get; set; } = null;
    
    [field: SerializeField]
    public TerrainGenerator TerrainGenerator { get; set; } = null;

    [field: SerializeField]
    public WeatherSystem WeatherSystem { get; set; } = null;

    [field: SerializeField]
    public WorldTime WorldTime { get; set; } = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(Instance);
        }
    }
}
