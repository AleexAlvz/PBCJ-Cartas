using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável pelo mecanismo da carta no baralho
/// </summary>
public class Tile : MonoBehaviour
{

    //private bool cartaRevelada = false; // Indicador da carta virada ou não (não está sendo usado)
    public Sprite cartaOriginal; //Sprite da frente da carta
    public Sprite cartaBackVermelha; //Sprite do avesso da carta vermelha
    public Sprite cartaBackBlue; //Sprite do avesso da carta preta
    private string backColor; //Cor que vai ficar no avesso da carta

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
        /*
        if(cartaRevelada)
            EscondeCarta();
        else
            RevelaCarta();
        */ //Aqui não se guardava a carta.
        GameObject.Find("gameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject);
    }

    public void SetBackColor(string backColorString)
    {
        backColor = backColorString;
    }

    public void EscondeCarta() //Vira a carta, mostrando a parte de trás
    {
        if (backColor == "red")
        {
            GetComponent<SpriteRenderer>().sprite = cartaBackVermelha; //configura o sprite do Tile para back vermelho
        } else if (backColor == "blue")
        {
            GetComponent<SpriteRenderer>().sprite = cartaBackBlue; //configura o sprite do Tile para back preto
        }
        
        //cartaRevelada = false; //indicador de cartaRevelada não está sendo usado
    }

    public void RevelaCarta() //Vira a carta, mostrando a parte da frente
    {
        GetComponent<SpriteRenderer>().sprite = cartaOriginal; //configura o sprite do Tile
        //cartaRevelada = true; //indicador de cartaRevelada não está sendo usado
    }

    public void SetCartaOriginal(Sprite sprite) //configura o sprite representate da carta instanciada
    {
        cartaOriginal = sprite;
    }
}
