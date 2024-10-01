using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public List<State> states = new List<State> ();
    public State currentState = null;

    public void switchState<aState>()
    {
        foreach (State s in states)
        {
            if (s.GetType() == typeof(aState))
            {
                currentState?.exitState();
                currentState = s;
                currentState?.enterState();
                return;
            }
        }
        Debug.LogWarning("State does not exist");
    }

    public virtual void updateStateMachine()
    {
        currentState?.updateState();
    }

    public bool isState<aState>()
    {
        if(!currentState) return false;
        return currentState.GetType() == typeof(aState);
    }

}
