using UnityEngine;
using System.Collections;

public class StateMachineBehaviourEvents: StateMachineBehaviour
{ 
    public delegate void StateEventHandler(AnimatorStateInfo stateInfo);
    public delegate void StateUpdateEventHandler(AnimatorStateInfo stateInfo, float deltaTime);

    public event StateEventHandler StateEnterEvent;
    public event StateEventHandler StateExitEvent;
    public event StateUpdateEventHandler StateUpdateEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateEnterEvent != null)
            StateEnterEvent(stateInfo);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateUpdateEvent != null)
            StateUpdateEvent(stateInfo, Time.deltaTime);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateExitEvent != null)
            StateExitEvent(stateInfo);
    }
}
