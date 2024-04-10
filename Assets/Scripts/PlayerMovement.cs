using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float distance = 1u;
    private HUD hudDisplay;
    private int movesLeft;

    private void Start() {
        
        movesLeft = 3;
        hudDisplay = FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
            if (movesLeft >= 1) {
                PlayerGridMovement();
            }

    }


    private void PlayerGridMovement() {

        if (Input.GetKeyDown(KeyCode.W)) {
            if (DetectTile(Vector3.forward)) {
                Movement(Vector3.forward);
                UpdateTotal();

            }
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (DetectTile(Vector3.left)) {
                Movement(Vector3.left);
                UpdateTotal();

            }
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            if (DetectTile(-Vector3.forward)) {
                Movement(-Vector3.forward);
                UpdateTotal();

            }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            if (DetectTile(Vector3.right)) {
                Movement(Vector3.right);
                UpdateTotal();

            }
        }


    }

    private void Movement(Vector3 direction) {

        transform.Translate(direction * distance);

    }

    private bool DetectTile(Vector3 direction) {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Tile")) {
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
                Debug.Log("Plus1");
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

    

}
