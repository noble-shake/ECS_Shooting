using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{ 
    public static StageManager Instance;
    public List<TextAsset> StageAssets;

    private void Awake()
    {
        if (Instance == this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}