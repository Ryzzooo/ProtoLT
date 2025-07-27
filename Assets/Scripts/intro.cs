using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] float introDuration = 3f; // durasi intro dalam detik

    private void Start()
    {
        Invoke(nameof(LoadRumahScene), introDuration);
    }

    private void LoadRumahScene()
    {
        musicmanager.Instance.PlayMusic("game");
        SceneManager.LoadScene("Scene Rumah");
    }
    public void SkipIntroAndPlay()
    {
        musicmanager.Instance.PlayMusic("game");
        SceneManager.LoadScene("Scene Rumah");
    }
}