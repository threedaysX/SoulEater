using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CheckCustomDamageInTime")]
public class CheckCustomDamageInTime : JudgeCondition
{
    [Header("受最大生命幾%傷害"), Range(0, 100)]
    [SerializeField] private float custumDamage;
    [Header("在幾秒內有受傷")]
    [SerializeField] private float customTime = 0;

    private float lastHealth = 0;
    private bool hasHit = false;
    public override bool CheckActConditionHaviour()
    {
        if (ai != null)
        {
            if(Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))
            {
                MonobehaviorCallerForSO.Instance.StartCoroutine(HasDamaged());
            }
        }

        return hasHit;
    }

    private IEnumerator HasDamaged()
    {
        lastHealth = ai.CurrentHealth;
        yield return new WaitForSeconds(customTime);
        if((lastHealth - ai.CurrentHealth) / ai.data.maxHealth.Value * 100 >= custumDamage)
        {
            hasHit = true;
        }
        else
        {
            hasHit = false;
        }
    }
}
