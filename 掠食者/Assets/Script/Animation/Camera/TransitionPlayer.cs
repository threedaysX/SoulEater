using System.Collections;
using UnityEngine;

public class TransitionPlayer : MonoBehaviour
{
    [Header("播放轉場動畫前等幾秒")]
    public float transitionWaitTime;

    [SerializeField] private Animator transitionController;
    private void Start()
    {
        transitionController = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();
        transitionController.SetBool ("True", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PlayTransition(transitionWaitTime));
        }
    }

    public IEnumerator PlayTransition(float duration)
    {
        yield return new WaitForSeconds(0.1f);
        transitionController.SetTrigger("TransitionPlaying");
        yield return new WaitForSeconds(duration);
        transitionController.ResetTrigger("TransitionPlaying");
    }
}
