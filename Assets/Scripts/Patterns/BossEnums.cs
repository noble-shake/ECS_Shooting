public enum BossStageType : byte
{
    SideBySide = 0, // 나란히
    AtSameTime = 1, // 동시에
    None = byte.MaxValue
}

public enum BossMidStageType: byte
{
    Single, // 조준하지 않고
    Aim, // 조준하고

}