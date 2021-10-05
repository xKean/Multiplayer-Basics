using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleDisplayNameUpdated))] [SerializeField] private string displayName = "Missing Name";
    [SyncVar(hook = nameof(HandleDisplayColorUpdated))] [SerializeField] private Color playerColor;

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName) // Server internal
    {
        displayName = newDisplayName;
    }
    [Server]
    public void SetPlayerColor(Color newplayerColor)
    {
        playerColor = newplayerColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName) // Client calls this on server
    {

        if (newDisplayName.Length <= 3 || newDisplayName.Length >= 10 || newDisplayName.Contains(" ")) { return; } // Server Authotity 

        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName);
    }

    #endregion
    #region client

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        displayNameText.text = newDisplayName;
    }


    [ContextMenu("SetMyName")]
    private void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName) // Call Server calls this to all Clients
    {
        Debug.Log(newDisplayName);
    }


    #endregion
}
