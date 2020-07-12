using UnityEngine;
using UnityEngine.UI;

public class EnemyUIControl : Singleton<EnemyUIControl>
{
    public GameObject UIObject;

    [Header("Hp")]
    public Image health;

    [Header("Name")]
    public Text enemyName;
    public Text enemyNameShadow;

    public void SetHealthUI(string enemyName, float maxHealth, float currentHealth)
    {
        if (this.enemyName == null || enemyNameShadow == null)
            return;

        this.enemyName.text = enemyName;
        this.enemyNameShadow.text = enemyName;
        float remainPercent = UIImageControll.Instance.SetImageFillAmount(health, maxHealth, currentHealth);

        if (remainPercent <= 0)
        {
            UIObject.SetActive(false);
        }
        else
        {
            UIObject.SetActive(true);
        }
    }
}
