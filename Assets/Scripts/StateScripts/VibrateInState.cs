using UnityEngine;
using System.Collections;

public class VibrateInState : StateMachineBehaviour {

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Handheld.Vibrate();
	}
}
