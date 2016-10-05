using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Symbol : MonoBehaviour
{
    public static event System.Action<Symbol> FullHitSymbolEvent;

    public TextMesh TextMesh;

    public int Index { get; private set; }

    // naming should be same as in symbolanimator controller states
    public enum EState
    {
        Idle,
        IsUnderFire,
        Die,
    }

    EState currState;
    StateMachineBehaviourEvents events;
    Animator symbolAnimator;

    float actHitTime;
    const float fullHitTime = 1f;
    static Dictionary<int, EState> animHashToState = Helper.CreateAnimatorStateDictionary<EState>(); //anim hash to state

    void Awake()
    {
        symbolAnimator = GetComponent<Animator>();
        symbolAnimator.GetBehaviour<PlayParticlesInState>().ParticleSystem = GetComponentInChildren<ParticleSystem>();

        events = symbolAnimator.GetBehaviour<StateMachineBehaviourEvents>();
        events.StateEnterEvent += StateEvents_StateEnterEvent;
    }

    void Destroy()
    {
        events.StateEnterEvent -= StateEvents_StateEnterEvent;
    }

    private void StateEvents_StateEnterEvent(AnimatorStateInfo stateInfo)
    {
        currState = animHashToState[stateInfo.shortNameHash];
        switch (currState)
        {
            case EState.Idle:
                break;
            case EState.IsUnderFire:
                break;
            case EState.Die:
                if (FullHitSymbolEvent != null)
                    FullHitSymbolEvent(this);

                Destroy(this.gameObject);
                break;
            default:
                break;
        }

        Debug.Log(this.gameObject.name + " : " + currState);
    }


    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case EState.Idle:
                break;
            case EState.IsUnderFire:
                actHitTime += Time.deltaTime;
                //Debug.Log(ActHitTime);
                if (actHitTime > fullHitTime)
                {
                    symbolAnimator.SetTrigger("Kill");
                }
                break;
            case EState.Die:
                break;
            default:
                break;
        }
    }

    public void SetIndex(int index)
    {
        Index = index;
        TextMesh.text = Index.ToString();
    }

    public void Kill()
    {
        symbolAnimator.SetTrigger("Kill");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Game.Instance.ActualSymbol == this)
        {
            symbolAnimator.SetTrigger("Hit");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (Game.Instance.ActualSymbol == this)
        {
            symbolAnimator.SetTrigger("Miss");
        }
    }

}
