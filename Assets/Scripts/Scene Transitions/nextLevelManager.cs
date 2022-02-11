using UnityEngine.SceneManagement;
using UnityEngine;

public class nextLevelManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
