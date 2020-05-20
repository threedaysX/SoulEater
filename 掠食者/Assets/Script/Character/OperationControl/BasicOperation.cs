[System.Serializable]
public class BasicOperation
{
    public bool canDo;
    public BasicOperationType operationType;
    [UnityEngine.SerializeField] private int lockNumber;

    public void Lock()
    {
        lockNumber++;
        if (canDo)
            canDo = false;
    }

    public void UnLock()
    {
        if (lockNumber > 0)
            lockNumber--;
        if (lockNumber == 0 && !canDo)
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