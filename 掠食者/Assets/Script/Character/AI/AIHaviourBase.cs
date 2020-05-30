using UnityEngine;

public class AiHaviourBase : ScriptableObject
{
    protected AI ai;
    public void GetCurrentAIHavior(AI currentAi)
    {
        ai = currentAi;
    }
}
