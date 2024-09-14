using UnityEngine.UI;
using TMPro;


public class FabricsNShowcasesCanvas
{
    private Image _itemIcon;
    public TextMeshProUGUI Text;
    private int _price;
    public Image BuyCircle;

    public FabricsNShowcasesCanvas(Image itemIcon, TextMeshProUGUI text, int price, Image buyCircle)
    {
         _itemIcon = itemIcon;
         Text = text;
        _price = price;
        BuyCircle = buyCircle;
        SetPrice();

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
