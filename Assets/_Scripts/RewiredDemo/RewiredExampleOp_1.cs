using Rewired;
using UnityEngine;

namespace myd.input
{
    /// <summary>
    /// 第一种Input操作方式,直接获取某个按键的状态
    /// </summary>
    public class RewiredExampleOp_1 : MonoBehaviour
    {
        public int playerId;

        private Player player;

        void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);

            // Some more examples:

            // Get the System Player
            //player = ReInput.players.GetSystemPlayer();

            // Iterating through Players (excluding the System Player)
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                Player p = ReInput.players.Players[i];
                Debug.LogFormat("Load ReInput.players {0}: {1}-{2}", i, p.name, p.descriptiveName);
            }

            // Iterating through Players (including the System Player)
            for (int i = 0; i < ReInput.players.allPlayerCount; i++)
            {
                Player p = ReInput.players.AllPlayers[i];
                Debug.LogFormat("Load ReInput.AllPlayers {0}: {1}-{2}", i, p.name, p.descriptiveName);
            }
        }

        private void Update()
        {
            if (player.GetButtonDown("Fire"))
            {
                Debug.Log("Fire!");
            }
        }
    }
}
