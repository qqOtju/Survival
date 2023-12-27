using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project_Assets.Scripts.InputControl
{
    public class InputHandlerController: MonoBehaviour
    {
        private Controls _controls;
        private int _count;
        
        public event Action<Vector2> OnMove;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _count = PlayerPrefs.GetInt("ScreenshotsCount");
            _controls = new Controls();
            // _controls.System.Screenshot.performed += TakeScreenshot;
            _controls.Player.Movement.performed += OnMovePerformed;
            _controls.Player.Movement.canceled += OnMovePerformed;
        }

        private void OnDestroy()
        {
            // _controls.System.Screenshot.performed -= TakeScreenshot;
            _controls.Player.Movement.performed -= OnMovePerformed;
            _controls.Player.Movement.canceled -= OnMovePerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            var movement = obj.ReadValue<Vector2>();
            OnMove?.Invoke(movement);
        }

        private void TakeScreen(InputAction.CallbackContext obj)
        {
            _count++;
            ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
            PlayerPrefs.SetInt("ScreenshotsCount", _count);
            Debug.Log("A screenshot was taken!");
        }
        
        private void OnEnable() => _controls.Enable();
        
        private void OnDisable() => _controls.Disable();
    }
}