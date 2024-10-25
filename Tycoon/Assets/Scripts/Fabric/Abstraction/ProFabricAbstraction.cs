using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

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
    private List<Item> _foundAllBaked;
    [SerializeField] private int _materialAnimationObjTime;
    [SerializeField] private int _bakedAnimationObjTime;


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

                    if (HashCoroutine == null)
                    {
                        HashCoroutine = StartCoroutine(GrowCoroutine());
                    }


                    if (true)
                    {
                        var cycle = 0;
                        foreach (Item foundMaterial in foundMaterials)
                        {
                            if (cycle == 4) break;
                            materialsInInventory[cycle].gameObject.SetActive(true);
                            backpack.RemoveItem(foundMaterial.ItemName);
                            MaterialsCount++;
                            cycle++;
                        }


                    }
                }
            }
            else if (_bakedTrigger.bounds.Intersects(other.bounds))
            {
                if (CanGrab)
                {
                    while (_itemCopies.Count != 0)
                    {
                        if (backpack is BackpackWorker && !backpack.IsBackpackFull())
                        {

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
                    _foundAllBaked = _baked.FindAll(item => !item.isActiveAndEnabled);
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

    protected override void BuyFabric()
    {
        base.BuyFabric();
        _materialTrigger.enabled = true;
        _bakedTrigger.enabled = true;
    }
    public override void BuyFabricThroughLoad()
    {
        base.BuyFabricThroughLoad();
        _materialTrigger.enabled = true;
        _bakedTrigger.enabled = true;
    }
    protected override IEnumerator GrowCoroutine()
    {

        Timer = 0;
        _foundAllBaked = _baked.FindAll(item => !item.isActiveAndEnabled);
        Debug.Log("material count: " + MaterialsCount);
        Debug.Log("foundallbaked: " + _foundAllBaked.Count);
        yield return new WaitWhile(() => MaterialsCount == 0);
        Debug.Log("12312331");
        yield return new WaitWhile(() => _foundAllBaked.Count == 0);
        Debug.Log("Start Of Coroutine");
        yield return new WaitForSeconds(0.5f);
        List<Item> materialsInInventory = _materials.FindAll(item => item.isActiveAndEnabled);
        //int randomNumber = Random.Range(0, materialsInInventory.Count);
        yield return new WaitWhile(() => materialsInInventory.Count == 0);
        materialsInInventory[/*randomNumber*/0].gameObject.SetActive(false);
        MaterialsCount--;
        var materialAnimationObject = gameObject.GetComponentsInChildren<Transform>().First(x => x.name == "MaterialAnimationObject");
        var bakedAnimationObject = gameObject.GetComponentsInChildren<Transform>().First(x => x.name == "BakedAnimationObject");
        materialAnimationObject.TryGetComponent<MeshRenderer>(out MeshRenderer mMeshRenderer);
        mMeshRenderer.enabled = true;
        var savedMaterialAObjTransform = materialAnimationObject.transform.position;
        bakedAnimationObject.TryGetComponent<MeshRenderer>(out MeshRenderer bMeshRenderer);
        if (bMeshRenderer != null) bMeshRenderer.enabled = true;
        else
            bakedAnimationObject.transform.GetChild(0).gameObject.SetActive(true);
        Tween tween = null;
        var savedBakedAObjTransform = bakedAnimationObject.transform.position;
        materialAnimationObject.DOMove(materialAnimationObject.transform.position + Vector3.forward, _materialAnimationObjTime).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                tween = bakedAnimationObject.DOMove(bakedAnimationObject.transform.position + Vector3.right * 2.5f, _bakedAnimationObjTime).SetEase(Ease.Linear);
            }
        );
        while (Timer < GrowSpeed)
        {
            yield return new WaitForSeconds(1);
            Timer++;
            GrowSignal?.Invoke(Timer);
            Canvas.Text.text = (Timer + "/" + GrowSpeed).ToString();
        }
        yield return tween.WaitForCompletion();
        materialAnimationObject.GetComponent<MeshRenderer>().enabled = false;
        materialAnimationObject.transform.position = savedMaterialAObjTransform;
        if (bMeshRenderer != null) bMeshRenderer.enabled = false;
        else
            bakedAnimationObject.transform.GetChild(0).gameObject.SetActive(false);
        bakedAnimationObject.transform.position = savedBakedAObjTransform;
        var found = _baked.FirstOrDefault(item => !item.isActiveAndEnabled);
        GetBackAnimationObjects2DefaultPositions(materialAnimationObject, bakedAnimationObject, savedMaterialAObjTransform, savedBakedAObjTransform);
        if (YandexGame.lang == "en")
            Canvas.Text.text = "done";
        else if (YandexGame.lang == "ru")
            Canvas.Text.text = "готово";
        if (found != null)
        {
            found.gameObject.SetActive(true);
            switch (this.Item.ItemName)
            {
                case ItemName.Strawberry:
                    UseObj(_itemHandler.StrawberryPool); break;
                case ItemName.Banana:
                    UseObj(_itemHandler.BananaPool); break;
                case ItemName.Lemon:
                    UseObj(_itemHandler.LemonPool); break;
                case ItemName.StrawberryJam:
                    UseObj(_itemHandler.StrawberryJamPool); break;
                case ItemName.Lemonade:
                    UseObj(_itemHandler.LemonadePool); break;
                case ItemName.Watermelon:
                    UseObj(_itemHandler.WatermelonPool); break;
            }
        }

        CanGrab = true;
        if (MaterialsCount > 0)
            HashCoroutine = StartCoroutine(GrowCoroutine());
        else
        {
            Debug.Log("END OF COROUTINE");
            HashCoroutine = null;
        }

    }
    protected override void UseObj<T>(ObjectPool<T> pool)
    {
        var item = pool.GetFromPool();
        item.TryGetComponent(out Item itemClass);
        if (itemClass != null)
        {
            ItemObj = itemClass;
            _itemCopies.Add(item);
        }
    }

    private void GetBackAnimationObjects2DefaultPositions(Transform materialAnimationObject, Transform bakedAnimationObject, Vector3 savedMaterialAObjTransform, Vector3 savedBakedAObjTransform)
    {
        materialAnimationObject.GetComponent<MeshRenderer>().enabled = false;
        materialAnimationObject.transform.position = savedMaterialAObjTransform;
        bakedAnimationObject.TryGetComponent<MeshRenderer>(out MeshRenderer bMeshRenderer);
        if (bMeshRenderer != null)
            bMeshRenderer.enabled = false;
        else
            bakedAnimationObject.transform.GetChild(0).gameObject.SetActive(false);
        bakedAnimationObject.transform.position = savedBakedAObjTransform;
    }
}

