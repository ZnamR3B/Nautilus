using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestMenuManager : MonoBehaviour
{
    public QuestManager questManager;
    public MenuManager menuManager;

    public GameObject rewardPrefab;
    //UI reference
    public GameObject infoPanel;

    public Image categoryBackground;
    public Color[] categoryColor;
    public TextMeshProUGUI categoryText;

    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI objectiveProgessText;
    public TextMeshProUGUI rewardDescriptionText;

    public Image questType;

    public GameObject clearedIcon;

    public Transform rewardList;

    //info
    public Sprite[] questTypeIcon;
    public Sprite[] rewardIcon;
    public int pinnedQuest; //-1 if no any pinned quest

    public int currentQuestIndex;
    public int currentCategory; // 0 = main story, 1 = progressing side story, 2 = cleared side story

    public bool inMenu;    

    public void openMenu()
    {
        inMenu = true;
        if(pinnedQuest >= 0)
        {
            //there's pinned quest
            currentCategory = 1;
            currentQuestIndex = pinnedQuest;
        }
        else
        {
            currentCategory = 0;
            currentQuestIndex = 0;
        }
        showQuestInfo();
    }

    private void Update()
    {
        if(inMenu)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
            {
                currentQuestIndex--;
                if(currentQuestIndex < 0)
                {
                    currentQuestIndex = questManager.quests.Length - 1;
                }
                showQuestInfo();
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentQuestIndex++;
                if(currentQuestIndex > questManager.quests.Length - 1)
                {
                    currentQuestIndex = 0;
                }
                showQuestInfo();
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                currentCategory++;
                if (currentCategory > 2)
                    currentCategory = 0;
                categoryBackground.color = categoryColor[currentCategory];
                if(currentCategory == 2)
                {
                    clearedIcon.SetActive(true);
                }
                else
                {
                    clearedIcon.SetActive(false);
                }
                currentQuestIndex = 0;
                showQuestInfo();
            }
            if(Input.GetButtonDown("Cancel"))
            {
                closeMenu();
            }
        }
    }
    public void closeMenu()
    {
        inMenu = false;
        gameObject.SetActive(false);
        menuManager.openMenu();
    }
    public void showQuestInfo()
    {
        Quest quest;
        int originalIndex;
        bool noQuest = false;
        if (currentCategory == 0)
        {
            categoryText.text = "Main Story Quest";
            quest = questManager.mainQuest.GetComponent<Quest>();            
        }
        else if(currentCategory == 1)
        {
            categoryText.text = "Side Story Quest - In Progress";
            quest = questManager.quests[currentQuestIndex].GetComponent<Quest>();
            originalIndex = currentQuestIndex;
            while(quest.state != QuestState.unlocked)
            {
                currentQuestIndex++;
                if (currentQuestIndex >= questManager.quests.Length)
                {
                    currentQuestIndex = 0;
                }
                if (currentQuestIndex == originalIndex)
                {
                    noQuest = true;
                    break;
                }
                quest = questManager.quests[currentQuestIndex].GetComponent<Quest>();
            }
        }
        else
        {
            categoryText.text = "Side Story Quest - Cleared";
            quest = questManager.quests[currentQuestIndex].GetComponent<Quest>();
            originalIndex = currentQuestIndex;
            while (quest.state != QuestState.cleared)
            {
                currentQuestIndex++;
                if(currentQuestIndex >= questManager.quests.Length)
                {
                    currentQuestIndex = 0;
                }
                if (currentQuestIndex == originalIndex)
                {
                    noQuest = true;
                    break;
                }
                quest = questManager.quests[currentQuestIndex].GetComponent<Quest>();
            }
        }
        //if have available quest
        if(!noQuest)
        {
            infoPanel.SetActive(true);
            questNameText.text = quest.questName;
            descriptionText.text = quest.description;
            objectiveText.text = quest.objectiveDescription;
            objectiveProgessText.text = quest.objectiveProgress();
            rewardDescriptionText.text = quest.rewardDescription;
            questType.sprite = questTypeIcon[(int)quest.questType];
            //print reward
            foreach (Transform t in rewardList)
            {
                Destroy(t.gameObject);
            }
            //print money reward
            if (quest.questReward.money > 0)
            {
                GameObject rewardObj =
                    Instantiate(rewardPrefab, rewardList);
                rewardObj.transform.GetChild(0).GetComponent<Image>().sprite = rewardIcon[0];
                rewardObj.GetComponentInChildren<TextMeshProUGUI>().text = "Gold: " + quest.questReward.money;
            }
            //print item reward
            if (quest.questReward.rewardItem.Length > 0)
            {
                for (int i = 0; i < quest.questReward.rewardItem.Length; i++)
                {
                    GameObject rewardObj =
                        Instantiate(rewardPrefab, rewardList);
                    rewardObj.GetComponentInChildren<Image>().sprite = rewardIcon[1];
                    int temp = i + 1;
                    while (quest.questReward.rewardItem[temp] == quest.questReward.rewardItem[i])
                    {
                        temp++;
                    }
                    rewardObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.questReward.rewardItem[i].itemName + "\t\t" + (temp - i).ToString();
                    i = temp - 1;
                }
            }
            if (quest.questReward.rewardWeapon.Length > 0)
            {
                for (int i = 0; i < quest.questReward.rewardWeapon.Length; i++)
                {
                    GameObject rewardObj =
                        Instantiate(rewardPrefab, rewardList);
                    rewardObj.GetComponentInChildren<Image>().sprite = rewardIcon[(int)quest.questReward.rewardWeapon[i].type + 2];
                    rewardObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.questReward.rewardWeapon[i].weaponName + "\t\t" + 1;
                }
            }
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
}
