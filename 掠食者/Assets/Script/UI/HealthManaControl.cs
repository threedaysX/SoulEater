public class HealthManaControl : Singleton<HealthManaControl>
{
    public float SetHealthBar(UnityEngine.UI.Image healthBar, float maxHealth, float currentHealth)
    {
        return (healthBar.fillAmount = currentHealth / maxHealth);
    }
}
