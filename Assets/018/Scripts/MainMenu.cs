using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnLevel1ButtonPressed()
    {
        StartCoroutine(nameof(StartGame), "Level 1");
    }
    public void OnLevel2ButtonPressed()
    {
        StartCoroutine(nameof(StartGame), "Level 2");
    }
    public void OnExitToDesktopButtonPressed()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    private IEnumerator StartGame(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }
}
