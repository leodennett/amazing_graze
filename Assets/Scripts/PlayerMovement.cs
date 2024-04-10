using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float distance = 1u;
    private HUD hudDisplay;
    private PlayerRotation spinPlayer;
    private int movesLeft;
    private int stepsTaken;
    private bool winState;

    private void Start() {
        
        movesLeft = 3;
        stepsTaken = 0;
        hudDisplay = FindObjectOfType<HUD>();
        spinPlayer = FindObjectOfType<PlayerRotation>();
        DisableHUD();

    }

    // Update is called once per frame
    void Update()
    {
        if (winState == true) {
            GameWon();
            return;
        }

        if (movesLeft >= 1) {
            PlayerGridMovement(); 
        } else { 
            GameOver();
        }

    }


    private void PlayerGridMovement() {

        if (Input.GetKeyDown(KeyCode.W)) {
            if (DetectTile(Vector3.forward)) {
                Movement(Vector3.forward);
                UpdateTotal();
                stepsTaken++;

            }
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (DetectTile(Vector3.left)) {
                Movement(Vector3.left);
                UpdateTotal();
                stepsTaken++;

            }
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            if (DetectTile(-Vector3.forward)) {
                Movement(-Vector3.forward);
                UpdateTotal();
                stepsTaken++;

            }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            if (DetectTile(Vector3.right)) {
                Movement(Vector3.right);
                UpdateTotal(); 
                stepsTaken++;

            }
        }


    }

    private void Movement(Vector3 direction) {

        transform.Translate(direction * distance);
        spinPlayer.PlayerMeshRotation();

    }

    private bool DetectTile(Vector3 direction) {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Tile")) {
                return true;
            } else if (hit.collider.CompareTag("WinTile")) {
                winState = true;
                return true;
            }
        }

        return false;
       
    }

    private void UpdateTotal() {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Grass+1")) {
                UpdateTotalStepsPlus();
                Destroy(collider.gameObject);
            } else {
                UpdateTotalStepsMinus();
                Debug.Log("Minus1");
            }
        }

    }

    private void UpdateTotalStepsMinus() {
        movesLeft--;
        hudDisplay.totalStepsLeft.SetText("Steps Left: " + movesLeft.ToString());

    }

    private void UpdateTotalStepsPlus() {
        movesLeft++;
        hudDisplay.totalStepsLeft.SetText("Steps Left: " + movesLeft.ToString());

    }

    private void DisableHUD() {
        hudDisplay.gameOverText.gameObject.SetActive(false);
        hudDisplay.lostResetButton.gameObject.SetActive(false);
        hudDisplay.gameWonText.gameObject.SetActive(false);
        hudDisplay.wonNextButton.gameObject.SetActive(false);
        hudDisplay.wonResetButton.gameObject.SetActive(false);
    }

    private void GameOver() {
        hudDisplay.gameOverText.gameObject.SetActive(true);
        hudDisplay.lostResetButton.gameObject.SetActive(true);
    }

    private void GameWon() {
        hudDisplay.gameWonText.gameObject.SetActive(true);
        hudDisplay.wonNextButton.gameObject.SetActive(true);
        hudDisplay.wonResetButton.gameObject.SetActive(true);
        hudDisplay.stepsTakenText.SetText("Total Steps Taken: " + stepsTaken.ToString());
    }

}
