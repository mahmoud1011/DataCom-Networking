using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log ("I connected to a server");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        if (conn.identity == null)
        {
            Debug.LogError("Player object is null on connection!");
            return;
        }
        
        if (!conn.identity.TryGetComponent<MyNetworkPlayer>(out var player))
        {
            Debug.LogError("MyNetworkPlayer component not found on player object!");
            return;
        }
        
        player.SetDisplayName ($"Player {numPlayers}");

        Color displayColor = new(Random.Range(0f, 1f), Random.Range(0f, 1f),
        Random.Range(0f, 1f));

        player.SetDisplayColor(displayColor);
    }

}
