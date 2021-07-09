using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameStatus : MonoBehaviour
{
    #region Variable Fields
    //Configuration
    [SerializeField] private TextMeshProUGUI player1ScoreText = null;
    [SerializeField] private TextMeshProUGUI player2ScoreText = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private GameObject ballLauncher = null;
    [SerializeField] private PaddleAgent agent1 = null;
    [SerializeField] private PaddleAgent agent2 = null;
    [SerializeField] private Ball ball = null;
    public float delayLaunchTime = 0.5f;
    //Cached component references
    private int player1Score = 0;
    private int player2Score = 0;
    private float remainTime = 0.0f;
    //State
    private bool startTimer = false;
    #endregion

    #region Unity Methods

    private void Start()
    {
        remainTime = delayLaunchTime;
        if (player1ScoreText != null)
            player1ScoreText.text = player1Score.ToString();
        if (player2ScoreText != null)
            player2ScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        if (startTimer)
        {
            remainTime -= Time.deltaTime;
            if (remainTime <= 0.0f)
            {
                remainTime = 0.0f;
                startTimer = false;
            }
            if (timerText != null)
                timerText.text = remainTime.ToString("0.00");
        }

        if (agent1.IsAwayTooLongFromBall() && agent2.IsAwayTooLongFromBall())
        {
            ResetEnv();
        }
    }

    #endregion

    #region Private Methods

    #endregion

    public void StartTimer()
    {
        remainTime = delayLaunchTime;
        startTimer = true;
    }

    public void TurnOnAutoStart(bool autoStart)
    {
        if (autoStart)
        {
            if (timerText != null)
                timerText.enabled = true;
            if (ballLauncher != null)
                ballLauncher.SetActive(false);
        }
        else
        {
            if (timerText != null)
                timerText.enabled = false;
            if (ballLauncher != null)
                ballLauncher.SetActive(true);
        }
    }

    public void EndAgents()
    {
        agent1.EndEpisode();
        agent2.EndEpisode();
    }

    public void Agent1Win()
    {
        ++player1Score;
        agent1.ReceiveWinReward();
        agent2.ReceiveLossPenalty();
        if (player1ScoreText != null)
            player1ScoreText.text = player1Score.ToString();
    }

    public void Agent2Win()
    {
        ++player2Score;
        agent2.ReceiveWinReward();
        agent1.ReceiveLossPenalty();
        if (player2ScoreText != null)
            player2ScoreText.text = player2Score.ToString();
    }

    public void ResetEnv()
    {
        ball.ResetBall();
        agent1.ResetAgent();
        agent2.ResetAgent();
    }
}