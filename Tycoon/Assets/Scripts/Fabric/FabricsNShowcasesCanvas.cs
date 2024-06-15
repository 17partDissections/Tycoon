using UnityEngine.UI;
using TMPro;


public class FabricsNShowcasesCanvas
{
    private FabricAbstraction _fabricAbstraction;
    private Image _itemIcon;
    public TextMeshProUGUI Text;
    private int _price;

    public FabricsNShowcasesCanvas(Image itemIcon, TextMeshProUGUI text, int price)
    {
         _itemIcon = itemIcon;
         Text = text;
        _price = price;
    }

    public void SetPrice()
    {
        _itemIcon.enabled = false;
        Text.text = (_price + "$").ToString();
    }
    public void OnlyIcon()
    {
        _itemIcon.enabled = true;
        Text.text = null;
    }
    public void None()
    {
        _itemIcon.enabled = false;
        Text.text = null;
    }
}
