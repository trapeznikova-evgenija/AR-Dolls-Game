using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour {
    public Text debugText;
    public Text debugText2;

    public Ray GetRay(Vector3 clickPos) {
        debugText.text = clickPos.ToString();
        return Camera.main.ScreenPointToRay(clickPos);
    }

    public GameObject GetObjectByClick(Vector3 clickPos) {
        if (IsIntersectWithUi(clickPos)) return null;
        
        Ray ray = GetRay(clickPos);
        if (Physics.Raycast(ray, out var hit)) {
            Transform objectHit = hit.transform;
            debugText2.text = objectHit.gameObject.tag + "\n" + hit.point;
            return objectHit.gameObject;
        }
        return null;
    }

    public Vector3 GetGroundWorldPointByClick(Vector3 clickPos) {
        if (IsIntersectWithUi(clickPos)) return Vector3.zero;
        
        Ray ray = GetRay(clickPos);
        if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Ground"))) {
            return hit.point;
        }
        return Vector3.zero;
    }
    
    public Vector3 GetWorldPointByClick(Vector3 clickPos) {
        if (IsIntersectWithUi(clickPos)) return Vector3.zero;
        
        Ray ray = GetRay(clickPos);
        if (Physics.Raycast(ray, out var hit)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    private bool IsIntersectWithUi(Vector3 clickPos) {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = clickPos;
        List<RaycastResult> res = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, res);
        if (res.Count != 0) return true;
        return false;
    }
}
