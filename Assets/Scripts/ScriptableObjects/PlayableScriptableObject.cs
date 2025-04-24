using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Playable 01", menuName = "GameSO/PlayerAsset", order = -1)]
public class PlayableScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite SmallPortrait;
    public Sprite Portrait;


}