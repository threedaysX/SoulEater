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
        sourceCaster.CurrentHealth += sourceCaster.data.maxHealth.Value * 0.1f + sourceCaster.data.status.dexterity.Value * 5 + sourceCaster.data.status.intelligence.Value * 10;
    }
}