using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoroutineHelper: MonoBehaviour
{
    private static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if ( !_instance )
            {
                _instance = new GameObject ( ).AddComponent<CoroutineHelper> ( );
                DontDestroyOnLoad ( _instance.gameObject );
            }
            return _instance;
        }
    }
}

public static class CoroutineHelperExtensions
{
    public static void SafeCall ( this Action action )
    {
        if ( action != null )
        {
            action ( );
        }
    }

    public static IEnumerator Co_WaitAndCallback ( float time, Action callback )
    {
        yield return new WaitForSeconds ( time );
        callback.SafeCall ( );
    }

    public static Coroutine WaitAndCallback ( this MonoBehaviour mono, float time, Action callback )
    {
        return mono.StartCoroutine ( Co_WaitAndCallback ( time, callback ) );
    }

    public static IEnumerator Co_DoWhen ( Func<bool> condition, Action callback )
    {
        while ( !condition ( ) )
        {
            yield return null;
        }
        callback.SafeCall ( );
    }

    public static Coroutine DoWhen ( this MonoBehaviour mono, Func<bool> condition, Action callback )
    {
        return mono.StartCoroutine ( Co_DoWhen ( condition, callback ) );
    }

    public static IEnumerator Co_DoWhile(Func<bool> condition, Action callback)
    {
        while (!condition())
        {
            callback.SafeCall();
            yield return null;
        }
    }

    public static Coroutine DoWhile(this MonoBehaviour mono, Func<bool> condition, Action callback)
    {
        return mono.StartCoroutine(Co_DoWhile(condition, callback));
    }

    //public static void StopCo(this MonoBehaviour mono, Coroutine coroutine)
    //{
    //    mono.StopCoroutine(coroutine);
    //}

    public static IEnumerator Co_PlayAll ( MonoBehaviour mono, IEnumerable<IEnumerator> coroutines, Action callback = null )
    {
        foreach ( var co in coroutines )
        {
            yield return mono.StartCoroutine ( co );
        }
        callback.SafeCall ( );
    }

    public static Coroutine PlayAll ( this MonoBehaviour mono, IEnumerable<IEnumerator> coroutines, Action callback = null )
    {
        return mono.StartCoroutine ( Co_PlayAll ( mono, coroutines, callback ) );
    }
}