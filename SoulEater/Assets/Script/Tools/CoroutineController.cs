using System;
using System.Collections;
using UnityEngine;

public class CoroutineController<T>
{
    /// <summary>
    /// Use this to get coroutine yield return data.
    /// </summary>
    public Coroutine CoroutineData { get; private set; }
    public T result;
    private readonly IEnumerator target;

    public CoroutineController(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.CoroutineData = owner.StartCoroutine(Run());
    }

    public CoroutineController(MonoBehaviour owner, IEnumerator target, Action<T> setter)
    {
        this.target = target;
        this.CoroutineData = owner.StartCoroutine(Run(setter));
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = (T)target.Current;
            yield return result;
        }
    }

    private IEnumerator Run(Action<T> setter)
    {
        while (target.MoveNext())
        {
            result = (T)target.Current;
            setter.Invoke(result);
            yield return result;
        }
    }
}
