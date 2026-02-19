using UnityEngine;
#if !UNITY_WEBGL
using Firebase;
using Firebase.Analytics;
#endif
using System.Threading.Tasks;

public class FirebaseInit : MonoBehaviour
{
    private static bool initialized = false;

#if !UNITY_WEBGL
    async void Start()
    {
        if (initialized) return;

        var status = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (status == DependencyStatus.Available)
        {
            initialized = true;
            Debug.Log("Firebase Ready");
        }
        else
        {
            Debug.LogError("Firebase Error: " + status);
        }
    }

#endif
}
