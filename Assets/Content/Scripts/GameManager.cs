using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stately;

public class GameManager: MonoBehaviour
{
    private State rootState = new State("root");
    private State idleState = new State("idle");
    private State jumpState = new State("move");
    private State coolDownState = new State("coolDown");

    private Vector3 targetPos;

    private float speed = 10.0f;

    [SerializeField]
    private Renderer coolDownBar;
    private Material coolDownBarMat;

    // The distance each jump should travel in world units.
    private float jumpDist = 2.0f;
    private float jumpDur = 2.0f;
    private float coolDownDur = 2.0f;

    private struct CurveValues
    {
        public float startTimeStamp;
        public float t01; // our current place along the vertical axis from 0 to 1; 
        public float t; // current value (time, usually Time.TimeSinceLevelLoad - startTimeStamp)
        public float b; // start value (start time, usually 0.0f)
        public float c; // change in value (increment time, usually deltaTime)
        public float d; // duration (end time)
    }

    private CurveValues jumpCurve;
    private CurveValues coolDownCurve;

    void Awake()
    {
        DefineStateMachine();
    }

    void Start()
    {
        coolDownBarMat = coolDownBar.material;
        targetPos = transform.position;

        jumpCurve = new CurveValues();
        coolDownCurve = new CurveValues();

        rootState.Start();
    }

    void Update()
    {
        rootState.Update(Time.deltaTime);
        Debug.Log(rootState.CurrentStatePath);

    }

    public void DefineStateMachine()
    {
        rootState.StartAt(idleState);

        idleState.ChangeTo(jumpState).If(() => Input.GetKeyDown(KeyCode.Space));

        jumpState.OnEnter = delegate
        {
            jumpCurve.startTimeStamp = Time.timeSinceLevelLoad;
            jumpCurve.t01 = 0.0f;
            jumpCurve.b = 0.0f;
            jumpCurve.d = jumpDur;
        };

        jumpState.OnUpdate = (deltaTime) =>
        {
            jumpCurve.t = Time.timeSinceLevelLoad - jumpCurve.startTimeStamp;
            jumpCurve.c = deltaTime; 
            float val = EasingCurveHelper.EaseOutQuad(jumpCurve.t, jumpCurve.b, jumpCurve.c, jumpCurve.d);
            jumpCurve.t01 += val;
            // print(jumpCurve.t01);
            transform.position += val * jumpDist * Vector3.forward;

            coolDownBarMat.SetFloat("_Level", 1.0f - jumpCurve.t01);
        };

        jumpState.ChangeTo(coolDownState).If(() => jumpCurve.t01 > .999f);

        coolDownState.OnEnter = delegate 
        {
            coolDownCurve.startTimeStamp = Time.timeSinceLevelLoad;
            coolDownCurve.t01 = 0.0f;
            coolDownCurve.b = 0.0f;
            coolDownCurve.d = coolDownDur;
        };

        coolDownState.OnUpdate = (deltaTime) =>
        {
            coolDownCurve.t = Time.timeSinceLevelLoad - coolDownCurve.startTimeStamp;
            coolDownCurve.c = deltaTime;
            float val = EasingCurveHelper.EaseInQuad(coolDownCurve.t, coolDownCurve.b, coolDownCurve.c, coolDownCurve.d);
            coolDownCurve.t01 += val;

            coolDownBarMat.SetFloat("_Level", coolDownCurve.t01);
        };

        coolDownState.ChangeTo(idleState).If(() => coolDownCurve.t01 > .999f);
    }

}
