using UnityEngine;

public class Heal : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(Healing);
    }

    ///<summary>
    ///恢復前方2m內一名友軍 [BaseINT * (INT - BaseINT + 1) * 10] 的生命， 若有多個，則指定血量較少的目標。						
    ///</summary>
    private void Healing()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D targetCol)
    {

    }
}

