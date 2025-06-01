using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimationLogic : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
