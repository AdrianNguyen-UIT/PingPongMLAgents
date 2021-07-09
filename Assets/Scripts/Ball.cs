using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Ball : MonoBehaviour
{
    #region Variable Fields
    //Configuration
    [SerializeField] private float speed = 1.0f;
    [Range(0.1f, 1.0f)] [SerializeField] float randomFactor = 0.1f;
    [SerializeField] private bool autoStart = false;
    [SerializeField] private bool randomReset = false;
    [SerializeField] private GameStatus gameStatus = null;
    //Cached component references
    private Rigidbody2D myRigidbody = null;
    //State
    private bool hasStarted = false;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        gameStatus.TurnOnAutoStart(autoStart);
    }

    private void Start()
    {
        ResetBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SideBorder>() == null)
        {
            AddTweakVelocity();
            StabilizeVelocity();
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator AutoStartDelay()
    {
        gameStatus.StartTimer();
        yield return new WaitForSeconds(gameStatus.delayLaunchTime);
        Launch();
    }

    private void Launch()
    {
        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        direction.Normalize();
        myRigidbody.velocity = direction * speed;
        hasStarted = true;
    }

    private void AddTweakVelocity()
    {
        Vector2 velocityTweak = new Vector2(Random.Range(-randomFactor, randomFactor), Random.Range(-randomFactor, randomFactor));
        myRigidbody.velocity += velocityTweak;
    }

    private void StabilizeVelocity()
    {
        Vector2 currentVelocity = myRigidbody.velocity;
        currentVelocity.Normalize();
        myRigidbody.velocity = currentVelocity * speed;
    }

    private void AutoStart()
    {
        if (autoStart)
        {
            StartCoroutine(AutoStartDelay());
        }
    }
    #endregion

    public void OnLaunch(InputAction.CallbackContext value)
    {
        if (!hasStarted && value.started)
        {
            Launch();
        }
    }

    public void ResetBall()
    {
        if (randomReset)
        {
            transform.localPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-4f, 4f));
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        myRigidbody.velocity = Vector2.zero;
        hasStarted = false;
        AutoStart();
    }
}