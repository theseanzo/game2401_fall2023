using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInputField : MonoBehaviour
{
    #region Private variables
    static string playerNamePrefKey = "PlayerName";
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        string defaultName = "";
        TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField != null) //let's make sure we actually got an input field
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string value)
    {
        Debug.Log("We are saving our name as " + value);
        PhotonNetwork.NickName = value + ""; //the space was here to end the name
        PlayerPrefs.SetString(playerNamePrefKey, value);
        PlayerPrefs.Save();
    }
}
