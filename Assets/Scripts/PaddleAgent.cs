using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using UnityEngine.InputSystem;

public class PaddleAgent : Agent
{
    #region Variable Fields
    //Configuration
    [SerializeField] private Ball ball = null;
    [SerializeField] private float hitReward = 1.0f;
    [SerializeField] private float winReward = 1.0f;
    [SerializeField] private float losePenalty = -1.0f;
    [SerializeField] private float internalClockForBallReset = 15.0f;
    [SerializeField] private bool randomReset = false;
    //Cached component references
    private Rigidbody2D ballRb = null;
    private Rigidbody2D agentRB = null;
    private PlayerController playerController = null;
    private float currentInternalClock = 0.0f;
    private bool awayTooLongFromBall = false;
    //State
    #endregion

    #region Unity Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ball == collision.gameObject.GetComponent<Ball>())
        {
            AddReward(hitReward);
            ResetInternalClock();
        }
    }

    private void Update()
    {
        if (!awayTooLongFromBall)
        {
            currentInternalClock -= Time.deltaTime;
            if (currentInternalClock <= 0.0f)
            {
                awayTooLongFromBall = true;
            }
        }
    }

    #endregion

    #region Private Methods


    #endregion

    public override void Initialize()
    {
        ballRb = ball.GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        agentRB = GetComponent<Rigidbody2D>();
        ResetAgent();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(agentRB.velocity.y);

        sensor.AddObservation(ball.transform.localPosition.x);
        sensor.AddObservation(ball.transform.localPosition.y);
        sensor.AddObservation(ballRb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveY = actions.DiscreteActions[0];
        playerController.Move(moveY - 1.0f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)playerController.GetVerticalMovement() + 1;
    }

    public void ReceiveLossPenalty()
    {
        AddReward(losePenalty);
    }

    public void ReceiveWinReward()
    {
        AddReward(winReward);
    }

    public void ResetAgent()
    {
        if (randomReset)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, Random.Range(-3.6f, 3.6f));
        }
        else
        {
            transform.localPosition = new Vector2(transform.localPosition.x, 0.0f);
        }
        agentRB.velocity = Vector2.zero;
        ResetInternalClock();
    }

    public void ResetInternalClock()
    {
        awayTooLongFromBall = false;
        currentInternalClock = internalClockForBallReset;
    }

    public bool IsAwayTooLongFromBall()
    {
        return awayTooLongFromBall;
    }
}