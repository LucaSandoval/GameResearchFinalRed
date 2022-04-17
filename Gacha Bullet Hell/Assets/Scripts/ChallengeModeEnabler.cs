using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeModeEnabler : MonoBehaviour
{
    void Start()
    {
        if (SceneManage.level == 6)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
