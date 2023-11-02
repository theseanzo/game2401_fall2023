using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Serializable Values
    [SerializeField] 
    GameObject connectPanel;
    [SerializeField]
    GameObject actionPanel;
    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    GameObject attackRoomButton;
    #endregion
    #region Private Variables
    string _gameVersion = "1";
    string ourRoom = "classRoom";
    byte maxPlayers = 20;
    Dictionary<string, GameObject> roomButtons = new Dictionary<string, GameObject>();
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        //Our awake happens when the script is loaded
        PhotonNetwork.AutomaticallySyncScene = true; //if we have any levels in the network, we want to sync them automatically
    }
    void Start()
    {

        connectPanel.SetActive(true);
    }
    public void Connect()
    {
        //this is going to handle our connection in the Pun network
        if (PhotonNetwork.NickName == "")
            return;
        connectPanel.SetActive(false);
        //two situations: if we are connected or are we aren't connected already
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.JoinOrCreateRoom(ourRoom, new RoomOptions() { MaxPlayers = maxPlayers, PublishUserId = true }, null);
        }
        else
        {
            
            PhotonNetwork.ConnectUsingSettings();
            
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }
    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.NickName, new RoomOptions() { MaxPlayers = maxPlayers, PublishUserId = true }, null);
    }

    public void AttackLobby()
    {
        lobbyPanel.SetActive(true);
        actionPanel.SetActive(false);

    }
    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("We are now connected to the master server and can join a room with id {0} and Nickname {1}", PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.NickName);
        actionPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("We have disconnected from photon due to " + cause);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene"); //photon is going to handle loading our levels
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int count = 0;
        foreach(RoomInfo room in roomList)
        {
            Debug.LogFormat("Room id {0} and name {1}", count++, room.Name);
        }
        ClearRoomListButtons();
        UpdateRoomListButtons(roomList);
    }
    #endregion
    #region Private functions
    private void ClearRoomListButtons()
    {
        foreach(GameObject entry in roomButtons.Values)
        {
            Destroy(entry.gameObject);
        }
        roomButtons.Clear();
    }
    private void UpdateRoomListButtons(List<RoomInfo> roomList)
    {
        foreach(RoomInfo room in roomList)
        {
            GameObject button = Instantiate(attackRoomButton, attackRoomButton.transform.parent);
            button.GetComponentInChildren<TMP_Text>().text = room.Name;
            button.SetActive(true);
            roomButtons.Add(room.Name, button);
        }
        attackRoomButton.SetActive(false);
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
