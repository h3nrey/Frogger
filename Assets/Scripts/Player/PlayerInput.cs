using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {

    public void OnMoveUp(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerController.instance.onMove?.Invoke(new Vector2(0, 1));
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerController.instance.onMove?.Invoke(new Vector2(0, -1));
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerController.instance.onMove?.Invoke(new Vector2(-1, 0));
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerController.instance.onMove?.Invoke(new Vector2(1, 0));
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            print("jumped");
        }
    }
}