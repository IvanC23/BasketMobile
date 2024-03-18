using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsConnector : MonoBehaviour
{
    [SerializeField] private List<Transform> _rewardsSlots;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _rewardsSlotsPanel;
    [SerializeField] private GameObject _rewardPopUpPanel;



    private RewardsManager _rewardsManager;

    void Start()
    {
        _rewardsManager = FindObjectOfType<RewardsManager>();
        if (_rewardsManager == null)
        {
            Debug.LogError("RewardsManager not found");
        }
    }

    public void OnClickReward(int index)
    {
        _buttonsPanel.SetActive(false);
        _rewardsSlotsPanel.SetActive(false);
        _rewardPopUpPanel.SetActive(true);

        _rewardsManager.RemoveReward(index);
    }

    public List<Transform> GetRewardsSlots()
    {
        return _rewardsSlots;
    }

    public void CloseRewardPanel()
    {
        _buttonsPanel.SetActive(true);
        _rewardsSlotsPanel.SetActive(true);
        _rewardPopUpPanel.SetActive(false);
    }
}
