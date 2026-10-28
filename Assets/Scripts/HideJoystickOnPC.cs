using UnityEngine;

public class HideJoystickOnPC : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_ANDROID || UNITY_IOS
            gameObject.SetActive(true);
            return;
        #endif

        #if UNITY_WEBGL && !UNITY_EDITOR
            if (Application.isMobilePlatform)
            {
                gameObject.SetActive(true); // Mobil tarayıcıysa (telefon/tablet) GÖSTER
            }
            else
            {
                gameObject.SetActive(false); // PC tarayıcısıysa GİZLE
            }
            return;
        #endif

        gameObject.SetActive(false); 
    }
}