using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connection Status: Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connection Status: Connected");

        Debug.Log("Room Status: Joining <DefaultRoom>");
        PhotonNetwork.JoinRoom("DefaultRoom");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Status: Failed Joining <DefaultRoom>");

        Debug.Log("Room Status: Creating <DefaultRoom>");
        PhotonNetwork.CreateRoom("DefaultRoom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Status: Created <DefaultRoom>");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("Room Status: Joined as Player {0}", PhotonNetwork.CurrentRoom.MasterClientId);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Room Status: Player {0} Joined", newPlayer.ActorNumber);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Room Status: Player {0} Left", otherPlayer.ActorNumber);
    }
}
