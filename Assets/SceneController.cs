using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Main,
    Lobby,
    Match
}

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private Scenes lobbyScene;
    [SerializeField]
    private Scenes matchScene;

    private void Awake()
        => LoadLobbyScene();

    public void LoadMatchScene()
        => SceneManager.LoadSceneAsync((int)matchScene, LoadSceneMode.Additive);

    public void LoadLobbyScene()
        => SceneManager.LoadSceneAsync(((int)lobbyScene), LoadSceneMode.Additive);
}
