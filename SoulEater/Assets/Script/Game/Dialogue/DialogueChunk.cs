using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueChunk")]
public class DialogueChunk : ScriptableObject
{
    public string chunkName;
    [Range(0, 3)]
    public int bifurcationAmount;
    public DialogueChunk[] bifurcationChunk;
    [TextArea]
    public string[] stenences;
}
