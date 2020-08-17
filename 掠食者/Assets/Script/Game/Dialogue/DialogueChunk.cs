using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueChunk")]
public class DialogueChunk : ScriptableObject
{
    public string chunkName;
    [TextArea]
    public string[] stenences;
    [Range(0, 3)]
    public int choices;
}
