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
    SideBySide   =0,                    // ������
    AtSameTime =1,                    // ���ÿ�
    Test = 2,
    None            =byte.MaxValue // �ܵ� ����
}

public enum PatternEnemyAimType : byte
{ 
    Single = 0, // �������� �ʰ�
    Aim    = 1, // �����ϰ�
    Test = 2,
    None = byte.MaxValue // ����ũ ��
}

public enum PatternEnmyShootType : byte
{
    SideBySide = 0,           //������ �߻�
    Radial = 1,                  // �����
    Combine =2,               // ���ļ�
    NoShot = 3,                //�߻縦 ���� ����
    Test = 4,
    None = byte.MaxValue // ����ũ ��
}

//TODO: �ӽ÷� ���� �̸���.
public enum StageNames : byte
{ 
    WeakBossCave =0,
    FinalBossWorld = 1,
    Test = 2,
    None = byte.MaxValue,
}