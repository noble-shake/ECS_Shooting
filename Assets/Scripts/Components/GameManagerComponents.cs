using UnityEngine;
using Unity.Entities;

public struct GameManagerTag : IComponentData { }
public struct PauseComponent : IComponentData, IEnableableComponent { }