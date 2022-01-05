using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    bool m_bInit;
    Stage m_Stage;

    void Start()
    {
        InitStage();
    }

    private void InitStage()
    {
        if (m_bInit)
            return;

        m_bInit = true;
    }

}
