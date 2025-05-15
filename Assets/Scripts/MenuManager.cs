using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Appelée depuis le bouton "Lancer AR"
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Appelée depuis le bouton "Quitter"
    public void QuitApplication()
    {
        Application.Quit();
    }
}
