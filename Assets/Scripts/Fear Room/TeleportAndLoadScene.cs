using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportAndLoadScene : MonoBehaviour
{
    public int indexSceneLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadToScene();
        }
    }

    public void LoadToScene()
    {
        SceneManager.LoadScene(indexSceneLoad);
    }
}
