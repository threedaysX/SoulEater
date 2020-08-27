using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D door;
    public Vector2 pointToTeleport;
    [Header("把boss打開(SetActive)")]
    public GameObject boss;
    private void Start()
    {
        door = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = pointToTeleport;
            if(!boss.activeInHierarchy)
                boss.SetActive(true);
        }
    }
}
