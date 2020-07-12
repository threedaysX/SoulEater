using UnityEngine;

public class PlayerSkillSlotKeyControl : MonoBehaviour
{
    public SkillSlot SkillSlot1; 
    public SkillSlot SkillSlot2; 
    public SkillSlot SkillSlot3; 
    public SkillSlot SkillSlot4; 
    public SkillSlot SkillSlot5; 
    public SkillSlot SkillSlot6; 
    public SkillSlot SkillSlot7; 
    public SkillSlot SkillSlot8;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey1)))
        {
            player.UseSkill(SkillSlot1.skill);
        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey2)))
        {
            player.UseSkill(SkillSlot2.skill);
        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey3)))
        {
            player.UseSkill(SkillSlot3.skill);
        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey4)))
        {
            player.UseSkill(SkillSlot4.skill);
        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey5)))
        {

        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey6)))
        {

        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey7)))
        {

        }
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.SkillKey8)))
        {

        }
    }
}
