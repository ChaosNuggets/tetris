using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    //public GameObject nextBox;
    //public GameObject board;
    private bool hasHorizontalBeenReleased = true;
    private float timeSinceLastDrop = 0;
    private const float SOFTDROPTIME = 1f / 60f;
    private const float REGULARDROPTIME = 1;
    private float timeBetweenDrops = REGULARDROPTIME;
    public static bool justLocked = false;

    // Update is called once per frame
    void Update()
    {
        handleHorizontalMovement();
        handleVerticalMovement();
    }

    private void handleHorizontalMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool isRightPressed = horizontal > 0;
        bool isLeftPressed = horizontal < 0;
        if (hasHorizontalBeenReleased)
        {
            bool isTouchingRightWall = ActivePieces.currentPiece.isTouchingRightWall();
            bool isTouchingPieceRight = ActivePieces.currentPiece.isTouchingPieceRight();
            bool isTouchingRight = isTouchingRightWall || isTouchingPieceRight;

            bool isTouchingLeftWall = ActivePieces.currentPiece.isTouchingLeftWall();
            bool isTouchingPieceLeft = ActivePieces.currentPiece.isTouchingPieceLeft();
            bool isTouchingLeft = isTouchingLeftWall || isTouchingPieceLeft;

            if (isRightPressed && !isTouchingRight)
            {
                ActivePieces.currentPiece.movePieceR(new Vector3(1, 0));
                hasHorizontalBeenReleased = false;
            } else if (isLeftPressed && !isTouchingLeft)
            {
                ActivePieces.currentPiece.movePieceR(new Vector3(-1, 0));
                hasHorizontalBeenReleased = false;
            }
        }
        if (horizontal == 0)
        {
            hasHorizontalBeenReleased = true;
        }
    }

    private void handleVerticalMovement()
    {
        handleDownInput();
        makePieceFall();
    }

    private void handleDownInput()
    {
        float vertical = Input.GetAxis("Vertical");
        bool isDownPressed = vertical < 0;
        if (isDownPressed)
        {
            timeBetweenDrops = justLocked ? REGULARDROPTIME : SOFTDROPTIME;
        } else
        {
            justLocked = false;
            timeBetweenDrops = REGULARDROPTIME;
        }
    }

    private void makePieceFall()
    {
        timeSinceLastDrop += Time.deltaTime;
        if (timeSinceLastDrop >= timeBetweenDrops)
        {
            bool isTouchingFloor = ActivePieces.currentPiece.isTouchingFloor();
            bool isTouchingPieceTop = ActivePieces.currentPiece.isTouchingPieceTop();
            if (isTouchingFloor || isTouchingPieceTop)
            {
                ActivePieces.currentPiece.lockPiece();
            } else
            {
                ActivePieces.currentPiece.movePieceR(new Vector3(0, -1));
            }
            timeSinceLastDrop = 0;
        }
    }
}