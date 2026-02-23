using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderSceneGame : MonoBehaviour
{
   




    public void LoadGameSceneMain(int indexload)
    {
        SceneManager.LoadScene(indexload);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) LoadGameSceneMain(0);
    }
}
