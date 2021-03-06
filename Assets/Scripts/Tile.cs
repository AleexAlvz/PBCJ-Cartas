using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe respons?vel pelo mecanismo da carta no baralho
/// </summary>
public class Tile : MonoBehaviour
{

    //private bool cartaRevelada = false; // Indicador da carta virada ou n?o (n?o est? sendo usado)
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

    //Ao clicar na carta, verifica de qual jogo pertence e inverte o estado para revelada ou escondida.
    public void OnMouseDown()
    {
        print("Voc? clicou em uma carta!");        

        string ultimoModoJogado = PlayerPrefs.GetString(GameStrings.ultimoModojogado);
        if (ultimoModoJogado == GameStrings.modoNormal) GameObject.Find("gameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject); //Verifica a qual jogo a carta pertence, e notifica do click.
        else if (ultimoModoJogado == GameStrings.modoC1) GameObject.Find("gameManager").GetComponent<ManageModoC1>().CartaSelecionada(gameObject);
        else if (ultimoModoJogado == GameStrings.modoC2) GameObject.Find("gameManager").GetComponent<ManageModoC2>().CartaSelecionada(gameObject);
    }

    public void SetBackColor(string backColorString) //Configura a cor das costas da carta.
    {
        backColor = backColorString; 
    }

    public void EscondeCarta() //Vira a carta, mostrando a parte de tr?s
    {
        if (backColor == GameStrings.redColor)
        {
            GetComponent<SpriteRenderer>().sprite = cartaBackVermelha; //configura o sprite do Tile para back vermelho
        } else if (backColor == GameStrings.blueColor)
        {
            GetComponent<SpriteRenderer>().sprite = cartaBackBlue; //configura o sprite do Tile para back preto
        }
        
        //cartaRevelada = false; //indicador de cartaRevelada n?o est? sendo usado
    }

    public void RevelaCarta() //Vira a carta, mostrando a parte da frente
    {
        GetComponent<SpriteRenderer>().sprite = cartaOriginal; //configura o sprite do Tile       
    }

    public void SetCartaOriginal(Sprite sprite) //configura o sprite representante da carta instanciada
    {
        cartaOriginal = sprite;
    }
}
