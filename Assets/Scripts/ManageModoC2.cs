using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script gerenciador das cartas, responsável por gera-las e garantir o funcionamento do game.
/// </summary>
public class ManageModoC2 : MonoBehaviour
{
    public GameObject carta; //Carta a ser descartada;
    public int numMaximoTentativas = 60;
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; //Indicadores para cada carta escolhida em uma linha.
    private GameObject carta1, carta2 ; //Armazena cartas reveladas;
    private string ladoCarta1, ladoCarta2;
    public string linha1Nipe, linha2Nipe, linha3Nipe, linha4Nipe; //Escolhe o nipe das cartas pela linha
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
        ultimoRecorde = PlayerPrefs.GetInt(GameStrings.recordeModoC2, 0); // Seta o valor do ultimo recorde de menos tentativas no jogo.
        //PlayerPrefs.SetString("UltimoModoJogado", "ModoNormal"); //Salva o ultimo modo jogado no PlayerPrefs
        GameObject.Find("ultimoRecorde").GetComponent<Text>().text = "Recorde: " + ultimoRecorde;
    }

    // Update is called once per frame
    void Update()
    {
        VerificaVitoriaEDerrota();

        if (timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if (timer > 1) //Caso tempo seja maior que 1, verifica se houve acerto ou erro no jogo
            {
                timerPausado = true;
                timerAcionado = false;
                //carta.name
                //if (carta1.tag == carta2.tag)
                
                if (carta1.name.Substring(2, carta1.name.Length - 2) == carta2.name.Substring(2, carta2.name.Length - 2))
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somAcerto.Play();
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                LimpaCampos();
            }
        }
    }

    private void LimpaCampos() //Limpa todos os campos necessários após uma tentativa de escolher duas cartas.
    {
        primeiraCartaSelecionada = false;
        segundaCartaSelecionada = false;
        carta1 = null;
        carta2 = null;
        ladoCarta1 = "";
        ladoCarta2 = "";
        timer = 0;
    }

    void MostraCartas()
    {
        int[] lista = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a primeira linha lado esquerdo
        int[] lista2 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a segunda linha lado esquerdo

        int linha = 0;
        int rank = 0;
        for (int i = 0; i < 16; i++) //Algoritmo que adiciona as cartas de acordo com a posição, mudando a coluna e a linha da carta.
        {
            AdicionaCarta(0, linha, rank, lista[i], linha1BackColor);
            AdicionaCarta(1, linha, rank, lista2[i], linha2BackColor);

            if (rank == 3)
            {
                rank = 0;
                linha++;
            } else
            {
                rank++;
            }
        }
    }

    //Método responsável por instanciar a carta na posição correta.
    void AdicionaCarta(int lado,int linha, int rank, int valor, string backColor)
    {
        string letraCarta = "";
        string nipeCarta = "";

        switch (valor)
        {
            case 0:
                letraCarta = "ace";
                nipeCarta = "clubs";
                break;
            case 1:
                letraCarta = "ace";
                nipeCarta = "hearts";
                break;
            case 2:
                letraCarta = "ace";
                nipeCarta = "spades";
                break;
            case 3:
                letraCarta = "ace";
                nipeCarta = "diamonds";
                break;
            case 4:
                letraCarta = "jack";
                nipeCarta = "clubs";
                break;
            case 5:
                letraCarta = "jack";
                nipeCarta = "hearts";
                break;
            case 6:
                letraCarta = "jack";
                nipeCarta = "spades";
                break;
            case 7:
                letraCarta = "jack";
                nipeCarta = "diamonds";
                break;
            case 8:
                letraCarta = "queen";
                nipeCarta = "clubs";
                break;
            case 9:
                letraCarta = "queen";
                nipeCarta = "hearts";
                break;
            case 10:
                letraCarta = "queen";
                nipeCarta = "spades";
                break;
            case 11:
                letraCarta = "queen";
                nipeCarta = "diamonds";
                break;
            case 12:
                letraCarta = "king";
                nipeCarta = "clubs";
                break;
            case 13:
                letraCarta = "king";
                nipeCarta = "hearts";
                break;
            case 14:
                letraCarta = "king";
                nipeCarta = "spades";
                break;
            case 15:
                letraCarta = "king";
                nipeCarta = "diamonds";
                break;
        }
        
        GameObject centroDaTela = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;
        Vector3 novaPosicao = new Vector3(centroDaTela.transform.position.x + ((rank - 4 / 2) * fatorEscalaX) + (lado * 12) - 5, centroDaTela.transform.position.y + ((linha - 2 / 2) * fatorEscalaY) - 1, centroDaTela.transform.position.z); //Calcula posição da nova carta.

        GameObject novaCarta = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity)); //Instanica nova carta na posição calculada, para formação do baralho.

        novaCarta.name = "" + lado + "_" + valor + "-" + nipeCarta; //configra nome da nova carta.

        string nomeCarta = "";
        

        nomeCarta = letraCarta + "_of_" + nipeCarta; // Nome da carta de acordo com o seu numero e o nipe.
        Sprite s1 = (Sprite)Resources.Load<Sprite>("Images/Cartas/" + nomeCarta); //Busca a carta pelo nome nos resources, no path "Cartas/"
        print("S1: " + s1);
        Tile tile = GameObject.Find("" + lado + "_" + valor + "-" + nipeCarta).GetComponent<Tile>(); //Encontra tile com nome especifico
        tile.SetCartaOriginal(s1); //Configura a imagem da carta
        tile.SetBackColor(backColor); //Configura backColor da carta
    }

    //Cria um array de 16 numeros embaralhado para o baralho de 16 cartas apenas com letras e 4 nipes.
    public int[] CriaArrayEmbaralhado()
    {
        int[] array = new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 }; // Array ordenado
        int temp;
        for (int t = 0; t < 16; t++) // Embaralha o array de forma aleatória.
        {
            temp = array[t];
            int r = Random.Range(t, 16);
            array[t] = array[r];
            array[r] = temp;
        }
        return array;
    }

    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada)
        {
            string lado = carta.name.Substring(0, 1); // Armazena a lado da carta selecionada.
            ladoCarta1 = lado;
            primeiraCartaSelecionada = true; // Indica que a carta foi selecionada
            carta1 = carta; // define a carta1 como a carta clicada.
            carta1.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string Lado2 = carta.name.Substring(0, 1); // Armazena a linha da carta selecionada.
            if (ladoCarta1 != Lado2) // Verifica se a segunda carta está no outro baralho.
            {
                ladoCarta2 = Lado2;
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
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas + " / " + numMaximoTentativas;
    }

    void VerificaVitoriaEDerrota()
    {
        if (numAcertos >= 13)
        {  
            if ((numTentativas < ultimoRecorde) ^ (ultimoRecorde == 0)) //Caso o numero de tentativas seja um novo recorde, salva esse novo recorde no PlayerPrefs.
            {
                PlayerPrefs.SetInt(GameStrings.recordeModoC2, numTentativas);
            }
            SceneManager.LoadScene(GameStrings.telaVitoria);
        }
        else if (numTentativas >= numMaximoTentativas)
        {
            SceneManager.LoadScene(GameStrings.telaDerrota);
        }
    }
}
