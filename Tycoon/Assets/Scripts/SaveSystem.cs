using System.Collections.Generic;
using System.Linq;
using YG;

public class SaveSystem
{
    public void SaveShowcase(ShowcaseAbstraction showcase)
    {
       if(YandexGame.savesData.showcaseSave != null)
        {
            var s = YandexGame.savesData.showcaseSave.FirstOrDefault(x => x.ShowcaseName == showcase.name);
            if ( s != null)
            {
                s.Buyed = showcase.Buyed;
                s.ReservedSlots = showcase.EnabledItems4Buyers;
            }
            else
            {
                s = new SavesYG.ShowcaseSave();
                s.ShowcaseName = showcase.name;
                s.Buyed = showcase.Buyed;
                s.ReservedSlots = showcase.EnabledItems4Buyers;
                List<SavesYG.ShowcaseSave> newReservedSlots = new List<SavesYG.ShowcaseSave>();
                newReservedSlots.AddRange(YandexGame.savesData.showcaseSave);
                newReservedSlots.Add(s);
                YandexGame.savesData.showcaseSave = newReservedSlots.ToArray();
            }

        }
       else
        {
            YandexGame.savesData.showcaseSave = new SavesYG.ShowcaseSave[1];
            YandexGame.savesData.showcaseSave[0] = new SavesYG.ShowcaseSave();
            YandexGame.savesData.showcaseSave[0].ShowcaseName = showcase.name;
            YandexGame.savesData.showcaseSave[0].Buyed = showcase.Buyed;
            YandexGame.savesData.showcaseSave[0].ReservedSlots = showcase.EnabledItems4Buyers;
        }
    }
    public void SaveFabric(FabricAbstraction fabric)
    {
        if (YandexGame.savesData.fabricSave != null)
        {
            var f = YandexGame.savesData.fabricSave.FirstOrDefault(x => x.FabricName == fabric.name);
            if (f != null)
            {
               f.Buyed = fabric.Buyed;
                f.FabricName = fabric.name;
            }
            else
            {
                f = new SavesYG.FabricSave();
                f.FabricName = fabric.name;
                f.Buyed = fabric.Buyed;
                List<SavesYG.FabricSave> newReservedSlots = new List<SavesYG.FabricSave>();
                newReservedSlots.AddRange(YandexGame.savesData.fabricSave);
                newReservedSlots.Add(f);
                YandexGame.savesData.fabricSave = newReservedSlots.ToArray();
            }

        }
        else
        {
            YandexGame.savesData.fabricSave = new SavesYG.FabricSave[1];
            YandexGame.savesData.fabricSave[0] = new SavesYG.FabricSave();
            YandexGame.savesData.fabricSave[0].FabricName = fabric.name;
            YandexGame.savesData.fabricSave[0].Buyed = fabric.Buyed;
        }
    }
}
