using System.Linq;
using UnityEngine;

public class WatermelonShowcase : ShowcaseAbstraction
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
            nonActiveItems[randomNumber1].gameObject.SetActive(true);
            nonActiveItems = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
            int randomNumber2 = Random.Range(0, nonActiveItems.Count);
            nonActiveItems[randomNumber2].gameObject.SetActive(true);
            EnabledItems4Buyers += 2;
            BackpackWasWorker();
            if (BuyerEnteredTrigger.Count == 0)
            {
                return;
            }
            else
            {
                GiveItems2Buyer(BuyerEnteredTrigger[0]);
                //GiveItems2Buyer(BuyerEnteredTrigger[0]);
            }

        }
    }
}
