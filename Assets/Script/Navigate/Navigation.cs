using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void Body()
    {
        SceneManager.LoadSceneAsync(2);
    }
   public void Retry(){
        SceneManager.LoadSceneAsync(3)  ;
   }
}
