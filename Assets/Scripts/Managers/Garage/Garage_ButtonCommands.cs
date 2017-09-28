using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Garage_ButtonCommands : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BuyRegularPack()
    {
        Inventory.Instance.BuyPack(Inventory.PackType.REGULAR);
    }

    public void BuyPremiumPack()
    {
        Inventory.Instance.BuyPack(Inventory.PackType.PREMIUM);
    }

    public void BuySuperPack()
    {
        Inventory.Instance.BuyPack(Inventory.PackType.SUPER);
    }
}
