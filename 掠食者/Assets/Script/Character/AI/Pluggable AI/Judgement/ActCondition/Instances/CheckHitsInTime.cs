using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CheckHitsInTime")]
public class CheckHitsInTime : JudgeCondition
{
    [Header("在幾秒內有受傷")]
    public float customTime;

    private float lastHealth;
    private bool hasHit = false;
    public override bool CheckActConditionHaviour()
    {
        if (ai != null)
        {
            if(Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))        //if input attack
            {
                Caller.Instance.StartCoroutine(HasHit());
            }
        }

        return hasHit;
    }

    private IEnumerator HasHit()
    {
        lastHealth = ai.CurrentHealth;
        yield return new WaitForSeconds(customTime);
        if (lastHealth <= ai.CurrentHealth)
        {
            hasHit = true;
        }
        else
        {
            hasHit = false;
        }
    }
}