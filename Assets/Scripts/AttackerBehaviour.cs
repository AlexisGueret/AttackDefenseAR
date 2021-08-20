using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehaviour : MonoBehaviour
{
    [SerializeField]
    private AttackerData attackerData;
    private bool isActive;
    private bool isBallHolder=false;
    private bool isWaitingForPass = false;
    private BallBehaviour ball;
    private NavMeshAgent agent;
    private GameObject defenderGate;
    private GameController gameController;

    [SerializeField]
    private Material enemyMat, playerMat;

    private bool isControlledByPlayer;
    Animator animator;

    [SerializeField]
    private Transform ballHoldingPosition;

    [SerializeField]
    SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    private GameObject highlightSprite;

    private Material normalMaterial;
    private void Awake()
    {
        normalMaterial = meshRenderer.material;
        gameController = GameObject.FindObjectOfType<GameController>();
        isActive = false;
        agent = GetComponent<NavMeshAgent>();
        defenderGate = GameObject.FindGameObjectWithTag("Gate");
        agent.speed = attackerData.normalSpeed;
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        StartCoroutine(Spawning());
    }


    public bool GetIsActive()
    {
        return this.isActive;
    }

    void Update()
    {

        if (!isActive)
            return;

        if (isWaitingForPass)
            return;


        if(ball ==null)
        {
            ball = GameObject.FindObjectOfType<BallBehaviour>();
        }
        /* If the ball is being held by an attacker, the destination will be the enemy gate 
         * otherwise, the destination is the ball
         */

        var vect2Position = new Vector2(this.transform.position.x, this.transform.position.z);
        if(!ball.GetIsBeingHeld())
        {
            var vect2BallPosition = new Vector2(ball.gameObject.transform.position.x, ball.gameObject.transform.position.z);
            if(Vector2.Distance(vect2Position, vect2BallPosition) >attackerData.CatchBallDistance)
            {
                agent.SetDestination(ball.transform.position);
            }
                
            else
            {
                ReceiveBall();
            }
        }
        else
        {
            var vect2GatePosition = new Vector2(defenderGate.gameObject.transform.position.x, defenderGate.gameObject.transform.position.z);
            if (Vector2.Distance(vect2Position, vect2GatePosition) > attackerData.ScorePointDistance)
            {
                agent.SetDestination(defenderGate.transform.position);
            }
            /*enemy gate has been reached so if:  
             * attacker has the ball => it's a goal
             * attacker doesn't have the ball => destroyed
             * 
             */
            else
            {
                if (isBallHolder)
                {
                    gameController.AttackScorePoint();
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
    }

    /// <summary>
    /// Function called when an attacker receives the ball
    /// - If called during the maze game, the player wins
    /// - If called during the normal game, the attacker simply gets the ball
    /// </summary>
    private void ReceiveBall()
    {
        if(gameController.GetGameState()== GameState.PlayingMaze)
        {
            gameController.MazeVictory(true);
        }
        else
        {
            highlightSprite.SetActive(true);
            ball.SetHeld(this);
            isBallHolder = true;
            agent.speed = attackerData.carryingSpeed;
            animator.SetInteger("State", 2);
        }       
    }

    /// <summary>
    /// Routine called at the spawn of the attacker to make him inactive during a certain duration.
    /// </summary>
    IEnumerator Spawning()
    {
        isActive = false;
        meshRenderer.material = attackerData.inactiveMaterial;
        yield return new WaitForSeconds(attackerData.spawnTime);
        isActive = true;
        meshRenderer.material = normalMaterial;
        animator.SetInteger("State", 1);
    }


    /// <summary>
    /// Called by the defender who wants to inactivate this attacker.
    /// </summary>
    public void InactivateAttacker()
    {
        if (isActive)
            StartCoroutine(Inactivate());
    }

    private void PassBall(AttackerBehaviour target)
    {
        target.WaitForPass();
        ball.Pass(target,attackerData.ballSpeed);
        this.isBallHolder = false;
    }

    /// <summary>
    /// Makes the pass receiver wait while the ball is travelling from the passer to him.
    /// </summary>
    public void WaitForPass()
    {
        animator.SetInteger("State", 0);
        this.agent.destination = this.transform.position;
        this.isWaitingForPass = true;
    }

    /// <summary>
    /// After a pass is finished, makes the pass target the current ball handler.
    /// </summary>
    public void CatchPass()
    {
        animator.SetInteger("State", 2);
        this.isWaitingForPass = false;
        ReceiveBall();
    }


    /// <summary>
    /// Returns the closest attacker if there is one that is not inactive. Returns null otherwise.
    /// </summary>
    /// <returns>Returns the closest attacker from this attacker.</returns>
    private AttackerBehaviour FindClosestAttacker()
    {
        float currentMinDistance = 100000;
        AttackerBehaviour result=null;

        var position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        AttackerBehaviour[] attackList = GameObject.FindObjectsOfType<AttackerBehaviour>();
        foreach (AttackerBehaviour attacker in attackList)
        {
            if (attacker.GetIsHoldingTheBall() || !attacker.GetIsActive())
                continue;

            var attackerPosition = new Vector2(attacker.gameObject.transform.position.x, attacker.gameObject.transform.position.z);
            var distance = Vector2.Distance(position, attackerPosition);
            if(distance< currentMinDistance)
            {
                currentMinDistance = distance;
                result = attacker;
            }
        }

        if (result == null)
            return null;
        else
            return result;
    }

    /// <summary>
    /// Called after the attacker got inactivated to play a "fall animation". This coroutine enables a synchronization with the defender's animation.
    /// </summary>
    IEnumerator KnockoutAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Hit");

    }
    /// <summary>
    /// Coroutine called after the attacker is inactivated. He will be in standby for a fixed duration.
    /// If he has someone to ball the ball to, he will pass the ball.
    /// If not, the game is won by the defenders.
    /// </summary>
    IEnumerator Inactivate()
    {
        highlightSprite.SetActive(false);
        animator.SetInteger("State", 0);
        this.SetColor(attackerData.inactiveMaterial);
        StartCoroutine(KnockoutAnimation());       
        isActive = false;
        this.agent.SetDestination(this.transform.position);

        var passTarget = FindClosestAttacker();
        if(passTarget!=null)
        {
            PassBall(passTarget);
        }
        else
        {
            gameController.DefenseWin();
        }

        yield return new WaitForSeconds(attackerData.reactivateTime);
        isActive = true;
        this.SetColor(normalMaterial);
        animator.SetInteger("State", 1);
    }

    public void SetColor(Material mat)
    {       
        meshRenderer.GetComponent<SkinnedMeshRenderer>().material = mat;
    }

    public bool GetIsHoldingTheBall()
    {
        return this.isBallHolder;
    }

    public Vector3 GetBallHoldingPosition()
    {
        return this.ballHoldingPosition.position;
    }
}
