using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
enemy state machine with 3 states.
[1] Idle
[2] KillLuke
[3] BobaGoWeeInDaSky
[1>2] see player and in range of sight
[2>1] can't see player
[2>3] (funky looping counter math with a % and a range fo incrimenting) for boba to jetpack jump for a bit
[3>2] instant, jetpack is a coroutine so boba can shoot at the player while in the sky
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
    public AudioSource sounds;
    public AudioClip pew;
    bool KillOnce;
    Vector3 KPos;
    public Transform GunTipT;
    GameObject SelectedPre;
    GameObject Fired;
    public GameObject BlastetBolt;
    public GameObject Rocket;
    int rocketMult = 3;
    float SelectedF;
    public float BlasterBoltF = 5.5f;
    public float RocketF = 1.7f;
    
    //bobaGoWeeInDaSky variables
    public bool isBoba;
    int JmpTme = 0;

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
        if (InRange() && CanSee()/*Input.GetKey(KeyCode.W)*/)//change to can see luke later
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
        //math on the random actions that enemies can take
        JmpTme = (JmpTme + Random.Range(0, 3)) % 550;
        if(JmpTme > 385 && isBoba) { SelectedPre = Rocket; SelectedF = RocketF; }
        else { SelectedPre = BlastetBolt; SelectedF = BlasterBoltF; }

        //circle luke
        if (KillOnce)
        {
            InvokeRepeating("Strafe", 0f, 8f);//Roberto changed this seens to work kind of better with A high repetitiong
            KillOnce = false;
        }

        //shoot luke
        transform.LookAt(Plyr.transform);

        //instantiate bullets here
        if (SelectedPre == Rocket) { if (JmpTme % 70 * rocketMult == 0) { Fire(); } }
        else { if (JmpTme % 70 == 0) { Fire(); } }

        //transitions
        //Debug.Log("execute KillLuke");
        if (!CanSee()/*Input.GetKey(KeyCode.Q)*/)//replace with luke being out of signt
        {
            Transition(State.Idle);
        }
        if (isBoba && (JmpTme == 30 /*|| JmpTme == 120*/ || JmpTme == 250 || JmpTme == 251 /*|| JmpTme == 86 || JmpTme == 85 || JmpTme == 195*/)/*Input.GetKey(KeyCode.E)*/)
        {
            Transition(State.BobaGoWeeInDaSky);
        }
    }
    void Strafe()
    {
        Vector2 tempv2 = Random.insideUnitCircle;
        KPos = (Plyr.transform.position + (Random.RandomRange(7, 13) * Plyr.transform.forward) + (Random.Range(6, 12) * new Vector3(tempv2.x, 0f, tempv2.y)));//Roberto changed this It add a bit more randoness to the behavior
        AI.destination = KPos;
    }
    void Fire()
    {
        sounds.PlayOneShot(pew);
        Fired = Instantiate(SelectedPre);
        Fired.transform.position = GunTipT.position;
        Fired.transform.rotation = GunTipT.rotation;
        Fired.GetComponent<Rigidbody>().mass = .2f;

        Vector3 FinalF = (GunTipT.forward * SelectedF);

        Fired.GetComponent<Rigidbody>().velocity = transform.GetComponent<Rigidbody>().velocity;
        Fired.GetComponent<Rigidbody>().AddForce(FinalF, ForceMode.Impulse);
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
        //auto transition, its in the coroutine
        //big jetpack jump
        StartCoroutine(JetpackJump());
    }
    IEnumerator JetpackJump()//the jetpack jump boba does.
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