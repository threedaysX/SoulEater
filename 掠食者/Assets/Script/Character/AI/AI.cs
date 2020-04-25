using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AI : Character 
{
    
    [Header("偵測動作")]
    [SerializeField] protected Detect[] detects;
    [Header("行為模式")]
    [SerializeField] protected Action[] actions;
    [Header("上一個行為模式")]
    [SerializeField] public Action lastAction;
    private bool lastActionSuccess;

    [Header("偵測距離")]
    public float detectDistance;
    [Header("行為共通延遲時間(秒)")]
    public float overridedAactionDelay = 0f;  
    private float nextActTimes = 0f;

    private bool inCombatState = false; // 是否進入戰鬥狀態
    private float outOfCombatDuration = 3f; // 脫離戰鬥狀態所需時間
    private float nextOutOfCombatTime = 0f;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public LayerMask playerLayer;
    [HideInInspector] public DistanceDetect distanceDetect;

    private void Start()
    {
        this.gameObject.AddComponent(typeof(DistanceDetect));
        distanceDetect = GetComponent<DistanceDetect>();
    }

    void Update()
    {
        playerLayer = LayerMask.GetMask("Player");

        DoDetects();

        CheckOutOfCombatState();
    }

    private void CheckOutOfCombatState()
    {
        if (inCombatState && nextOutOfCombatTime == 0)
        {
            nextOutOfCombatTime = Time.time + outOfCombatDuration;
        }

        if (Time.time >= nextOutOfCombatTime && inCombatState)
        {
            inCombatState = false;
        }

        // 脫離戰鬥後，重置所有行動權重
        if (!inCombatState && nextOutOfCombatTime != 0)
        {
            nextOutOfCombatTime = 0;
            foreach (var action in actions)
            {
                action.ResetWeight();
            }
        }
    }

    public void DoDetects()
    {
        foreach (var detect in detects)
        {
            detect.GetCurrentAIHavior(this);
            if (detect.StartDetectHaviour())
            {
                if (Time.time >= nextActTimes)
                {
                    inCombatState = true;
                    DoActions();
                    nextActTimes = Time.time + overridedAactionDelay + GetAnimationLength(animator);
                }
                else
                {
                    AlwaysFaceTarget(); // 在執行Action以外，會持續面對目標
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
            action.GetCurrentAIHavior(this);
            if (action.CheckActionThatCanDo())
            {
                if (action == lastAction && !lastActionSuccess)
                {
                    action.ActionWeight -= 2;
                    action.AddDiffCount(2);
                }
                actionToDoList.Add(action, action.ActionWeight);
            }            
        }

        // 如果沒動作可以做，就隨機抓一個
        if (actionToDoList.Count == 0)
        {
            int random = Random.Range(0, actions.Length);
            Action randomAct = actions[random];
            actionToDoList.Add(randomAct, randomAct.ActionWeight);
        }

        // 決定最後的動作
        DoHightestWeightAction(actionToDoList);
    }

    protected void DoHightestWeightAction(Dictionary<Action, int> actionToDoList)
    {
        Action action;
        if (actionToDoList.Count == 1)
        {
            action = actionToDoList.Keys.First();
            overridedAactionDelay = action.actionDelay;
            lastActionSuccess = action.StartActHaviour();
            lastAction = action;
            return;
        }

        // 找到權重最高和-N點權重的那些動作
        int diff = 1;
        int maxWeight = actionToDoList.Values.Max();
        Action[] keyActionsOfMaxWeight = actionToDoList.Where(x => x.Value <= maxWeight && x.Value >= maxWeight - diff).Select(x => x.Key).ToArray();
        // 執行任一動作，但除了移動以外，不會做相同的動作
        action = keyActionsOfMaxWeight[Random.Range(0, keyActionsOfMaxWeight.Length)];
        if (action == lastAction && action.actionType != ActionType.Move)
        {
            action.ActionWeight -= 1;    // 做重複的動作，導致權重下降1，降低這個對於動作的慾望
            action.AddDiffCount(1);
            return;
        }
        overridedAactionDelay = action.actionDelay;
        lastActionSuccess = action.StartActHaviour();
        lastAction = action;
    }

    protected float GetAnimationLength(Animator anim)
    {
        if (anim == null)
            return 0;
        return AnimationBase.Instance.GetCurrentAnimationLength(anim);
    }

    private void AlwaysFaceTarget()
    {
        if (chaseTarget == null)
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