using UnityEngine;

public class LoadBankInAwake : MonoBehaviour
{
    public AK.Wwise.Bank BankToLoad;

    private void Awake()
    {
        BankToLoad.Load(false);
    }
}