using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public string startSceneName = string.Empty;

    // Start is called before the first frame update
    //void Start()
    //{
    //    StartCoroutine(LoadSceneSetActive(startSceneName));
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �V�[���Ɉړ�����
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private IEnumerator Transition(string sceneName, Vector3 targetPosition)
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return LoadSceneSetActive(sceneName);
    }

    /// <summary>
    /// �V�[���Z�b�g�̃��[�h���A�N�e�B�u�ɂ���
    /// </summary>
    /// <param name="sceneName">�V�[���̖��O</param>
    /// <returns></returns>
    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newScene);
    }
}
