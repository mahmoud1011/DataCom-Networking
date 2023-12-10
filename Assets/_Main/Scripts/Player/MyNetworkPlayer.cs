using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [Header("Components")]
    [HorizontalLine]
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [Header("SyncVars")]
    [HorizontalLine]
    [SyncVar (hook = nameof(OnDisplayNameUpdate))]
    [SerializeField]
    private string playerName = "Missing Name";
    [SyncVar(hook = nameof(OnDisplayColourUpdate))]
    [SerializeField]
    private Color displayColor = Color.black;

    [Header("HP Settings")]
    [HorizontalLine]
    [SyncVar(hook = nameof(OnHealthChanged))]
    public int currentHealth = 100;

    #region server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        playerName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName (string newDisplayName)
    {
        //server authority to limit playerName into 2-20 letter length
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            return;
        }
        RpcDisplayNewName(newDisplayName);
        SetDisplayName (newDisplayName);

    }
    #endregion

    #region client
    private void OnHealthChanged(int oldValue, int newValue)
    {
        // Handle UI updates or other actions when health changes (only on clients)
        // Example: Update health UI, show damage effects, etc.
        Debug.Log($"Old HP: {oldValue} = New HP: {newValue}");
    }

    private void OnDisplayColourUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_MainColor", newColor);
    }

    private void OnDisplayNameUpdate (string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu ("Set This Name")]
    private void SetThisName()
    {
        CmdSetDisplayName("This is a new name");
    }

    [ClientRpc]
    private void RpcDisplayNewName (string newDisplayName)
    {
        Debug.Log (newDisplayName);
    }
    #endregion
}
