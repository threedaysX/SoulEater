using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform pointToTeleport = null;
    [Header("把boss打開(SetActive)")]
    [SerializeField] private GameObject boss = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = pointToTeleport.transform.position;
            if(!boss.activeInHierarchy)
                boss.SetActive(true);
        }
    }
}
