using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ProFabricAbstraction : FabricAbstraction
{
    [SerializeField] private ItemName _material;
    [SerializeField] private List<Item> _materials;
    private int _materialsCount;


    protected override void OnTriggerEnter(Collider other)
    {
        if (Buyed)
        {
            other.TryGetComponent<Backpack>(out Backpack backpack);
            if (CanGrab)
            {
                if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                {
                    AudioHandler.PlaySFX(PlantRemoving);
                    backpack?.SaveItem(ItemCopy);
                    CanGrab = false;
                    Canvas.Text.text = "";
                    GrowSignal.Invoke(Timer += 2); //disabling last phase visual object
                    _materialsCount--;
                    if (HashCoroutine == null)
                    {
                        HashCoroutine = StartCoroutine(GrowCoroutine());
                    }

                }
                else
                {
                    Debug.Log("Backpack is full");
                }
            }
            if (backpack is BackpackWorker)
            {
                List<Item> foundMaterials = backpack.Items.FindAll(item => item.ItemName == _material);
                List<Item> materialsInInventory = _materials.FindAll(item => !item.isActiveAndEnabled);
                if (materialsInInventory.Count == 0)
                {
                    return;
                }
                else
                {
                    var cycle = 0;
                    foreach (Item foundMaterial in foundMaterials)
                    {
                        //int randomNumber = Random.Range(0, materialsInInventory.Count);
                        materialsInInventory[/*randomNumber*/cycle].gameObject.SetActive(true);
                        backpack.RemoveItem(foundMaterial.ItemName);
                        _materialsCount++;
                        cycle++;
                    }


                }
            }
        }
        else
        {
            BuyFabric();
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (CanGrab)
        {
            other.TryGetComponent<Backpack>(out Backpack backpack);
            if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                _materialsCount--;
        }
        base.OnTriggerStay(other);
    }
    protected override IEnumerator GrowCoroutine()
    {
        Timer = 0;
        yield return new WaitWhile(()=> _materialsCount == 0);
        yield return new WaitForSeconds(0.5f);
        List<Item> materialsInInventory = _materials.FindAll(item => item.isActiveAndEnabled);
        //int randomNumber = Random.Range(0, materialsInInventory.Count);
        materialsInInventory[/*randomNumber*/0].gameObject.SetActive(false);
        _materialsCount--;
        
        while (Timer < GrowSpeed)
        {
            yield return new WaitForSeconds(1);
            Timer++;
            GrowSignal.Invoke(Timer);
            Canvas.Text.text = (Timer + "/" + GrowSpeed).ToString();

        }
        Canvas.Text.text = "done";
        var itemCopyOfGameObject = Instantiate(VisualTemplate, gameObject.transform);
        itemCopyOfGameObject.TryGetComponent(out Item itemCopy);
        if (itemCopy != null)
        {
            ItemCopy = itemCopy;
        }
        CanGrab = true;
        if (_materialsCount > 0)
            HashCoroutine = StartCoroutine(GrowCoroutine());
        else
            HashCoroutine = null;
    }
}
