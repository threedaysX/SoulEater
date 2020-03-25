using UnityEngine;

public class AIHaviourBase : ScriptableObject
{
    protected AI ai;
    public void GetCurrentAI(AI currentAi)
    {
        ai = currentAi;
    }
}
