using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private bool isBeingHeld = false;
    private bool isBeingPassed = false;
    private GameObject currentAttacker;
    private AttackerBehaviour passTarget;
    private float passSpeed;

    private float catchDistance;
    private void Awake()
    {
        
    }
    void Start()
    {
        var gc = FindObjectOfType<GameController>();

        if(gc.GetIsARMode())
        {
            catchDistance = 0.005f;
        }
        else
        {
            catchDistance = 0.5f;
        }
    }

   /* Handles the ball being passed from one attacker to another.
    * If the ball is being passed, checks the distance to target. 
    * If the distance is low enough, the ball is caught by the pass target.
    */
    void Update()
    {

        if(isBeingPassed)
        {
            var vect2Position = new Vector2(transform.position.x, transform.position.z);
            var vect2TargetPosition = new Vector2(passTarget.transform.position.x, passTarget.transform.position.z);
            if (Vector2.Distance(vect2Position, vect2TargetPosition) > catchDistance)
            {
                float step = passSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, passTarget.transform.position, step);
            }
            else
            {
                passTarget.CatchPass();
                isBeingPassed = false;
            }
        }
    }

    /// <summary>
    /// Returns true if the ball is being held by an attacker, false otherwise.
    /// </summary>
    /// <returns>Returns whether the ball is held or not.</returns>
    public bool GetIsBeingHeld()
    {
        return this.isBeingHeld;
    }


    /// <summary>
    /// Starts a pass from the current ball holder to the target with the specified speed
    /// </summary>
    /// <param name="target">The pass target.</param>
    /// <param name="speed">The pass speed.</param>
    /// <returns>Returns whether the ball is held or not.</returns>
    public void Pass(AttackerBehaviour target,float speed)
    {
        this.isBeingPassed = true;
        this.passTarget = target;
        this.passSpeed = speed;
    }


    /// <summary>
    /// Sets the new ball holder
    /// </summary>
    /// <param name="attacker">The new ball holder.</param>
    public void SetHeld(AttackerBehaviour attacker)
    {
        isBeingHeld = true;
        currentAttacker = attacker.gameObject;
        this.transform.position = attacker.GetBallHoldingPosition();
        this.transform.SetParent(currentAttacker.transform);
    }
}
