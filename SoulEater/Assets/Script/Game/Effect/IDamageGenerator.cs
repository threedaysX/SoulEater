using UnityEngine;

public interface IDamageGenerator
{
    void SetupDamage(bool isCritical, int damageAmount, Color color);
}
