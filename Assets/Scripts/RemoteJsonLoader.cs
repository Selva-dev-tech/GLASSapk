using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace WorkflowSystem
{
    public class RemoteJsonLoader : IJsonLoader
    {
        private string apiUrl;

        public RemoteJsonLoader(string url)
        {
            apiUrl = url;
        }

        public string LoadJson()
        {
            // Start the coroutine and wait for it to complete
            string jsonResult = null;
            MonoBehaviour monoBehaviour = new GameObject("CoroutineRunner").AddComponent<MonoBehaviour>();
            monoBehaviour.StartCoroutine(LoadJsonFromAPIAsync((result) =>
            {
                jsonResult = result;
                Object.Destroy(monoBehaviour.gameObject); // Clean up the temporary GameObject
            }));

            // Wait until the coroutine sets the jsonResult
            while (jsonResult == null)
            {
                // Wait for the coroutine to finish
            }

            return jsonResult;
        }

        private IEnumerator LoadJsonFromAPIAsync(System.Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error loading JSON from API: " + webRequest.error);
                    callback(null); // Notify that loading failed
                }
                else
                {
                    callback(webRequest.downloadHandler.text); // Pass the JSON result
                }
            }
        }
    }
}
