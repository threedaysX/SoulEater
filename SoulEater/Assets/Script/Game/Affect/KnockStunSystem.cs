using System.Collections;
using UnityEngine;

public class KnockStunSystem : MonoBehaviour
{
    public void KnockStun(Character character, float damageDirectionX, float knockBackForce)
    {
        // 播放擊退硬直動畫
        StartCoroutine(KnockStunCoroutine(character, damageDirectionX, knockBackForce));
        character.isKnockStun = true;
        RecoverFromKnockStun(character);
    }
    public void RecoverFromKnockStun(Character character)
    {
        StartCoroutine(RecoverFromKnockStunCoroutine(character));
    }

    private IEnumerator KnockStunCoroutine(Character character, float damageDirectionX, float knockBackForce) 
    {
        // 從哪裡來的擊退力量，並往反方向退移
        Vector3 dir = new Vector3(-damageDirectionX, 0, 0).normalized;
        float basicKnockBackDuration = 0.2f;
        float timeLeft = basicKnockBackDuration;
        while (timeLeft > 0)
        {
            if (character == null)
                yield break;

            if (timeLeft > Time.deltaTime)
                character.transform.position += (dir * knockBackForce * Time.deltaTime / basicKnockBackDuration);
            else
                character.transform.position += (dir * knockBackForce * timeLeft / basicKnockBackDuration);

            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RecoverFromKnockStunCoroutine(Character character)
    {
        yield return new WaitForSeconds(character.data.recoverFromKnockStunTime.Value);
        if (character == null)
            yield break;

        character.isKnockStun = false;
    }
}
