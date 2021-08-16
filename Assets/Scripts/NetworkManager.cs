using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;


    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInput;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;


    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;


    public string GameMode;
    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(LoginUIPanel.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to internet!!");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " connected to photon!!");
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created!!");    
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            object gameModeName;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gm", out gameModeName))
            {
                Debug.Log(gameModeName.ToString());
            }
        }
    }
    #endregion






    #region Public Methods

    public void ActivatePanel(string panelName)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelName));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelName));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelName));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelName));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelName));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelName));
    }

    public void SetGameMode( string _gameMode)
    {
        GameMode = _gameMode;
    }

    #endregion







    #region UI Callbacks



    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }



    public void OnLoginButonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel.name);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        else
        {
            Debug.Log("player name is invalid");
        }


    }



    public void OnCreateRoomButtonClicked()
    {
        if (GameMode != null)
        {
            string room = RoomNameInput.text;

            if (string.IsNullOrEmpty(RoomNameInput.text))
            {
                room = "Room" + Random.Range(0, 1000);
            }
            RoomOptions roomOptions = new RoomOptions();

            roomOptions.MaxPlayers = 4;

            string[] roomPropsInLobby = { "gm" }; //gm = game mode

            //racing = "rc"
            //death race = "dr"

            ExitGames.Client.Photon.Hashtable customRoomProps = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProps;

            PhotonNetwork.CreateRoom(room, roomOptions);
        }
        
    }
    #endregion


}
