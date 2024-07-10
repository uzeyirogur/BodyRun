using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void returnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
