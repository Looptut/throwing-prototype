using System;
using System.Collections;
using UnityEngine;

public class CoroutineHandle : IEnumerator
{
    public event Action<CoroutineHandle> OnComplete = delegate { };
    public bool IsDone { get; private set; }

    public object Current { get; }

    public bool MoveNext() => !IsDone;

    public void Reset() { }

    public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
    {
        Current = owner.StartCoroutine(Wrap(coroutine));
    }
    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
        OnComplete.Invoke(this);
    }
}
