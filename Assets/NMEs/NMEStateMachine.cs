using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
enemy state machine with 3 states.
[1] Idle
[2] KillLuke
[3] BobaGoWeeInDaSky
[1>2] see player
[2>1] can't see player
[2>3] special conditional for boba to jetpack jump for a bit
[3>2] special conditional for Boba to stop jetpacking, probably gonna be a timer so he dosen't just stay up there for too long.
*/

public class NMEStateMachine : MonoBehaviour
{
    public enum State { Idle, KillLuke, BobaGoWeeInDaSky}
    public State CurState;

    private Dictionary<State, System.Action> enter;
    private Dictionary<State, System.Action> exit;
    private Dictionary<State, System.Action> execute;

    // Start is called before the first frame update
    void Start()
    {
        enter = new Dictionary<State, System.Action>()
        {
            {State.Idle, entI },
            {State.KillLuke, entK },
            {State.BobaGoWeeInDaSky, entB }
        };
        exit = new Dictionary<State, System.Action>()
        {
            {State.Idle, extI },
            {State.KillLuke, extK },
            {State.BobaGoWeeInDaSky, extB }
        };
        execute = new Dictionary<State, System.Action>()
        {
            {State.Idle, excI },
            {State.KillLuke, excK },
            {State.BobaGoWeeInDaSky, excB }
        };

    }

    // Update is called once per frame
    void Update()
    {
        execute[CurState]();
    }

    void Transition(State nextState)
    {
        exit[CurState]();
        CurState = nextState;
        enter[CurState]();
    }

    //State Behaviors
    //Idle: Alpha1
    void entI()
    {
        Debug.Log("enter Idle");
    }
    void extI()
    {
        Debug.Log("exit Idle");
    }
    void excI()
    {
        Debug.Log("execute Idle");
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Transition(State.KillLuke);
        }
    }

    //KillLuke: Alpha2
    void entK()
    {
        Debug.Log("enter KillLuke");
    }
    void extK()
    {
        Debug.Log("exit KillLuke");
    }
    void excK()
    {
        Debug.Log("execute KillLuke");
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Transition(State.Idle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Transition(State.BobaGoWeeInDaSky);
        }
    }

    //BobaGoWeeInDaSky: Alpha3
    void entB()
    {
        Debug.Log("enter BobaGoWeeInDaSky");
    }
    void extB()
    {
        Debug.Log("exit BobaGoWeeInDaSky");
    }
    void excB()
    {
        Debug.Log("execute BobaGoWeeInDaSky");
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Transition(State.KillLuke);
        }
    }


}
