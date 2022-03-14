using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script gerenciador das cartas, responsável por gera-las e garantir o funcionamento do game.
/// </summary>
public class manageCartas : MonoBehaviour
{

    public GameObject carta; //Carta a ser descartada;

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas(); //Carrega as cartas ao inicio do jogo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MostraCartas()
    {
        //Instantiate(carta, new Vector3(0,0,0),Quaternion.identity);
        //AdicionaCarta();
        int[] lista = criaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a primeira linha
        int[] lista2 = criaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a segunda linha
        
        for (int i=0;i<12;i++)
        {
            AdicionaCarta(0, i, lista[i], "clubs"); //Gera baralho da primeira linha com nipe de clubs
            AdicionaCarta(1, i, lista2[i], "hearts"); //Gera baralho da primeira linha com nipe de hearts

        }
    }

    //Método responsável por instanciar a carta na posição correta.
    void AdicionaCarta(int linha, int rank, int valor, string nipe)
    {
        GameObject centroDaTela = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;
        //Vector3 novaPosicao = new Vector3(centroDaTela.transform.position.x + ((rank - 13 / 2) * 1.3f), centroDaTela.transform.position.y, centroDaTela.transform.position.z);
        //GameObject novaCarta = (GameObject)(Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity));
        //GameObject novaCarta = (GameObject)(Instantiate(carta, new Vector3(rank*2.0f, 0, 0), Quaternion.identity));
        Vector3 novaPosicao = new Vector3(centroDaTela.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centroDaTela.transform.position.y + ((linha - 2/2) * fatorEscalaY), centroDaTela.transform.position.z); //Calcula posição da nova carta.
        GameObject novaCarta = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity)); //Instanica nova carta na posição calculada, para formação do baralho.
        novaCarta.tag = "" +valor; //configura tag da nova carta.
        novaCarta.name = "" +linha+"_"+valor; //configra nome da nova carta.

        string nomeCarta = "";
        string numeroCarta = "";

        /*
        if(rank==0)
        {
            numeroCarta = "ace";
        } else if (rank==10)
        {
            numeroCarta = "jack";
        }
        else if (rank == 11)
        {
            numeroCarta = "queen";
        }
        else if (rank == 12)
        {
            numeroCarta = "king";
        } else
        {
            numeroCarta = "" + (rank + 1);
        }
        */ //else if para array ordenado no deck

        //Tratamento para pegar o nome correto da carta
        if (valor == 0)
        {
            numeroCarta = "ace";
        }
        else if (valor == 10)
        {
            numeroCarta = "jack";
        }
        else if (valor == 11)
        {
            numeroCarta = "queen";
        }
        else if (valor == 12)
        {
            numeroCarta = "king";
        }
        else
        {
            numeroCarta = "" + (valor + 1);
        }

        nomeCarta = numeroCarta + "_of_"+nipe; // Nome da carta de acordo com o seu numero e o nipe.
        Sprite s1 = (Sprite)Resources.Load<Sprite>("Cartas/"+nomeCarta); //Busca a carta pelo nome nos resources, no path "Cartas/"
        print("S1: " + s1);
        //GameObject.Find(""+valor).GetComponent<Tile>().SetCartaOriginal(s1);
        GameObject.Find("" +linha+"_"+valor).GetComponent<Tile>().SetCartaOriginal(s1);
    }

    //Cria um array de 13 numeros embaralhado para o baralho.
    public int[] criaArrayEmbaralhado()
    {
        int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for(int t=0;t<13;t++)
        {
            temp = array[t];
            int r = Random.Range(t, 13);
            array[t] = array[r];
            array[r] = temp;
        }
        return array;
    }
}
