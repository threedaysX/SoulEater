using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AI : Character 
{
    [Header("偵測動作")]
    [SerializeField] protected Detect[] detects;
    [Header("行為模式")]
    [SerializeField] protected Action[] actions;
    // 上一個行動
    private Action lastAction;
    private bool lastActionSuccess;

    [Header("偵測距離")]
    public float detectDistance;
    [Header("行為共通延遲時間(秒)")]
    public float actionDelay = 2f;  
    private float nextActTimes = 0f;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public LayerMask playerLayer;

    void Update()
    {
        playerLayer = LayerMask.GetMask("Player");

        DoDetects();
        // Debug.Log(this);
    }

    public void DoDetects()
    {
        foreach (var detect in detects)
        {
            detect.GetCurrentAI(this);
            if (detect.StartDetectHaviour())
            {
                if (Time.time >= nextActTimes)
                {
                    DoActions();
                    nextActTimes = Time.time + actionDelay + GetAnimationLength(animator);
                }
            }
        }
    }

    public void DoActions()
    {
        Dictionary<Action, int> actionToDoList = new Dictionary<Action, int>();

        // 判斷哪些動作符合條件，代表可以做
        // 若上一個動作執行失敗，則該動作的權重降低2一次
        foreach (var action in actions)
        {
            action.GetCurrentAI(this);
            if (action.CheckActionThatCanDo())
            {
                int weight = action.actionWeight;
                if (action == lastAction && !lastActionSuccess)
                {
                    weight -= 2;
                }
                actionToDoList.Add(action, weight);
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

    protected void DoHightestWeightAction(Dictionary<Action, int> actionToDoList)
    {
        Action currentAction;
        if (actionToDoList.Count == 1)
        {
            currentAction = actionToDoList.Keys.First();
            lastActionSuccess = currentAction.StartActHaviour();
            lastAction = currentAction;
            return;
        }

        // 找到權重最高和-N點權重的那些動作
        int diff = 1;
        int maxWeight = actionToDoList.Values.Max();
        Action[] keyActionsOfMaxWeight = actionToDoList.Where(x => x.Value <= maxWeight && x.Value >= maxWeight - diff).Select(x => x.Key).ToArray();
        // 執行任一動作，但除了移動以外，不會做相同的動作
        currentAction = keyActionsOfMaxWeight[Random.Range(0, keyActionsOfMaxWeight.Length)];
        if (currentAction == lastAction && currentAction.actionType != ActionType.Move)
        {
            currentAction.actionWeight -= 1;    // 做重複的動作，導致權重下降1，降低這個對於動作的慾望
            return;
        }
        lastActionSuccess = currentAction.StartActHaviour();
        lastAction = currentAction;
    }

    protected float GetAnimationLength(Animator anim)
    {
        if (anim == null)
            return 0;
        return AnimationController.Instance.GetCurrentAnimationLength(anim);
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