using UnityEngine;
using UnityEngine.InputSystem;

public class DragMovement : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _difference;
    private Vector3 _newPosition;
    private Camera _mainCamera;
    private bool _isDragging = false;

    private void Awake() {
        _mainCamera = Camera.main;
    }

    public void OnDrag(InputAction.CallbackContext context) {
        if (context.started) _origin = GetMousePosition;
        _isDragging = context.started || context.performed;

    }

    private void LateUpdate() {
        if (_isDragging) {
            _difference = GetMousePosition - transform.position;
            _newPosition = _origin - _difference;
            _newPosition.x = Mathf.Clamp(_newPosition.x, -100, 100);
            _newPosition.y = Mathf.Clamp(_newPosition.y, -60, 60);
            _newPosition.z = Mathf.Clamp(_newPosition.z, -100, 100);
            transform.position = _newPosition;
        }    
    }
    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());

}
