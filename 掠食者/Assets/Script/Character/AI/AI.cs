using System.Collections.Generic;
using UnityEngine;

public abstract class AI : Character 
{
    public bool CanDetect { get; set; }
    public bool CanAction { get; set; }

    [Header("偵測動作")]
    [SerializeField] protected Detect[] detects;
    [Header("行為模式")]
    [SerializeField] protected AiAction[] actions;
    [Header("預設行為模式")]
    [SerializeField] public AiAction defaultAction;
    [Header("上一個行為模式")]
    [SerializeField] public AiAction lastAction;
    private bool lastActionSuccess;

    [Header("偵測距離")]
    public float detectDistance;
    [Header("行為延遲時間(秒)")]
    public float commonActionDelay = 0f;  
    public float animationActionDelay = 0.1f;
    private float nextActTimes = 0f;

    [Header("最大權重容許區間")]
    public float maxWeightOffset = 2;    // 將符合【最大權重】與【最大權重-Offset】挑出並執行。
    [Header("動作權重恢復")]
    public float actionWeightRegen = 1;       // 每次恢復量
    public int actionWeightRegenCount = 10;  // 每做N個動作，就恢復權重一次
    private int cumulativeActionCount = 0;

    private bool inCombatStateTrigger = false; // 是否進入戰鬥狀態
    private bool outOfCombatTrigger = false;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public LayerMask playerLayer;
    [HideInInspector] public DistanceDetect distanceDetect;

    public virtual void Start()
    {
        this.gameObject.AddComponent(typeof(DistanceDetect));
        distanceDetect = GetComponent<DistanceDetect>();
        CanDetect = true;
        CanAction = true;
        if (chaseTarget == null)
        {
            outOfCombatTrigger = true;
        }
        ReturnDefaultAction(true);
    }

    public virtual void Update()
    {
        playerLayer = LayerMask.GetMask("Player");

        DoDetects();
        Combat();
        CheckOutOfCombatState();
    }

    private void CheckOutOfCombatState()
    {
        if (inCombatStateTrigger && chaseTarget == null)
        {
            inCombatStateTrigger = false;
            outOfCombatTrigger = true;
        }

        // 脫離戰鬥後，重置所有行動權重
        if (!inCombatStateTrigger && outOfCombatTrigger) 
        {
            outOfCombatTrigger = false;
            foreach (var action in actions)
            {
                action.ResetWeight();
            }
        }

        // 每執行N次動作後，恢復N點的行動權重
        if (cumulativeActionCount != 0 && cumulativeActionCount % actionWeightRegenCount == 0)
        {
            foreach (var action in actions)
            {
                action.ResetWeight(actionWeightRegen);
            }
        }
    }

    public void DoDetects()
    {
        if (!CanDetect)
            return;

        // 偵測
        foreach (var detect in detects)
        {
            detect.GetCurrentAIHavior(this);
            if (detect.StartDetectHaviour())
            {
                if (!inCombatStateTrigger)
                    inCombatStateTrigger = true;
            }
            else
            {
                if (chaseTarget != null)
                    chaseTarget = null;
            }
        }
    }

    public void Combat()
    {
        // 進入戰鬥狀態
        if (inCombatStateTrigger && chaseTarget != null)
        {
            if (CanAction && Time.time >= nextActTimes)
            {
                // 每次開始執行動作之前，回到Idle狀態
                ReturnDefaultAction();
                // 在執行Action時，會持續面對目標
                AlwaysFaceTarget();
                DoActions();
                // 只有移動不需要等待延遲，且算是實際的行動數
                if (lastAction.actionType != AiActionType.Move)
                {
                    nextActTimes = Time.time + commonActionDelay + animationActionDelay;
                    cumulativeActionCount++;
                }
            }
        }
        else
        {
            ReturnDefaultAction();
        }
    }

    public void DoActions()
    {
        List<AiAction> actionToDoList = new List<AiAction>();
        
        // 判斷哪些動作符合條件，代表可以做
        // 若上一個相同的動作執行失敗，則權重降低N一次
        foreach (var action in actions)
        {
            action.GetCurrentAIHavior(this);
            if (action.CheckActionThatCanDo())
            {
                if (action == lastAction && !lastActionSuccess)
                {
                    float amount = action.minusWeightAmountWhenNotSuccess;
                    action.ActionWeight -= amount;
                    action.AddDiffCount(amount);
                }
                // 只留下【最大】與【最大-offset】之間權重的動作(相同權重也會留下)。
                if (actionToDoList.Count != 0)
                {
                    if (action.ActionWeight < actionToDoList[0].ActionWeight - maxWeightOffset)
                        continue;
                    if (action.ActionWeight > actionToDoList[0].ActionWeight)
                    {
                        actionToDoList.Clear();
                    }
                }
                actionToDoList.Add(action);
            }
        }

        // 如果沒動作可以做，就隨機抓一個
        if (actionToDoList.Count == 0)
        {
            AiAction randomAct = actions[Random.Range(0, actions.Length)];
            actionToDoList.Add(randomAct);
        }

        // 決定最後的動作
        DoHightestWeightAction(actionToDoList);
    }

    protected void DoHightestWeightAction(List<AiAction> actions)
    {
        AiAction action;
        if (actions.Count == 1)
        {
            action = actions[0];
            DoAction(action);
            return;
        }

        // 執行任一動作
        action = actions[Random.Range(0, actions.Count - 1)];
        DoAction(action);
    }

    /// <summary>
    /// 動作執行
    /// </summary>
    /// <param name="action">要執行的動作</param>
    /// <param name="exceptTypesToMinusWeight">除了做哪些類型的動作不會降低權重</param>
    protected void DoAction(AiAction action)
    {
        float amount = action.minusWeightAmountAfterAction;
        if (amount > 0)
        {
            action.ActionWeight -= amount;    // 動作結束後，權重下降N點，降低這個對於動作的慾望
            action.AddDiffCount(amount);
        }

        float animDelay = 0f;
        if (action.clips != null)
        {
            foreach (var clip in action.clips)
            {
                animDelay += clip.length;
            }
        }
        animationActionDelay = animDelay;
        commonActionDelay = action.commonActionDelay;
        lastActionSuccess = action.StartActHaviour();
        lastAction = action;
    }

    protected void ReturnDefaultAction(bool setToLastAction = false)
    {
        if (lastAction == null || defaultAction == null)
            return;
        if (lastAction.actionType == AiActionType.Idle)
            return;

        defaultAction.GetCurrentAIHavior(this);
        defaultAction.StartActHaviour();
        if (setToLastAction)
            lastAction = defaultAction;
    }

    private void AlwaysFaceTarget()
    {
        if (chaseTarget == null || !freeDirection.canDo)
            return;

        float faceDirX = gameObject.transform.position.x - chaseTarget.transform.position.x;
        if (faceDirX < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (faceDirX > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }
}