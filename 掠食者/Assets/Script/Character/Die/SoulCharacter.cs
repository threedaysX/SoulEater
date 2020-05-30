using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCharacter : Character
{
    public override void Die()
    {
        // Give Frag.
        SlowMotionController.Instance.DoSlowMotion(0.05f, 1f);
        Destroy(this.gameObject);
        EndGameManager.Instance.EndGame();
    }
}