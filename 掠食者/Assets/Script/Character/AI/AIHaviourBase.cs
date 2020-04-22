using UnityEngine;

public class AIHaviourBase : ScriptableObject
{
    protected AI ai;
    public void GetCurrentAIHavior(AI currentAi)
    {
        ai = currentAi;
    }
}
