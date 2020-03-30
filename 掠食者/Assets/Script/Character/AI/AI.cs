using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AI : Character 
{
    [Header("偵測動作")]
    [SerializeField] protected Detect[] detects;
    [Header("行為模式")]
    [SerializeField] protected Action[] actions;

    [Header("偵測距離")]
    public float detectDistance;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public LayerMask playerLayer;

    private void Start()
    {

    }

    void Update()
    {
        playerLayer = LayerMask.GetMask("Player");

        DoDetects();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill(skills[0]);
        }
        // Debug.Log(this);
    }

    public void DoDetects()
    {
        foreach (var detect in detects)
        {
            detect.GetCurrentAI(this);
            if (detect.StartDetectHaviour())
            {
                DoActions();
            }
        }
    }

    public void DoActions()
    {
        Dictionary<Action, int> actionToDoList = new Dictionary<Action, int>();

        // 判斷哪些動作符合條件，代表可以做
        foreach (var action in actions)
        {
            action.GetCurrentAI(this);
            if (action.CheckActionThatCanDo())
            {
                actionToDoList.Add(action, action.actionWeight);
            }
        }

        // 如果沒動作可以做，就隨機抓一個
        if (actionToDoList.Count == 0)
        {
            int random = Random.Range(0, actions.Length);
            Action randomAct = actions[random];
            actionToDoList.Add(randomAct, randomAct.actionWeight);
        }

        // 決定最後的動作
        DoHightestWeightAction(actionToDoList);
    }

    public void DoHightestWeightAction(Dictionary<Action, int> actionToDoList)
    {
        if (actionToDoList.Count == 1)
        {
            actionToDoList.Keys.First().StartActHaviour();
            return;
        }

        // 找到權重最高的那些動作
        int maxWeight = actionToDoList.Values.Max();
        Action[] keyActionsOfMaxWeight = actionToDoList.Where(x => x.Value == maxWeight).Select(x => x.Key).ToArray();
        // 執行任一動作
        keyActionsOfMaxWeight[Random.Range(0, keyActionsOfMaxWeight.Length)].StartActHaviour();
    }

    /// <summary>
    /// 根據狀態來調整行為權重
    /// </summary>
    public abstract void ChangeActionWeight();

    /// <summary>
    /// 偵測到敵人，重新調整偵測範圍(擴大、不變或縮小)
    /// </summary>
    public abstract void ChangeInDetectRange();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }
}