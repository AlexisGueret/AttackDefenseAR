using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    private PlayerControls playerInput;
    [SerializeField]
    GameController gameController;


    private float startPressTime;
    private Vector2 startPressPosition;



    private void Start()
    {
        //Used for touch recognition
        playerInput.Controls.PrimaryTouch.started += ctx => StartTouch(ctx);
        playerInput.Controls.PrimaryTouch.canceled += ctx => EndTouch(ctx);

        //Used for Swipe recognition
        playerInput.Controls.Press.started += ctx => StartPress(ctx);
        playerInput.Controls.Press.canceled += ctx => EndPress(ctx);
    }


    private void StartTouch(InputAction.CallbackContext context)
    {

        if (gameController != null)
            gameController.HandleTouch(playerInput.Controls.PrimaryTouchPosition.ReadValue<Vector2>());


    }
   private void EndTouch(InputAction.CallbackContext context)
    {
    }


    /*Horizontal Swipe recognition: 
     * First, we compute the time difference between the start of press and the end of press.
     * If the time difference is significant enough, we compute the position difference between the two presses.
     * If this position difference is significant enough, the gesture is recognized as a swipe and used to rotate the football field
     * 
     */
    private void StartPress(InputAction.CallbackContext context)
    {
        startPressTime = Time.time;
        startPressPosition = playerInput.Controls.PrimaryTouchPosition.ReadValue<Vector2>();
    }


    private void EndPress(InputAction.CallbackContext context)
    {
        if(Time.time - startPressTime >0.1f)
        {
            //Swipe performed
            var endPressPosition = playerInput.Controls.PrimaryTouchPosition.ReadValue<Vector2>();
            var swipeMovement = endPressPosition.x - startPressPosition.x;
            if (swipeMovement<-200)
            {
                //Swipe left
                if(gameController!=null)
                {
                    gameController.RotateField(90);
                }
            }

            if (swipeMovement > 200)
            {
                //Swipe right
                if (gameController != null)
                {
                    gameController.RotateField(-90);
                }
            }

        }

    }

 
    private void Awake()
    {
        playerInput = new PlayerControls();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
