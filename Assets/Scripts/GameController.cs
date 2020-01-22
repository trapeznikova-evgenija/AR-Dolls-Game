using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Mode {
        Menu,
        ControlGirl, 
        MoveInterior, RotateInterior,
        ChangeClothColor, ChangeSkinColor,
        Record, Play
    }

    public Mode mode;

    private void Start() {
        mode = Mode.ControlGirl;
    }

    public void SetMode(Mode mode) {
        this.mode = mode;
    }

    public void SetMoveInteriorMode() {
        mode = Mode.MoveInterior;
    }
    
    public void SetRotateInteriorMode() {
        mode = Mode.RotateInterior;
    }
    
    public void SetControlGirlMode() {
        mode = Mode.ControlGirl;
    }

    public void Exit() {
        Application.Quit();
    }
}
