using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider2D[] GetAttackHits()
    {
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCollider(this.gameObject.GetComponent<Collider2D>(), new ContactFilter2D(), results);
        return results.ToArray();
    }
}