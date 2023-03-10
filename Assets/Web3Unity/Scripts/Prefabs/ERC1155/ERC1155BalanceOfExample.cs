using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "Ethereum";
        string network = "rinkeby";
        string contract = "0x495f947276749Ce646f68AC8c248420045cb7b5e";
        string account = "0xee6a1BD160F77e2C147B6d2aAe12FECf04BBc136";
        string tokenId = "24930137487246807284660534116032347285179868545871067192096120633491745407476";

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print(balanceOf);
    }
}
