using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static void LoadSceneAsnyc(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
