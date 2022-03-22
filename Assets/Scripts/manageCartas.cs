using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script gerenciador das cartas, responsável por gera-las e garantir o funcionamento do game.
/// </summary>
public class ManageCartas : MonoBehaviour
{
    public GameObject carta; //Carta a ser descartada;
    public int numMaximoTentativas;
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; //Indicadores para cada carta escolhida em uma linha.
    private GameObject carta1, carta2; //Armazena cartas reveladas;
    private string linhaCarta1, linhaCarta2; //Armazena a linha da primeira carta selecionada e da segunda carta selecionada.
    public string linha1Nipe, linha2Nipe; //Escolhe o nipe das cartas pela linha
    public string linha1BackColor, linha2BackColor; //Escolhe a backColor das cartas

    bool timerAcionado, timerPausado; //Boolean para o timer
    float timer; //Variavel que representa o timer quando duas cartas são escolhidas.

    int numTentativas = 0; //Tentativas de escolher duas cartas.
    int numAcertos = 0; // Quantidade de vezes que acertou as cartas com mesmo número.
    AudioSource somAcerto; //Som de acerto.

    int ultimoRecorde = 0; //Variavel de recorde de menos tentativas no jogo.

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas(); //Carrega as cartas ao inicio do jogo
        UpdateTentativas(); //Atualiza numero de tentativas
        somAcerto = GetComponent<AudioSource>(); //Pega o AudioSource do GameObject gameManager
        ultimoRecorde = PlayerPrefs.GetInt("Recorde", 0); // Seta o valor do ultimo recorde de menos tentativas no jogo.
        PlayerPrefs.SetString("UltimoModoJogado", "ModoNormal"); //Salva o ultimo modo jogado no PlayerPrefs
        GameObject.Find("ultimoRecorde").GetComponent<Text>().text = "Recorde: " + ultimoRecorde;
    }

    // Update is called once per frame
    void Update()
    {
        VerificaVitoriaEDerrota();

        if(timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if(timer>1) //Caso tempo seja maior que 1, verifica se houve acerto ou erro no jogo
            {
                timerPausado = true;
                timerAcionado = false;
                if(carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somAcerto.Play();
                } else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostraCartas()
    {
        //Instantiate(carta, new Vector3(0,0,0),Quaternion.identity);
        //AdicionaCarta();
        int[] lista = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a primeira linha
        int[] lista2 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a segunda linha
        
        for (int i=0;i<13;i++)
        {
            AdicionaCarta(0, i, lista[i], linha1Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de clubs
            AdicionaCarta(1, i, lista2[i], linha2Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de hearts
        }
    }

    //Método responsável por instanciar a carta na posição correta.
    void AdicionaCarta(int linha, int rank, int valor, string nipe, string backColor)
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
        Sprite s1 = (Sprite)Resources.Load<Sprite>("Images/Cartas/"+nomeCarta); //Busca a carta pelo nome nos resources, no path "Cartas/"
        print("S1: " + s1);
        //GameObject.Find(""+valor).GetComponent<Tile>().SetCartaOriginal(s1);
        Tile tile = GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>(); //Encontra tile com nome especifico
        tile.SetCartaOriginal(s1); //Configura a imagem da carta
        tile.SetBackColor(backColor); //Configura backColor da carta
    }

    //Cria um array de 13 numeros embaralhado para o baralho.
    public int[] CriaArrayEmbaralhado()
    {
        int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; // Array ordenado
        int temp;
        for(int t=0;t<13;t++) // Embaralha o array de forma aleatória.
        {
            temp = array[t];
            int r = Random.Range(t, 13);
            array[t] = array[r];
            array[r] = temp;
        }
        return array;
    }

    public void CartaSelecionada(GameObject carta)
    {
        if(!primeiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1); // Armazena a linha da carta selecionada.
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true; // Indica que a carta foi selecionada
            carta1 = carta; // define a carta1 como a carta clicada.
            carta1.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
        } else if(primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha2 = carta.name.Substring(0, 1); // Armazena a linha da carta selecionada.
            if(linhaCarta1 != linha2) // Verifica se a segunda carta está no outro baralho.
            {
                linhaCarta2 = linha2;
                segundaCartaSelecionada = true; // Indica que a carta foi selecionada
                carta2 = carta; // define a carta1 como a carta clicada.
                carta2.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
                VerificaCartas();
            }
        }
    }

    public void VerificaCartas() //Dispara o timer
    {
        DisparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    public void DisparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }

    void UpdateTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: "+numTentativas+" / "+numMaximoTentativas;
    }

    void VerificaVitoriaEDerrota()
    {
        if (numAcertos >= 13)
        {
            if (ultimoRecorde == 0) //Caso seja o primeiro placar, salva como recorde.
            {
                PlayerPrefs.SetInt("Recorde", numTentativas);
            }
            else if (numTentativas < ultimoRecorde) //Caso o numero de tentativas seja um novo recorde, salva esse novo recorde no PlayerPrefs.
            {
                PlayerPrefs.SetInt("Recorde", numTentativas);
            }
            SceneManager.LoadScene("Vitoria");
        } else if (numTentativas>=numMaximoTentativas)
        {
            SceneManager.LoadScene("Derrota");
        }
    }
}
