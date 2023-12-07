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
        //Debug.Log ("Player added to Server");
        //Debug.Log ($"There are now {numPlayers} players");
        
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.setDisplayName ($"Player {numPlayers}");

        Color displayColor = new Color (Random.Range (0f,1f), Random.Range(0f,1f), 
        Random.Range(0f,1f));

        player.setDisplayColor (displayColor);
    }

}
