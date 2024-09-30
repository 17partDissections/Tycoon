using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ProFabricAbstraction : FabricAbstraction
{
    private List<Item> _itemCopies = new List<Item>();
    [SerializeField] private ItemName _material;
    [SerializeField] private List<Item> _materials;
    private int _materialsCount;
    public int MaterialsCount { get => _materialsCount; set { if (_materialsCount + value < 0) _materialsCount = 0; else _materialsCount = value; } }
    [SerializeField] private List<Item> _baked;
    [SerializeField] private Collider _materialTrigger;
    [SerializeField] private Collider _bakedTrigger;


    protected override void OnTriggerEnter(Collider other)
    {
        if (Buyed)
        {
            other.TryGetComponent<Backpack>(out Backpack backpack);
            if (_materialTrigger.bounds.Intersects(other.bounds))
            {
                if (backpack is BackpackWorker)
                {
                    List<Item> foundMaterials = backpack.Items.FindAll(item => item.ItemName == _material);
                    List<Item> materialsInInventory = _materials.FindAll(item => !item.isActiveAndEnabled);

                    Debug.Log(foundMaterials.Count + "Count");
                    if (HashCoroutine == null)
                        HashCoroutine = StartCoroutine(GrowCoroutine());

                    if (true)
                    {
                        var cycle = 0;
                        foreach (Item foundMaterial in foundMaterials)
                        {
                            //int randomNumber = Random.Range(0, materialsInInventory.Count);
                            materialsInInventory[/*randomNumber*/cycle].gameObject.SetActive(true);
                            backpack.RemoveItem(foundMaterial.ItemName);


                            MaterialsCount++;
                            cycle++;
                        }


                    }
                }
            }
            else if (_bakedTrigger.bounds.Intersects(other.bounds))
            {
                Debug.Log("intersects");
                if (CanGrab)
                {
                    Debug.Log("canGrab");
                    Debug.Log(_itemCopies.Count);
                    while (_itemCopies.Count != 0)
                    {
                        if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                        {
                            Debug.Log("Trying2SveItem");

                            AudioHandler.PlaySFX(PlantRemoving);
                            backpack?.SaveItem(_itemCopies[0]);
                            _baked.Find(predict => predict.gameObject.activeSelf == true).gameObject.SetActive(false);
                            _itemCopies.RemoveAt(0);
                            CanGrab = false;
                            Canvas.Text.text = "";
                            GrowSignal?.Invoke(Timer += 2); //disabling last phase visual object
                            MaterialsCount--;
                            if (HashCoroutine == null)
                            {
                                HashCoroutine = StartCoroutine(GrowCoroutine());
                            }
                        }
                        else
                        {
                            Debug.Log("Backpack is full");
                            break;
                        }
                    }
                }
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (Buyed)
        {
            //if (CanGrab)
            //{
            //    other.TryGetComponent<Backpack>(out Backpack backpack);
            //    if (backpack is BackpackWorker && !backpack.IsBackpackFull())
            //    {
            //        AudioHandler.PlaySFX(PlantRemoving);
            //        backpack?.SaveItem(ItemCopy);
            //        CanGrab = false;
            //        Canvas.Text.text = "";
            //        GrowSignal.Invoke(Timer += 2); //disabling last phase visual object
            //        if (HashCoroutine == null)
            //            HashCoroutine = StartCoroutine(GrowCoroutine());
            //    }
            //}
        }
        else
        {
            BuyingProcess();
        }
    }
    protected override IEnumerator GrowCoroutine()
    {

        Timer = 0;
        var foundAllBacked = _baked.FindAll(item => !item.isActiveAndEnabled);
        yield return new WaitWhile(() => MaterialsCount == 0);
        yield return new WaitWhile(() => foundAllBacked.Count == 0);
        yield return new WaitForSeconds(0.5f);
        List<Item> materialsInInventory = _materials.FindAll(item => item.isActiveAndEnabled);
        //int randomNumber = Random.Range(0, materialsInInventory.Count);
        yield return new WaitWhile(() => materialsInInventory.Count == 0);
        materialsInInventory[/*randomNumber*/0].gameObject.SetActive(false);
        MaterialsCount--;
        var materialAnimationObject = gameObject.GetComponentsInChildren<Transform>().First(x => x.name == "MaterialAnimationObject");
        var bakedAnimationObject = gameObject.GetComponentsInChildren<Transform>().First(x => x.name == "BakedAnimationObject");
        materialAnimationObject.GetComponent<MeshRenderer>().enabled = true;
        var savedMaterialAObjTransform = materialAnimationObject.transform.position;
        bakedAnimationObject.GetComponent<MeshRenderer>().enabled = true;
        var savedBakedAObjTransform = bakedAnimationObject.transform.position;
        materialAnimationObject.DOMove(materialAnimationObject.transform.position + Vector3.forward, 5).SetEase(Ease.Linear)
            .OnComplete(() => bakedAnimationObject.DOMove(bakedAnimationObject.transform.position + Vector3.right * 2.5f, 10).SetEase(Ease.Linear));
        while (Timer < GrowSpeed)
        {
            yield return new WaitForSeconds(1);
            Timer++;
            GrowSignal?.Invoke(Timer);
            Canvas.Text.text = (Timer + "/" + GrowSpeed).ToString();

        }
        var found = _baked.First(item => !item.isActiveAndEnabled);
        GetBackAnimationObjects2DefaultPositions(materialAnimationObject, bakedAnimationObject, savedMaterialAObjTransform, savedBakedAObjTransform);
        Canvas.Text.text = "done";
        //var itemCopyOfGameObject = Instantiate(found[_itemCopies.Count], found[_itemCopies.Count].transform.position, Quaternion.Euler(0, 0, 0));
        if (false != found)
        {
            found.gameObject.SetActive(true);
            _itemCopies.Add(Instantiate(found));
        }

        CanGrab = true;
        if (MaterialsCount > 0)
            HashCoroutine = StartCoroutine(GrowCoroutine());
        else
            HashCoroutine = null;
    }

    private void GetBackAnimationObjects2DefaultPositions(Transform materialAnimationObject, Transform bakedAnimationObject, Vector3 savedMaterialAObjTransform, Vector3 savedBakedAObjTransform)
    {
        materialAnimationObject.GetComponent<MeshRenderer>().enabled = false;
        materialAnimationObject.transform.position = savedMaterialAObjTransform;
        bakedAnimationObject.GetComponent<MeshRenderer>().enabled = false;
        bakedAnimationObject.transform.position = savedBakedAObjTransform;
    }
}

