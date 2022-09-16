using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class VersionControll : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(CheckVersion(delegate(bool isVersion)
        {
            if(isVersion)
                Debug.Log("This version is current");
            else
                Debug.LogWarning("The version is not up to date, we advise you to update to the current one");
        }));
    }

    private IEnumerator CheckVersion(Action<bool> resualt = null)
    {
        UnityWebRequest request = UnityWebRequest.Get($"https://api.github.com/repos/{Application.companyName}/{Application.productName}/releases/latest");
        yield return request.SendWebRequest();
        if (!string.IsNullOrEmpty(request.error))
            throw new Exception(request.error);
        resualt?.Invoke(JsonUtility.FromJson<GitJson>(request.downloadHandler.text).tag_name == Application.version);
    }
}

// Github json
[Serializable]
public class GitJson
{
    public string tag_name;
}
