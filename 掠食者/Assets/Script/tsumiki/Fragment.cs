using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在寫touchStars(相對位置)時
//(0,0)一定要是正三角形!!!
[System.Serializable]
public class F_Data
{
    public List<Vector2> touchStars;
    string name;
    int edgeCount;
    int triggerAffixIndex;
    //class[] Affix;
    enum FragmentType { }
}

[CreateAssetMenu(fileName = "Fragment",menuName ="Tsumiki/Create Fagment Asset",order =1)]
public class Fragment : ScriptableObject
{
    public F_Data m_Data;
}
