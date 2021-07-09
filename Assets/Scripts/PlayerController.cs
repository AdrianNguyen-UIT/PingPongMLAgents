using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variable Fields
    //Configuration
    [SerializeField] private float moveSpeed = 10.0f;
    //Cached component references
    private float verticalMovement = 0.0f;
    private Rigidbody2D myRigidbody = null;
    private Vector2 inputMovement = Vector2.zero;
    //State
    #endregion

    #region Unity Methods
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(0.0f, verticalMovement * Time.fixedDeltaTime * 10.0f);
    }
    #endregion

    #region Private Methods
    #endregion

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    public void Move(float action)
    {
        verticalMovement = action * moveSpeed;
    }

    public float GetVerticalMovement()
    {
        return inputMovement.y;
    }
}
