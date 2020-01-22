using System;
using UnityEngine;

public class InteriorController : MonoBehaviour {
    private GameController gameController;
    private Raycaster raycaster;

    private bool isProcess;

    private Vector3 lastMousePos;

    private Transform currObjTransform;
    private Vector3 offset;

    private void Start() {
        gameController = GetComponent<GameController>();
        raycaster = GetComponent<Raycaster>();
        isProcess = false;
        lastMousePos = Vector3.positiveInfinity;
        offset = Vector3.positiveInfinity;
    }

    void Update() {
        if (!IsControlInterior()) return;
        
        if (Input.GetMouseButtonUp(0) && isProcess) {
            currObjTransform = null;
            isProcess = false;
            lastMousePos = new Vector3(-999,-999,-999);
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isProcess) {
            Vector3 clickPos = Input.mousePosition;
            GameObject obj = raycaster.GetObjectByClick(clickPos);
            if (obj == null) return;
            if (!obj.CompareTag("interior")) return;
            Vector3 clickWorldPos = raycaster.GetWorldPointByClick(clickPos);
            currObjTransform = obj.transform;
            offset = GetOffsetForMove(currObjTransform.position, clickWorldPos);
            isProcess = true;
            return;
        }
        if (isProcess) {
            if (gameController.mode == GameController.Mode.MoveInterior) {
                Move();
                return;
            }
            if (gameController.mode == GameController.Mode.RotateInterior) {
                Rotate();
            }
        }
    }

    private void Move() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 newPos = raycaster.GetGroundWorldPointByClick(mousePos);
        newPos = new Vector3(newPos.x + offset.x, currObjTransform.position.y, newPos.z + offset.z);
        currObjTransform.position = newPos;
    }

    private void Rotate() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 newPos = raycaster.GetGroundWorldPointByClick(mousePos);
        newPos = new Vector3(newPos.x, currObjTransform.position.y, newPos.z);
        currObjTransform.LookAt(newPos, Vector3.up);
        String objName = currObjTransform.name;
        if (objName == "Sofa" || objName == "Table") {
            Vector3 currRot = currObjTransform.localEulerAngles;
            if (objName == "Sofa") currObjTransform.localEulerAngles = new Vector3(currRot.x - 100, currRot.y, currRot.z);
            else currObjTransform.localEulerAngles = new Vector3(currRot.x - 90, currRot.y, currRot.z);
        }
    }

    private Vector3 GetOffsetForMove(Vector3 centerPos, Vector3 clickPos) {
        return centerPos - clickPos;
    }

    private bool IsControlInterior() {
        return gameController.mode == GameController.Mode.MoveInterior ||
               gameController.mode == GameController.Mode.RotateInterior;
    }
}
