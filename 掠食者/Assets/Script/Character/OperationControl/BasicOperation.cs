using System.Collections.Generic;

[System.Serializable]
public class BasicOperation
{
    public bool canDo;
    public BasicOperationType operationType;
    public Dictionary<LockType, int> locks;

    public void Lock(LockType lockType)
    {
        if (locks == null)
            locks = new Dictionary<LockType, int>();

        if (locks.ContainsKey(lockType))
        {
            locks[lockType]++;
        }
        else
        {
            locks.Add(lockType, 1);
        }
        if (canDo)
            canDo = false;
    }

    public void UnLock(LockType lockType)
    {
        if (locks == null)
            locks = new Dictionary<LockType, int>();

        if (locks.ContainsKey(lockType))
        {
            locks[lockType]--;
            if (locks[lockType] <= 0)
            {
                locks.Remove(lockType);
            }
        }
        if (locks.Count == 0)
            canDo = true;
    }
}

public enum BasicOperationType
{ 
    Move,
    Jump,
    Evade,
    Attack,
    UseSkill,
    LockDirection
}

public enum LockType 
{
    OperationAction,
    SkillAction,
    Stun,
    Freeze,
    Silence,
    Afraid,
    Lame, // 無法移動
}