using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaraBurst : DisposableSkill
{
    public Skill naraCircleBurstData;
    public float igniteDuration; // 擊中敵人【燃燒】異常的時間
    public float burstRadius;   // 爆炸的半徑
    public int burstLimitCount; // 爆炸的數量
    public AudioClip renderSound;
    public GameObject burstHint;
    private List<NaraCircleBurstData> bursts; // 記錄每個爆炸的資訊

    protected override void AddAffectEvent()
    {
        
    }

    public override void GenerateSkill(Character character, Skill skill)
    {
        base.GenerateSkill(character, skill);

        bursts = new List<NaraCircleBurstData>();
        burstHint.SetActive(false);
        burstHint.transform.localScale = new Vector3(burstHint.transform.localScale.x * burstRadius, burstHint.transform.localScale.y * burstRadius, 0);  // 調整爆炸半徑
        foreach (Transform item in burstHint.transform)
        {
            item.localScale = new Vector3(item.localScale.x * burstRadius, item.localScale.y * burstRadius, 0);  // 調整爆炸半徑
        }
        ObjectPools.Instance.RenderObjectPoolsInParent(burstHint, burstLimitCount);
    }

    public override void CastSkill()
    {
        base.CastSkill();

        float rangeDelay = currentSkill.fixedCastTime.Value / burstLimitCount;
        float upperRangeDelay = rangeDelay * 0.6f;
        float lowerRangeDelay = rangeDelay * 0.4f;
        float currentDelay = 0;
        for (int i = 0; i < burstLimitCount; i++)
        {
            currentDelay += Random.Range(lowerRangeDelay, upperRangeDelay);
            StartCoroutine(RenderBurstHint(GetRandomPos(), currentDelay));
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        float currentCount = 1;
        foreach (var burst in bursts)
        {
            currentCount++;
            StartCoroutine(StartBurst(burst.transform, burst.delay, currentCount, burstLimitCount));
        }
    }

    private IEnumerator RenderBurstHint(Vector3 position, float burstDelayBetweenFirst)
    {
        yield return new WaitForSeconds(burstDelayBetweenFirst);

        NaraCircleBurst burst = ObjectPools.Instance.GetObjectInPools(burstHint.name, position).GetComponent<NaraCircleBurst>();
        burst.data.transform = burst.transform;
        burst.data.delay = burstDelayBetweenFirst;
        burst.data.igniteDuration = igniteDuration;
        burst.GenerateSkill(sourceCaster, naraCircleBurstData);
        bursts.Add(burst.data);
    }

    private IEnumerator StartBurst(Transform burst, float delay, float currentCount, float finalCount)
    {
        yield return new WaitForSeconds(delay);
        burst.GetComponent<NaraCircleBurst>().UseSkill();
        if (currentCount == finalCount)
        {
            StartCoroutine(SetActiveAfterSkillDone(0.3f));
        }
    }

    private Vector3 GetRandomPos()
    {
        // 將在這塊方形的範圍內生成爆炸(點)
        float x = Random.Range(this.transform.position.x - this.transform.right.x * currentSkill.range.Value * 0.4f, this.transform.position.x + this.transform.right.x * currentSkill.range.Value * 0.6f);
        // 技能範圍窄化
        float y = Random.Range(this.transform.position.y, this.transform.position.y + currentSkill.range.Value * 0.15f);

        return new Vector3(x, y, 0);
    }
}
