using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput Instance {get; private set;}
    
        private InputSystem_Actions m_inputActions;
    
        public event EventHandler OnButtonClicked;

        private void Awake()
        {
            Instance = this;

            m_inputActions = new InputSystem_Actions();
        
            m_inputActions.Enable();

            m_inputActions.Player.Click.performed += PlayerClick_performed;
        }

        private void OnDestroy()
        {
            m_inputActions.Player.Click.performed -= PlayerClick_performed;
            m_inputActions.Disable();
        }

        private void PlayerClick_performed(InputAction.CallbackContext obj)
        {
            //Debug.Log("Player clicked");
            OnButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
