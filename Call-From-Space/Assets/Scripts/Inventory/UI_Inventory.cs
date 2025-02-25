using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public Transform journalSlotContainer;
    public Transform journalSlotTemplate;
    public bool onItemScreen = true;

    

    private void Start()
    {
        if (itemSlotContainer == null)
            itemSlotContainer = transform.Find("itemSlotContainer");
        if (itemSlotTemplate == null)
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        if (journalSlotContainer == null)
            journalSlotContainer = transform.Find("journalSlotContainer");
        if (journalSlotTemplate == null)
            journalSlotTemplate = journalSlotContainer.Find("journalSlotTemplate");
        //itemSlotContainer = transform.Find("itemSlotContainer");
        //itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        //journalSlotContainer = transform.Find("journalSlotContainer");
        //journalSlotTemplate = journalSlotContainer.Find("journalSlotTemplate");
        

    }
    
 
    public void setInventory(Inventory inventory )
    {
        this.inventory = inventory;
        //RefreshInventoryItems();
        //RefreshInventoryJournals();
    }

    public void refresh()
    {
        Debug.Log("REFRESH");
        Debug.Log("refreshing");
        if(onItemScreen)
        {
            Debug.Log("refreshing items: change audio here");
            AudioManager.Instance.PlaySound("Menu-Open");
            RefreshInventoryItems();
        }
        else
        {
            Debug.Log("refreshing journal: change audio here");
            AudioManager.Instance.PlaySound("Menu-Open");
            RefreshInventoryJournals();
        }
    }

    public void is_ontItemScreen(){
        onItemScreen = true;
    }
    public void not_ontItemScreen(){
        onItemScreen = false;
    }

    public void RefreshInventoryItems()
    {
        foreach( Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach( Transform child in journalSlotContainer)
        {
            if (child == journalSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int xpos = 0;
        int ypos = 0;
        float itemSlotCellSize = 150f;
        foreach (Item item in inventory.GetItemList())
        {
            
            
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            Vector2 additionalPosition = new Vector2(xpos*itemSlotCellSize, ypos*itemSlotCellSize);
            itemSlotRectTransform.anchoredPosition = additionalPosition;
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.itemImage;


            Item_interaction itemInter = itemSlotRectTransform.GetComponent<Item_interaction>();
            itemInter.item = item;

            xpos += 1;
            if(xpos > 3)
            {
                ypos -= 1;
                xpos = 0;
            }

        }
    }

    public void RefreshInventoryJournals()
    {
        Debug.Log("ERROR");
        foreach( Transform child in journalSlotContainer)
        {
            if (child == journalSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach( Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int xpos = 0;
        int ypos = 0;
        float itemSlotCellSize = 150f;

        foreach (Item item in inventory.GetJournalList())
        {
            

            RectTransform itemSlotRectTransform = Instantiate(journalSlotTemplate, journalSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            Vector2 additionalPosition = new Vector2(xpos*itemSlotCellSize, ypos*itemSlotCellSize);
            itemSlotRectTransform.anchoredPosition = additionalPosition;

            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.itemImage;

            Item_interaction itemInter = itemSlotRectTransform.GetComponent<Item_interaction>();
            itemInter.item = item;

            




            xpos += 1;
            if(xpos > 3)
            {
                ypos -= 1;
                xpos = 0;
            }

            
        }
    }

  
}
