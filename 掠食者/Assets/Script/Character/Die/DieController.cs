using System.Collections;
using UnityEngine;

public class DieController : MonoBehaviour
{
    private Character character;
    public Transform soulPrefab;
    public ParticleSystem dieParticle;
    public AudioClip dieSound;
    public string soulName;
    public float dieDuration;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public void StartDie()
    {
        character.GetIntoImmune(true);
        character.LockOperation(LockType.Die, true);
        character.StopAllCoroutines();
        character.operationController.InterruptAnimOperation();
        character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        character.CurrentHealth = 0;
        character.operationSoundController.PlaySound(dieSound);
        dieParticle.Play(true);
        GenerateSoul();
        StartCoroutine(DelayDie());
    }

    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(dieDuration);
        Destroy(this.gameObject);
    }

    public void GenerateSoul()
    {
        Transform soul = Instantiate(soulPrefab, transform.position, transform.rotation);
        soul.GetComponent<Character>().GetIntoImmune(dieDuration + 0.1f);
        soul.GetComponent<Character>().characterName = soulName;
        soul.tag = character.tag;
        soul.gameObject.layer = character.gameObject.layer;
        soul.name = soulName;
        StartCoroutine(FollowMaster(soul));
    }

    public virtual IEnumerator FollowMaster(Transform soul)
    {
        float timeleft = dieDuration;
        while (timeleft > 0)
        {
            if (transform == null)
                yield break;

            soul.position = transform.position;
            timeleft -= Time.deltaTime;
            yield return null;
        }
    }
}