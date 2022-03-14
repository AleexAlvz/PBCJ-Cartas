using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável pelo mecanismo da carta no baralho
/// </summary>
public class Tile : MonoBehaviour
{

    private bool cartaRevelada = false; //Indicador da carta virada ou não
    public Sprite cartaOriginal; //Sprite da frente da carta
    public Sprite cartaBack; //Sprite do avesso da carta

    public Sprite novaCarta; //atualiza carta

    // Start is called before the first frame update
    void Start()
    {
        EscondeCarta(); //Inicia o jogo com as cartas para baixo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Ao clicar na carta, inverte o estado para revelada ou escondida.
    public void OnMouseDown()
    {
        print("Você clicou em uma carta!");
        if(cartaRevelada)
            EscondeCarta();
        else
            RevelaCarta();
    }

    public void EscondeCarta() //Vira a carta, mostrando a parte de trás
    {
        GetComponent<SpriteRenderer>().sprite = cartaBack; //configura o sprite do Tile
        cartaRevelada = false;
    }

    public void RevelaCarta() //Vira a carta, mostrando a parte da frente
    {
        GetComponent<SpriteRenderer>().sprite = cartaOriginal; //configura o sprite do Tile
        cartaRevelada = true;
    }

    public void SetCartaOriginal(Sprite sprite) //configura o sprite representate da carta instanciada
    {
        cartaOriginal = sprite;
    }
}
