
using System;
using Zenject;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int Money;
        public ShowcaseSave[] showcaseSave;
        public FabricSave[] fabricSave;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            Money = 60;
            openLevels[1] = true;
        }
        [Serializable] public class ShowcaseSave
        {
            public string ShowcaseName;
            public bool Buyed;
            public int ReservedSlots;
        }
        [Serializable] public class FabricSave
        {
            public string FabricName;
            public bool Buyed;
        }
    }
}
