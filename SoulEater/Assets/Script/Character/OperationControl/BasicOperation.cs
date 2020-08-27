using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicOperation
{
    public bool canDo;
    public BasicOperationType operationType;
    public Dictionary<LockType, LockData> locks;

    public void Lock(LockType lockType, float nextUnLockTime = 0)
    {
        if (locks == null)
            locks = new Dictionary<LockType, LockData>();

        if (locks.ContainsKey(lockType))
        {
            locks[lockType].nextUnLockTime = Time.time + nextUnLockTime;
        }
        else
        {
            locks.Add(lockType, new LockData(1, Time.time + nextUnLockTime));
        }
        if (canDo)
            canDo = false;
    }

    public void UnLock(LockType lockType)
    {
        if (locks == null)
            locks = new Dictionary<LockType, LockData>();

        if (locks.ContainsKey(lockType))
        {
            locks[lockType].lockNum--;
            if (locks[lockType].lockNum <= 0)
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
    TypeChange,  // 型態改變(變身or變化...)
    Die
}

public class LockData
{
    public int lockNum;
    public float nextUnLockTime;

    public LockData(int lockNum, float nextUnlockTime)
    {
        this.lockNum = lockNum;
        this.nextUnLockTime = nextUnlockTime;
    }
}