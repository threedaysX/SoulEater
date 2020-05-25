using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEvent : MonoBehaviour
{
    public float fadeInTime;
    public float fadeOutTime;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            StartCoroutine(FadeScreen.Instance.Fade(fadeInTime, fadeOutTime));
        }
    }
}
