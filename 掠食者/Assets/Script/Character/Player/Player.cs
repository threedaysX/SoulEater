using UnityEngine;

public class Player : Character
{
    private PlayerSkillSlotKeyControl skillKey;

    private void Start()
    {
        this.tag = "Player";
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        skillKey = GetComponent<PlayerSkillSlotKeyControl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))
        {
            StartAttack(AttackType.Attack, data.attackElement);
        }

        if (Input.GetKeyDown(HotKeyController.skillKey1))
        {
            UseSkill(skillKey.SkillSlot1.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey2))
        {
            UseSkill(skillKey.SkillSlot2.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey3))
        {
            UseSkill(skillKey.SkillSlot3.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey4))
        {
            UseSkill(skillKey.SkillSlot4.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey5))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey6))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey7))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey8))
        {

        }

        ResetBarUI();
    }

    public override void Die()
    {
        ResetBarUI();
        base.Die();
    }

    private void ResetBarUI()
    {
        if (isHealthDirty)
        {
            PlayerUIControl.Instance.SetHealthUI(data.maxHealth.Value, CurrentHealth);
            isHealthDirty = false;
        }

        if (isManaDirty)
        {
            PlayerUIControl.Instance.SetManaUI(data.maxMana.Value, CurrentMana);
            isManaDirty = false;
        }
    }
}
