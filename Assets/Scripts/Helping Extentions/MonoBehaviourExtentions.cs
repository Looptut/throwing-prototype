using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtentions
{
    public static CoroutineHandle RunCoroutine(this MonoBehaviour owner,
        IEnumerator coroutine)
    {
        return new CoroutineHandle(owner, coroutine);
    }
}