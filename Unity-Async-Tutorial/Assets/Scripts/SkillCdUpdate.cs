using UnityEngine;

public class SkillCdUpdate : MonoBehaviour
{
    public float cdTime = 3f; // 冷却时间
    private float _currentCd = 0f; // 当前剩余CD
    private bool _isReady = true; // 技能是否可用

    void Update()
    {
        if (!_isReady)
        {
            _currentCd -= Time.deltaTime;
            if (_currentCd <= 0)
            {
                _isReady = true;
                Debug.Log("技能已就绪！");
            }
        }

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
            _currentCd = cdTime;
            Debug.Log("技能释放！");
        }
        else if (!_isReady)
        {
            Debug.Log("技能冷却中...");
        }
    }
}