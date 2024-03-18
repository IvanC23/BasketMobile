using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager instance;
    private List<Transform> _rewardsSlots;
    private List<bool> _currentlyActiveRewards;
    private RewardsConnector _rewardsConnector;
    private int _rewardsCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu" && instance == this)
        {
            _rewardsConnector = FindObjectOfType<RewardsConnector>();

            GameObject rewardsParent = GameObject.Find("Rewards");

            if (_rewardsConnector != null)
            {
                _rewardsSlots = _rewardsConnector.GetRewardsSlots();
                _rewardsCount = _rewardsSlots.Count;
            }
            else
            {
                Debug.LogError("Rewards connector not found");
            }

            if (_currentlyActiveRewards == null)
            {
                _currentlyActiveRewards = new List<bool>();
                for (int i = 0; i < _rewardsCount; i++)
                {
                    _currentlyActiveRewards.Add(false);
                }
            }
            else
            {
                for (int i = 0; i < _rewardsCount; i++)
                {
                    if (_currentlyActiveRewards[i])
                    {
                        _rewardsSlots[i].GetChild(0).gameObject.SetActive(false);
                        _rewardsSlots[i].GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void AddReward()
    {
        for (int i = 0; i < _rewardsCount; i++)
        {
            if (!_currentlyActiveRewards[i])
            {
                _currentlyActiveRewards[i] = true;
                break;
            }
        }
    }
    public void RemoveReward(int index)
    {
        _currentlyActiveRewards[index] = false;
        _rewardsSlots[index].GetChild(0).gameObject.SetActive(true);
        _rewardsSlots[index].GetChild(1).gameObject.SetActive(false);
    }



}