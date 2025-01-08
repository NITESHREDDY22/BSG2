using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsStatusUpdate : MonoBehaviour {


	void OnEnable()
    {
        _coinsText.text = GameManager.GetCoins().ToString();
      
        GameManager.coinsUpdatedEvent += UpdateText;
    }
    [SerializeField] UnityEngine.UI.Text _coinsText;
    void UpdateText()
    {
        _coinsText.text = GameManager.GetCoins().ToString();
    }

    void OnDisable()
    {
        GameManager.coinsUpdatedEvent -= UpdateText;
    }
}
