using UnityEngine;

[RequireComponent(typeof(Player.PlayerLoseSequence))]
[RequireComponent(typeof(Player.PlayerWinSequence))]
public class GameFlowManager : MonoBehaviour
{
    private Player.PlayerLoseSequence _playerLoseSequence;
    private Player.PlayerWinSequence _playerWinSequence;

    void Start()
    {
        _playerLoseSequence = GetComponent<Player.PlayerLoseSequence>();
        _playerWinSequence = GetComponent<Player.PlayerWinSequence>();

        this.RegisterListener(EventID.onLose, (param) => Lose());
        this.RegisterListener(EventID.onWin, (param) => Win());
    }
    public void Lose()
    {
        _playerLoseSequence.PlayPlayerLoseSequence();
    }

    public void Win()
    {
        _playerWinSequence.PlayPlayerWinSequence();
    }
}