using System;
using System.Collections;
using UnityEngine;

public class SkillCdCoroutine : MonoBehaviour
{
    public float cdTime = 3f;
    private bool _isReady = true;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CastSkill();
        }
    }

    private void CastSkill()
    {
        if (_isReady)
        {
            _isReady = false;
            Debug.Log("技能释放！");
            StartCoroutine(StartCd());
        }
        else if (!_isReady)
        {
            Debug.Log("技能冷却中...");
        }
    }

    private IEnumerator StartCd()
    {
        yield return new WaitForSeconds(cdTime);
        _isReady = true;
        Debug.Log("技能已就绪！");
    }
}