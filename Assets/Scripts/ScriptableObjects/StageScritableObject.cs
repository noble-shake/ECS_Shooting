using System.Collections.Generic;
using UnityEngine;

public struct StageContainer
{
    public int NumbOfSpawn;
    public float SpawnTime;
    public EnemyType enemyType;
    public PatternEnemyAimType aimType;
    public PatternEnemyOrderType orderType;
    public PatternEnmyShootType enemyShootType;
}


[CreateAssetMenu(fileName = "Stage 01", menuName = "GameSO/StageAsset", order =-1 )]
public class StageScriptableObject : ScriptableObject
{
    public TextAsset StageCSV;

    public StageContainer ConvertAsset()
    {
        var container = new StageContainer();

        //JsonConvert.DeserializeObject

        return new StageContainer();
    }
}