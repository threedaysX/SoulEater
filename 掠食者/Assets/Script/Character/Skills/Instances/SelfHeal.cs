using UnityEngine;

public class SelfHeal : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(Heal);
    }

    ///<summary>
    ///恢復自身[最大生命10%+DEX*5+INT*10]生命
    ///</summary>
    private void Heal()
    {
        StartCoroutine(FollowCaster(1f));
        sourceCaster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        sourceCaster.CurrentHealth += (int)(sourceCaster.data.maxHealth.Value * 0.1f + sourceCaster.data.status.dexterity.Value * 5 + sourceCaster.data.status.intelligence.Value * 10);
    }
}