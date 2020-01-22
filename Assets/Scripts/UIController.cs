using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private GameController _gameController;
    private ReplayController _replayController;
    public GirlController girlController;
    public Color activeColor;

    public GameObject replayScrollView;
    public GameObject replayList;
    public GameObject replayItem;

    public ColorPicker colorPicker;

    public Button moveButton;
    public Button rotateButton;
    public Button changeColorButton;
    public Button changeClothColorBtn;
    public Button changeSkinColorBtn;
    public Button recordButton;
    public Button playButton;
    public Button whirlButton;
    public Button gesticulateButton;

    private bool _isMoveBtnActive;
    private bool _isRotateBtnActive;
    private bool _isChangeColorBtnActive;
    private bool _isChangeClothColorBtnActive;
    private bool _isChangeSkinColorBtnActive;
    private bool _isRecordBtnActive;
    private bool _isPlayButtonActive;

    void Start() {
        _gameController = GetComponent<GameController>();
        _replayController = GetComponent<ReplayController>();
        _isMoveBtnActive = false;
        _isRotateBtnActive = false;
        _isRecordBtnActive = false;
        _isPlayButtonActive = false;
        _isChangeColorBtnActive = false;
        _isChangeClothColorBtnActive = false;
        _isChangeSkinColorBtnActive = false;
    }

    public void ClickMoveButton() {
        if (_isMoveBtnActive) {
            _isMoveBtnActive = false;
            DeactivateButton(moveButton);
            _gameController.mode = GameController.Mode.ControlGirl;
            return;
        }

        _gameController.mode = GameController.Mode.MoveInterior;
        _isMoveBtnActive = true;
        _isRotateBtnActive = false;
        _isChangeColorBtnActive = false;
        ActivateButton(moveButton);
        DeactivateButton(rotateButton);
        DeactivateButton(changeColorButton);
    }
    public void ClickRotateButton() {
        if (_isRotateBtnActive) {
            _isRotateBtnActive = false;
            DeactivateButton(rotateButton);
            _gameController.mode = GameController.Mode.ControlGirl;
            return;
        }
        _gameController.mode = GameController.Mode.RotateInterior;
        _isRotateBtnActive = true;
        _isMoveBtnActive = false;
        _isChangeColorBtnActive = false;
        ActivateButton(rotateButton);
        DeactivateButton(moveButton);
        DeactivateButton(changeColorButton);
    }
    public void ClickChangeColorButton() {
        if (_isChangeColorBtnActive) {
            _isChangeColorBtnActive = false;
            _isChangeClothColorBtnActive = false;
            _isChangeSkinColorBtnActive = false;
            DeactivateButton(changeColorButton);
            DeactivateButton(changeClothColorBtn);
            DeactivateButton(changeSkinColorBtn);
            ShowButton(moveButton);
            ShowButton(rotateButton);
            ShowButton(whirlButton);
            ShowButton(gesticulateButton);
            ShowButton(recordButton);
            ShowButton(playButton);
            HideButton(changeClothColorBtn);
            HideButton(changeSkinColorBtn);
            colorPicker.gameObject.SetActive(false);
            _gameController.mode = GameController.Mode.ControlGirl;
            return;
        }
        _gameController.mode = GameController.Mode.ChangeClothColor;
        _isChangeColorBtnActive = true;
        _isChangeClothColorBtnActive = true;
        _isMoveBtnActive = false;
        _isRotateBtnActive = false;
        HideButton(moveButton);
        HideButton(rotateButton);
        HideButton(whirlButton);
        HideButton(gesticulateButton);
        HideButton(recordButton);
        HideButton(playButton);
        ActivateButton(changeColorButton);
        ActivateButton(changeClothColorBtn);
        ShowButton(changeClothColorBtn);
        ShowButton(changeSkinColorBtn);
        colorPicker.gameObject.SetActive(true);
        girlController.InitColor();
    }
    
    public void ClickChangeSkinColorButton() {
        if (_isChangeSkinColorBtnActive) {
            return;
        }
        _gameController.mode = GameController.Mode.ChangeSkinColor;
        _isChangeClothColorBtnActive = false;
        _isChangeSkinColorBtnActive = true;
        ActivateButton(changeSkinColorBtn);
        DeactivateButton(changeClothColorBtn);
        girlController.InitColor();
    }
    
    public void ClickChangeClothColorButton() {
        if (_isChangeClothColorBtnActive) {
            return;
        }
        _gameController.mode = GameController.Mode.ChangeClothColor;
        _isChangeSkinColorBtnActive = false;
        _isChangeClothColorBtnActive = true;
        ActivateButton(changeClothColorBtn);
        DeactivateButton(changeSkinColorBtn);
        girlController.InitColor();
    }

    public void ClickPlayReplay() {
        if (_isPlayButtonActive) {
            _gameController.mode = GameController.Mode.ControlGirl;
            _isPlayButtonActive = false;
            ShowButton(moveButton);
            ShowButton(rotateButton);
            ShowButton(whirlButton);
            ShowButton(gesticulateButton);
            ShowButton(recordButton);
            ShowButton(changeColorButton);
            replayScrollView.SetActive(false);
            DeactivateButton(playButton);
            return;
        }
        _gameController.mode = GameController.Mode.Play;
        _isPlayButtonActive = true;
        HideButton(moveButton);
        HideButton(rotateButton);
        HideButton(whirlButton);
        HideButton(gesticulateButton);
        HideButton(recordButton);
        HideButton(changeColorButton);
        ActivateButton(playButton);
        
        replayScrollView.SetActive(true);

        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        Debug.Log(dir.FullName);
        foreach (var file in dir.GetFiles()) {
            if (!file.Name.EndsWith(".replay")) continue;
            GameObject newReplayItem = Instantiate(replayItem, replayList.transform);
            Text replayItemText = newReplayItem.transform.GetComponentInChildren<Text>();
            replayItemText.text = file.Name;
            Button newReplayItemButton = newReplayItem.GetComponent<Button>();
            newReplayItemButton.onClick.AddListener(() => {
                _replayController.StartPlay(file.FullName);
                replayScrollView.SetActive(false);
            });
        }

        _gameController.mode = GameController.Mode.Play;
    }

    public void ClickRecordReplay() {
        if (_isRecordBtnActive) {
            _isRecordBtnActive = false;
            ShowButton(changeColorButton);
            ShowButton(playButton);
            _gameController.mode = GameController.Mode.ControlGirl;

            string replayName = DateTime.Now.Millisecond + ".replay";
            _replayController.StopRecord(Application.persistentDataPath + "/" + replayName);
            DeactivateButton(recordButton);
            return;
        }

        _isRecordBtnActive = true;
        HideButton(changeColorButton);
        HideButton(playButton);
        ActivateButton(recordButton);
        
        _replayController.StartRecord();

        _gameController.mode = GameController.Mode.Record;
    }

    private void ActivateButton(Button btn) {
        btn.gameObject.GetComponent<Image>().color = activeColor;
    }

    private void DeactivateButton(Button btn) {
        btn.gameObject.GetComponent<Image>().color = Color.white;
    }

    private void HideButton(Button btn) {
        DeactivateButton(btn);
        btn.gameObject.SetActive(false);
    }
    
    private void ShowButton(Button btn) {
        btn.gameObject.SetActive(true);
    }

    public void SetColorToColorPicker(Color color) {
        colorPicker.CurrentColor = color;
    }
}
