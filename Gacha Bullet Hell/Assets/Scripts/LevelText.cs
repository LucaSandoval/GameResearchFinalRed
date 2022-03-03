using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<Text>().text = "LEVEL " + SceneManage.level.ToString();
    }
}
