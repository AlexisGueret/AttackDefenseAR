using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefenderBehaviour : MonoBehaviour
{
    [SerializeField]
    DefenderData defenderData;
    private NavMeshAgent agent;
    private bool isActive;
    private GameObject detectionCircle;
    private AttackerBehaviour currentTarget;
    private float detectionRange;
    private bool returnToStartPosition = false;
    private Vector3 startPosition;
    private Vector2 vect2Start;
    private GameController gameController;
    [SerializeField]
    SkinnedMeshRenderer meshRenderer;
    private Material normalMaterial;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        normalMaterial = meshRenderer.material;
        gameController = FindObjectOfType<GameController>();
        isActive = false;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = defenderData.normalSpeed;
        startPosition = this.transform.position;
        vect2Start = new Vector2(this.transform.position.x, this.transform.position.z);
    }

    void Start()
    {
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if(returnToStartPosition)
        {
            var vect2Position = new Vector2(this.transform.position.x, this.transform.position.z);
            
            if (Vector2.Distance(vect2Position, vect2Start) > 0.1f)
            {
                agent.SetDestination(startPosition);
            }
                
            else
            {
                returnToStartPosition = false;
                detectionCircle.SetActive(true);
                animator.SetInteger("State", 0);
            }
                
            
        }
        else
        {
            if (currentTarget == null)
            {
                FindClosestTarget();
            }
            else
            {
                if(!currentTarget.GetIsActive() || !currentTarget.GetIsHoldingTheBall())
                {
                    returnToStartPosition = true;
                    return;
                }
                if (Vector3.Distance(this.transform.position,currentTarget.gameObject.transform.position) > defenderData.inactivateDistance)
                {
                    agent.SetDestination(currentTarget.transform.position);
                }
                else
                {
                    currentTarget.InactivateAttacker();
                    StartCoroutine(Inactivate());
                }
            }
        }
        
    }

    private void FindClosestTarget()
    {
        var position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        AttackerBehaviour[] attackList = GameObject.FindObjectsOfType<AttackerBehaviour>();
        foreach(AttackerBehaviour attacker in attackList)
        {
            if (!attacker.GetIsHoldingTheBall())
                continue;

            var attackerPosition = new Vector2(attacker.gameObject.transform.position.x, attacker.gameObject.transform.position.z);

            if (Vector2.SqrMagnitude(position-attackerPosition)< detectionRange*detectionRange)
            {
                this.currentTarget = attacker;
                detectionCircle.SetActive(false);
                animator.SetInteger("State", 1);
                break;                
            }
        }
    }

    //Routine called at the spawn of the attacker
    IEnumerator Spawning()
    {
        this.SetColor(defenderData.inactiveMaterial);
        yield return new WaitForSeconds(defenderData.spawnTime);
        isActive = true;
        this.SetColor(normalMaterial);
        CreateDetectionZone();
    }

    public void SetColor(Material mat)
    {
        meshRenderer.GetComponent<SkinnedMeshRenderer>().material = mat; 
    }

    private void CreateDetectionZone()
    {
        float ARModificator=1f;
        if (gameController.GetIsARMode())
            ARModificator = 0.1f;

        var fieldWidth = GameObject.FindGameObjectWithTag("Field").GetComponent<BoxCollider>().size.y;
        fieldWidth *= ARModificator;
        detectionRange = fieldWidth * defenderData.detectionField / 2;
        detectionCircle = new GameObject { name = "Detection Circle" };      
        detectionCircle.DrawCircle(detectionRange, 0.2f * ARModificator);
        detectionCircle.transform.position = this.transform.position - new Vector3(0, 0.9f*ARModificator, 0);

            
        detectionCircle.GetComponent<LineRenderer>().material = defenderData.detectionMaterial;
        detectionCircle.transform.SetParent(this.transform);
        detectionCircle.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    //Routine called when the defender is inactivated
    IEnumerator Inactivate()
    {
        if(this.agent.isOnNavMesh)
        this.agent.SetDestination(this.transform.position);
        if (currentTarget != null)
            this.transform.LookAt(currentTarget.transform.position);
        animator.SetInteger("State", 2);
        animator.SetTrigger("Knockout");
        this.SetColor(defenderData.inactiveMaterial);
        isActive = false;
        currentTarget = null;

        if(this.agent.isOnNavMesh)
            this.agent.SetDestination(this.transform.position);

        yield return new WaitForSeconds(defenderData.reactivateTime);
        animator.SetInteger("State", 1);
        returnToStartPosition = true;
        isActive = true;
        this.SetColor(normalMaterial);
    } 
}
public static class GameObjectEx
{
    public static void DrawCircle(this GameObject container, float radius, float lineWidth)
    {
        var segments = 360;
        var line = container.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;
        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}

