using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoHo : MonoBehaviour
{
    private static CoHo _instance;
    public static CoHo Instance
    {
        get
        {
            if ( !_instance )
            {
                _instance = new GameObject ( ).AddComponent<CoHo> ( );
                DontDestroyOnLoad ( _instance.gameObject );
            }
            return _instance;
        }
    }
}

public static class CoHoExtensions
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

    public static void WaitAndCallback ( this MonoBehaviour mono, float time, Action callback )
    {
        mono.StartCoroutine ( Co_WaitAndCallback ( time, callback ) );
    }

    public static IEnumerator Co_DoWhen ( Func<bool> condition, Action callback )
    {
        while ( !condition ( ) )
        {
            yield return null;
        }
        callback.SafeCall ( );
    }

    public static void DoWhen ( this MonoBehaviour mono, Func<bool> condition, Action callback )
    {
        mono.StartCoroutine ( Co_DoWhen ( condition, callback ) );
    }

    public static IEnumerator Co_PlayAll ( MonoBehaviour mono, IEnumerable<IEnumerator> coroutines, Action callback = null )
    {
        foreach ( var co in coroutines )
        {
            yield return mono.StartCoroutine ( co );
        }
        callback.SafeCall ( );
    }

    public static void PlayAll ( this MonoBehaviour mono, IEnumerable<IEnumerator> coroutines, Action callback = null )
    {
        mono.StartCoroutine ( Co_PlayAll ( mono, coroutines, callback ) );
    }
}