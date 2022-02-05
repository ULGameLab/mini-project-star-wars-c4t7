using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

[RequireComponent(typeof(NavMeshAgent))]
public class NMEStateMachine : MonoBehaviour
{
    public enum State { Idle, KillLuke, BobaGoWeeInDaSky}
    public State CurState;

    [SerializeField] private Transform Plyr;
    private UnityEngine.AI.NavMeshAgent AI;
    private NavMeshAgent NavAg;

    private Dictionary<State, System.Action> enter;
    private Dictionary<State, System.Action> exit;
    private Dictionary<State, System.Action> execute;

    [Tooltip("default speed for navmeshes is 3.5")]
    public float SPD;

    //Idle variables
    bool isIdle;
    Vector3 IPos;
    Vector3 IRot;

    //KillLuke variables
    [Tooltip("175 for troopers, 255 for boba")]
    public float VisRng;
    [Tooltip("distance to see luke, probably 25")]
    public float VisDist;

    bool KillOnce;
    Vector3 KPos;
    
    //bobaGoWeeInDaSky variables
    public bool isBoba;

    // Start is called before the first frame update
    void Start()
    {
        Plyr = GameObject.FindGameObjectWithTag("Player").transform;
        AI = GetComponent<UnityEngine.AI.NavMeshAgent>();

        isIdle = true;
        IPos = transform.position;
        IRot = transform.forward;
        
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

        CurState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        execute[CurState]();
    }

    private bool CanSee()
    {
        Vector3 toPlyr = Plyr.position - transform.position;
        float AglToPlyr = Vector3.Angle(transform.forward, toPlyr);
        return AglToPlyr < VisRng && Vector3.Distance(transform.position, Plyr.position) < 30;
    }

    private bool InRange()
    {
        return Vector3.Distance(transform.position, Plyr.position) < VisDist;
    }

    void Transition(State nextState)
    {
        exit[CurState]();
        CurState = nextState;
        enter[CurState]();
    }

    //State Behaviors
    //Idle: q
    void entI()
    {

    }
    void extI()
    {
        isIdle = false;
    }
    void excI()
    {
        //if not there, return to idle possition, then stop checking the if statments
        if (!isIdle)
        {
            if (transform.position != IPos) { AI.destination = IPos; }
            else { 
                transform.rotation = Quaternion.LookRotation(IRot);
                isIdle = true;
            }
        }
        
        //Debug.Log("execute Idle")
        if (Input.GetKey(KeyCode.W))//change to can see luke later
        {
            Transition(State.KillLuke);
        }
    }

    //KillLuke: w
    void entK()
    {
        KillOnce = true;
    }
    void extK()
    {
        StopCoroutine("Strafe");
    }
    void excK()
    {
        //circle luke
        if (KillOnce)
        {
            InvokeRepeating("Strafe", 0f, 2f);
            KillOnce = false;
        }

        //shoot luke
        transform.LookAt(Plyr.transform);
        //shoot luke here

        //Debug.Log("execute KillLuke");
        if (Input.GetKey(KeyCode.Q))//replace with luke being out of signt
        {
            Transition(State.Idle);
        }
        if (Input.GetKey(KeyCode.E))//only have boba use this. lock the transition on some timer AND isBoba
        {
            Transition(State.BobaGoWeeInDaSky);
        }
    }
    void Strafe()
    {
        Vector2 tempv2 = Random.insideUnitCircle;
        KPos = (Plyr.transform.position + (7 * Plyr.transform.forward) + (6 * new Vector3(tempv2.x, 0f, tempv2.y)));
        AI.destination = KPos;
    }

    //BobaGoWeeInDaSky: e
    void entB()
    {

    }
    void extB()
    {

    }
    void excB()
    {
        //big jetpack jump
        StartCoroutine(JetpackJump());
    }
    IEnumerator JetpackJump()//speed up the movement speed of the navmesh, this allows for boba to fall down faster than a gentle drift
    {
        float tempf = 0;
        AI.speed = SPD * 8;
        while (tempf < 257)
        {
            transform.position += new Vector3(0, 0.017f, 0f);
            tempf += 1;
            yield return new WaitForSecondsRealtime(.001f);
        }
        AI.speed = SPD;
        Transition(State.KillLuke);
    }
}