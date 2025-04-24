public enum PlayerType : byte
{ 
    Dosa = 0,
    Crane = 1,
}


public enum EnemyType : byte
{ 
    EnemyA =0,
    EnemyB =1,
    EnemyC =2,
}

public enum PatternEnemyOrderType : byte
{ 
    SideBySide   =0,                    // 나란히
    AtSameTime =1,                    // 동시에
    Test = 2,
    None            =byte.MaxValue // 단독 출현
}

public enum PatternEnemyAimType : byte
{ 
    Single = 0, // 조준하지 않고
    Aim    = 1, // 조준하고
    Test = 2,
    None = byte.MaxValue // 유니크 샷
}

public enum PatternEnmyShootType : byte
{
    SideBySide = 0,           //나란히 발사
    Radial = 1,                  // 방사형
    Combine =2,               // 합쳐서
    NoShot = 3,                //발사를 하지 않음
    Test = 4,
    None = byte.MaxValue // 유니크 샷
}

//TODO: 임시로 지은 이름임.
public enum StageNames : byte
{ 
    WeakBossCave =0,
    FinalBossWorld = 1,
    Test = 2,
    None = byte.MaxValue,
}