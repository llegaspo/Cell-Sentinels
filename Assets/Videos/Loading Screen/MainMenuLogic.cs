using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
   public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
   }
}
