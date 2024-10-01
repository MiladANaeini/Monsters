using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public virtual void enterState() { }
    public virtual void exitState() { }
    public virtual void updateState() { }
}
