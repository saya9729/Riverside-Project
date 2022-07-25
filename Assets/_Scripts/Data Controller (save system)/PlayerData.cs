using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float sol;
    //public float energy;
    public float[] position;

    public PlayerData(Player.PlayerStatisticManager p_playerStatisticManager)
    {
        health = p_playerStatisticManager.GetHealth();
        sol = p_playerStatisticManager.GetSol();
        //energy = p_playerStatisticManager.GetEnergy();

        position = new float[3];
        position[0] = p_playerStatisticManager.transform.position.x;
        position[1] = p_playerStatisticManager.transform.position.y;
        position[2] = p_playerStatisticManager.transform.position.z;
    }
}
