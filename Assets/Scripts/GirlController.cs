using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlController : MonoBehaviour {
    public GameController gameController;
    public Raycaster raycaster;
    private Animator _animator;

    public SkinnedMeshRenderer bodyMeshRenderer;
    public SkinnedMeshRenderer topMeshRenderer;
    
    public float speed = 0.02f;
    private Transform _objTransformForSit;
    private Vector3 _posWhereToGo;
    private bool _walk;
    private bool _wantToSit;
    private bool _sit;

    private enum AnimationState {
        Walk, Up, Gesticulate, Seat, Rotate 
    }

    private readonly Dictionary<AnimationState, String> _animationStates = new Dictionary<AnimationState, string>{
        {AnimationState.Walk, "Walk"},
        {AnimationState.Seat, "Seat"},
        {AnimationState.Up, "Up"},
        {AnimationState.Gesticulate, "Gesticulate"},
        {AnimationState.Rotate, "Rotate"}
    };

    void Start() {
        _animator = GetComponent<Animator>();
        _walk = false;
        _wantToSit = false;
    }
    
    void Update()
    {
        AnimatorClipInfo info = _animator.GetCurrentAnimatorClipInfo(0)[0];
        if (info.clip.name == "Northern Soul Spin" || info.clip.name == "Standing Greeting") {
            return;
        }
        if (!_sit && _walk) {
            WalkToPoint();
        }
        
        if (_sit && _walk) {
            _animator.SetBool("StandUp", true);
            //AnimatorClipInfo info = _animator.GetCurrentAnimatorClipInfo(0)[0];
            if (info.clip.name == "Stand Up" || info.clip.name == "Sitting Idle") return;
            _animator.SetBool("StandUp", false);
            _sit = false;
            return;
        }

        if (gameController.mode != GameController.Mode.ControlGirl &&
            gameController.mode != GameController.Mode.Record) return;
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
            Vector3 clickPos = Input.mousePosition;
            GameObject clickObject = raycaster.GetObjectByClick(clickPos);
            if (clickObject != null) {
                if (clickObject.CompareTag("ground")) {
                    _posWhereToGo = raycaster.GetGroundWorldPointByClick(clickPos);
                    _walk = true;
                } else if (clickObject.name == "Chair" || clickObject.name == "Sofa") {
                    _posWhereToGo = clickObject.name == "Sofa"
                        ? GetPosInFrontBadObj(clickObject.transform, 0.6f)
                        : GetPosInFrontObj(clickObject.transform, 0.3f);
                    _objTransformForSit = clickObject.transform;
                    _walk = true;
                    _wantToSit = true;
                }
            }
        }
    }

    private void WalkToPoint() {
        if (_walk) {
            _animator.SetBool(_animationStates[AnimationState.Walk], true);
            transform.LookAt(_posWhereToGo);
            var girlPos = transform.position;
            transform.position = Vector3.MoveTowards(girlPos, _posWhereToGo, speed);
            if (HasCome()) {
                _walk = false;
                _animator.SetBool(_animationStates[AnimationState.Walk], false);
                if (_wantToSit) {
                    if (_objTransformForSit.name == "Sofa") SitDownForBadObject();
                    else SitDown();
                }
            }
        }
    }

    private void SitDown() {
        Vector3 posInFrontObj = GetPosInFrontObj(_objTransformForSit, 1f);
        transform.LookAt(posInFrontObj);
        _animator.SetTrigger("Sit");
        _sit = true;
        _wantToSit = false;
    }
    
    private void SitDownForBadObject() {
        Vector3 posInFrontObj = GetPosInFrontBadObj(_objTransformForSit, 1.2f);
        transform.LookAt(posInFrontObj);
        Vector3 rot = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rot.x + 10, rot.y, rot.z);
        _animator.SetTrigger("Sit");
        _sit = true;
        _wantToSit = false;
    }

    private bool HasCome() {
        Vector3 girlPos = transform.position;
        return Vector3.Distance(girlPos, _posWhereToGo) < 0.001f;
    }

    private Vector3 GetPosInFrontObj(Transform objTransform, float distance) {
        Vector3 objPos = objTransform.position;
        objPos = new Vector3(objPos.x, 0, objPos.z);
        return objPos + objTransform.forward * distance;
    }
    
    private Vector3 GetPosInFrontBadObj(Transform objTransform, float distance) {
        Vector3 objPos = objTransform.position;
        objPos = new Vector3(objPos.x, 0, objPos.z);
        return objPos + (-objTransform.up) * distance;
    }

    public void Gesticulate() {
        _animator.SetTrigger(_animationStates[AnimationState.Gesticulate]);
    }
    
    public void Whirl() {
        _animator.SetTrigger(_animationStates[AnimationState.Rotate]);
    }

    public void ChangeColor(Color color) {
        switch (gameController.mode) {
            case GameController.Mode.ChangeSkinColor:
                bodyMeshRenderer.material.color = color;
                break;
            case GameController.Mode.ChangeClothColor:
                topMeshRenderer.material.color = color;
                break;
        }
    }

    public void InitColor() {
        switch (gameController.mode) {
            case GameController.Mode.ChangeSkinColor:
                gameController.GetComponent<UIController>().SetColorToColorPicker(bodyMeshRenderer.material.color);
                break;
            case GameController.Mode.ChangeClothColor:
                gameController.GetComponent<UIController>().SetColorToColorPicker(topMeshRenderer.material.color);
                break;
            
        }
    }
}
