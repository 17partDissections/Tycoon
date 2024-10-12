using System.Collections.Generic;
using System.Linq;
using YG;
using UnityEngine;
public class LoadSystem
{
    public void LoadShowcase(ShowcaseAbstraction showcase)
    {
        if (YandexGame.savesData.showcaseSave != null)
        {
            var s = YandexGame.savesData.showcaseSave.FirstOrDefault(x => x.ShowcaseName == showcase.name);
            if (s != null)
            {
                showcase.gameObject.SetActive(true);
                if (s.Buyed)
                {

                    showcase.BuyShowcaseTroughLoader();
                }


                for (int i = 0; i < s.ReservedSlots; i++)
                {
                    showcase.LoadItems();
                }
            }
        }
    }
    public void LoadFabric(Transform fabricTransform)
    {
        if (YandexGame.savesData.fabricSave != null)
        {
            var fabric = fabricTransform.GetComponentInChildren<FabricAbstraction>();
            var f = YandexGame.savesData.fabricSave.FirstOrDefault(x => x.FabricName == fabric.name);
            if (f != null)
            {
                fabricTransform.gameObject.SetActive(true);
                if (f.Buyed)
                {
                    fabric.BuyFabricThroughLoad();
                }
            }
        }
    }
} 
