using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounding : MonoBehaviour
{
    #region Variable Fields
    //Configuration
    [SerializeField] private Ball ball = null;
    //Cached component references

    //State
    #endregion

    #region Unity Methods
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == ball)
        {
            ball.ResetBall();
        }
    }
    #endregion

    #region Private Methods
    #endregion
}
