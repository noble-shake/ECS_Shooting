using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [Header("Border")]
    public float SCREEN_WIDTH_MIN;
    public float SCREEN_WIDTH_MAX;
    public float SCREEN_HEIGHT_MIN;
    public float SCREEN_HEIGHT_MAX;

    [Header("Test")]
    public Button SpawnButton;
    public bool isSpawn;

    [Space]
    [Header("Stat")]
    public int Life;
    public int Bomb;
    public int Power;
    public float Speed;

    public void OnTestSpawnPressed()
    {
        isSpawn = true;
        SpawnButton.enabled = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SpawnButton.onClick.AddListener(OnTestSpawnPressed);
    }


}
