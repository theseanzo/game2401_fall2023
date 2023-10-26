using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Serializable Values
    [SerializeField] 
    GameObject controlPanel;
    [SerializeField]
    GameObject progressLabel;

    #endregion
    #region Private Variables
    string _gameVersion = "1";
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        //Our awake happens when the script is loaded
        PhotonNetwork.AutomaticallySyncScene = true; //if we have any levels in the network, we want to sync them automatically
    }
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    public void Connect()
    {
        //this is going to handle our connection in the Pun network
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        //two situations: if we are connected or are we aren't connected already
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();  
        }
        else
        {
            
            PhotonNetwork.ConnectUsingSettings();
            
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }
    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the master server and can join a room");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("We have disconnected from photon due to " + cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("We got error code of " + message);
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene"); //photon is going to handle loading our levels
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
