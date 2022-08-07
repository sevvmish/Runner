using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GeneralSettings settings;
    [SerializeField] private Camera mainCam;

    private Transform player;
    private Rigidbody playerRigidbody;
    private float movementCoolDown, jumpCoolDown;
    private Vector3 currMousePos, oldMousePos, deltaMousePos;
    private bool isTouchDetected;

    private const float DEF_MOVE_COOLDOWN = 0.5f;
    private const float DEF_JUMP_COOLDOWN = 1.5f;
    private const float DEF_TURN_LENGHT = 2f;


    public static PlayerPositionsInLine CurrentPositionInLine { get; private set; }
    public static PlayerStates CurrentPlayerState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody>();
        player.position = Vector3.zero;
        CurrentPositionInLine = PlayerPositionsInLine.center;
        CurrentPlayerState = PlayerStates.isRunning;

        GameObject pl = Instantiate(Resources.Load<GameObject>("player"), Vector3.zero, Quaternion.identity, transform);
        pl.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCoolDown<DEF_JUMP_COOLDOWN)
        {
            jumpCoolDown += Time.deltaTime;
        }

        if (movementCoolDown>DEF_MOVE_COOLDOWN)
        {
            if (Input.GetMouseButton(0))
            {
                currMousePos = Input.mousePosition;

                if (!isTouchDetected)
                {
                    deltaMousePos = Vector3.zero;
                    oldMousePos = currMousePos;
                }
                else
                {
                    deltaMousePos = currMousePos - oldMousePos;
                    oldMousePos = currMousePos;
                }

                isTouchDetected = true;

                //if (deltaMousePos!=Vector3.zero) print(deltaMousePos);

                if (Mathf.Abs(deltaMousePos.x) > 5 || Mathf.Abs(deltaMousePos.y) > 5)
                {
                    movementCoolDown = 0;

                    if (Mathf.Abs(deltaMousePos.x) > Mathf.Abs(deltaMousePos.y)) //left right
                    {
                        if (deltaMousePos.x > 0) //right
                        {
                            MakeTurnRight();
                        }
                        else //left
                        {
                            MakeTurnLeft();
                        }
                    }
                    else
                    {
                        if (deltaMousePos.y > 0 && jumpCoolDown>=DEF_JUMP_COOLDOWN) //up
                        {
                            jumpCoolDown = 0;
                            MakeJump();
                        }
                        else //down
                        {

                        }
                    }
                                        
                    isTouchDetected = false;
                }
            }
            else
            {
                isTouchDetected = false;           
            }

        }
        else
        {
            movementCoolDown += Time.deltaTime;
        }
    }


    private void MakeTurnLeft()
    {
        if (CurrentPositionInLine == PlayerPositionsInLine.right)
        {            
            CurrentPositionInLine = PlayerPositionsInLine.center;
            player.DOMove(new Vector3(player.position.x - DEF_TURN_LENGHT, player.position.y, player.position.z), DEF_MOVE_COOLDOWN);
        }
        else if (CurrentPositionInLine == PlayerPositionsInLine.center)
        {            
            CurrentPositionInLine = PlayerPositionsInLine.left;
            player.DOMove(new Vector3(player.position.x - DEF_TURN_LENGHT, player.position.y, player.position.z), DEF_MOVE_COOLDOWN);
        }
        else if (CurrentPositionInLine == PlayerPositionsInLine.left)
        {
            return;
        }
    }

    private void MakeJump()
    {
        playerRigidbody.AddForce(Vector3.up * 6, ForceMode.Impulse);
    }

    private void MakeTurnRight()
    {
        if (CurrentPositionInLine == PlayerPositionsInLine.left)
        {            
            CurrentPositionInLine = PlayerPositionsInLine.center;
            player.DOMove(new Vector3(player.position.x + DEF_TURN_LENGHT, player.position.y, player.position.z), DEF_MOVE_COOLDOWN);
        }
        else if (CurrentPositionInLine == PlayerPositionsInLine.center)
        {            
            CurrentPositionInLine = PlayerPositionsInLine.right;
            player.DOMove(new Vector3(player.position.x + DEF_TURN_LENGHT, player.position.y, player.position.z), DEF_MOVE_COOLDOWN);
        }
        else if (CurrentPositionInLine == PlayerPositionsInLine.right)
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            print("DEATH!!!!!!!!!");
        }
    }








}

public enum PlayerPositionsInLine
{
    left = 1,
    center = 2,
    right = 3
}

public enum PlayerStates
{
    isRunning,
    isJumping,
    isTurning,
    isDead
}
