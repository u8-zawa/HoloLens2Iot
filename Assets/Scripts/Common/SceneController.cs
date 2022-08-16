using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("TransitionToScene:" + sceneName);
    }
}
