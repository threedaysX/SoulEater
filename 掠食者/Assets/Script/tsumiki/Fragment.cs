using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在寫touchStars(相對位置)時
//(0,0)一定要是正三角形!!!
[System.Serializable]
public class F_Data
{
    public List<Vector2> touchStars_v2;
    string name;
    int edgeCount;
    int triggerAffixIndex;
    //class[] Affix;
    enum FragmentType { }

    public int fragmentID;
    public int triggerCount;
    [SerializeField]
    public List<int> touchStarsID=new List<int>();                  //用來記錄此碎片和哪個star交疊
    [SerializeField]
    public List<Neighbor> neighborStars;             //此碎片邊上的鄰居star
    [SerializeField]
    public List<neighborList> neighborRelative;  //此碎片邊上的鄰居關係

}

[CreateAssetMenu(fileName = "Fragment",menuName ="Tsumiki/Create Fagment Asset",order =1)]
public class Fragment : ScriptableObject
{
    public F_Data m_Data;
}
