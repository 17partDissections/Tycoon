using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class WatermelonShowcase : ShowcaseAbstraction<Watermelon>
{
    protected override void BackpackWasWorker()
    {
        var nonActiveItemsBeforeAdding = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
        if (nonActiveItemsBeforeAdding.Count() == 0)
        {
            return;
        }
        var trying2FindItem = BackpackAbstraction.RemoveItem(Item.ItemName);
        if (trying2FindItem != null)
        {
            var nonActiveItems = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
            int randomNumber1 = Random.Range(0, nonActiveItems.Count);
            int randomNumber2 = Random.Range(0, nonActiveItems.Count);
            EnabledItems4Buyers+=2;
            nonActiveItems[randomNumber1].gameObject.SetActive(true);
            nonActiveItems[randomNumber2].gameObject.SetActive(true);
            BackpackWasWorker();
            if (BuyerEnteredTrigger.Count == 0)
            {
                return;
            }
            else
            {
                GiveItems2Buyer(BuyerEnteredTrigger[0]);
                GiveItems2Buyer(BuyerEnteredTrigger[0]);
            }

        }
    }
}
