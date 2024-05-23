using UnityEngine;
using UnityEngine.SceneManagement;

namespace BYNetwork.Poi
{
    public class GameplayManager : MonoBehaviour
    {
        public void BackToMenu()
        {
            _ = gameObject.AddComponent<NetworkManager>().GameRoom.Leave(true);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public void Exit()
        {
            _ = gameObject.AddComponent<NetworkManager>().GameRoom.Leave(true);
            Application.Quit();
        }
    }
}
