using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBorder : MonoBehaviour
{
    #region Variable Fields
    //Configuration
    [SerializeField] bool isLeftBorder = true;
    [SerializeField] private GameStatus gameStatus = null;
    //Cached component references
    //State
    #endregion

    #region Unity Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isLeftBorder)
        {
            gameStatus.Agent2Win();

        }
        else
        {
            gameStatus.Agent1Win();
        }
        gameStatus.EndAgents();
        gameStatus.ResetEnv();
    }
    #endregion

    #region Private Methods
    #endregion
}
