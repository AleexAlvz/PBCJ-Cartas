using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script gerenciador das cartas, respons?vel por gera-las e garantir o funcionamento do game.
/// </summary>
public class ManageModoC2 : MonoBehaviour
{
    public GameObject carta; //Carta a ser descartada;
    public int numMaximoTentativas = 60; //Numero maximo de tentativas antes do jogador ser derrotado.
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; //Indicadores para cada carta escolhida em uma linha.
    private GameObject carta1, carta2 ; //Armazena cartas reveladas;
    private string ladoCarta1, ladoCarta2; //Armazena o lado das cartas escolhidas, na ordem

    bool timerAcionado; //Boolean para o timer
    float timer; //Variavel que representa o timer quando duas cartas s?o escolhidas.

    int numTentativas = 0; //Tentativas de escolher duas cartas.
    int numAcertos = 0; // Quantidade de vezes que acertou as cartas com mesmo n?mero.
    AudioSource somAcerto; //Som de acerto.

    int ultimoRecorde = 0; //Variavel de recorde de menos tentativas no jogo.

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas(); //Carrega as cartas ao inicio do jogo
        UpdateTentativas(); //Atualiza numero de tentativas
        somAcerto = GetComponent<AudioSource>(); //Pega o AudioSource do GameObject gameManager
        ultimoRecorde = PlayerPrefs.GetInt(GameStrings.recordeModoC2, 0); // Seta o valor do ultimo recorde de menos tentativas no jogo.
        GameObject.Find("ultimoRecorde").GetComponent<Text>().text = "Recorde: " + ultimoRecorde; //Pega ultimo recorde do modo
        PlayerPrefs.SetString(GameStrings.ultimoModojogado, GameStrings.modoC2); //Salva o ultimo modo jogado no PlayerPrefs
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
                timerAcionado = false;
                //carta.name
                //if (carta1.tag == carta2.tag)
                
                if (carta1.name.Substring(3, carta1.name.Length - 3) == carta2.name.Substring(3, carta2.name.Length - 3)) //Verifica se as cartas s?o as mesmas para destrui-las, e caso n?o sejam, esconde as cartas.
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++; //Aumenta numero de acertos.
                    somAcerto.Play(); //Toca o som de acerto quando duas cartas s?o identicas.
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

    private void LimpaCampos() //Limpa todos os campos necess?rios ap?s uma tentativa de escolher duas cartas.
    {
        primeiraCartaSelecionada = false;
        segundaCartaSelecionada = false;
        carta1 = null;
        carta2 = null;
        ladoCarta1 = "";
        ladoCarta2 = "";
        timer = 0;
    }

    void MostraCartas() //M?todo responsavel por gerar as cartas, com aleatoriedade na posi??o das cartas no baralho.
    {
        int[] lista = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a primeira linha lado esquerdo
        int[] lista2 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a segunda linha lado esquerdo

        int linha = 0;
        int rank = 0;
        for (int i = 0; i < 16; i++) //Algoritmo que adiciona as cartas de acordo com a posi??o, mudando a coluna e a linha da carta.
        {
            AdicionaCarta(GameStrings.esquerda, linha, rank, lista[i], GameStrings.blueColor);
            AdicionaCarta(GameStrings.direita, linha, rank, lista2[i], GameStrings.redColor);

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

    //M?todo respons?vel por instanciar a carta na posi??o correta.
    void AdicionaCarta(string lado,int linha, int rank, int valor, string backColor)
    {
        string letraCarta = "";
        string nipeCarta = "";

        switch (valor) //Verifica qual a letra e o nipe da carta de acordo com seu valor recebido.
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
        
        GameObject centroDaTela = GameObject.Find("centroDaTela"); //Encontra o centro da tela para posicionamento da carta.
        float escalaCartaOriginal = carta.transform.localScale.x; //Pega a escala da carta.
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f; //Calcula fator escala em x para posicionamento de carta.
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f; //Calcula fator escala em y para posicionamento de carta.
        int valorLado;
        if (lado == GameStrings.esquerda)
        {
            valorLado = -1;
        } else
        {
            valorLado = 1;
        }
        Vector3 novaPosicao = new Vector3(valorLado*(centroDaTela.transform.position.x + ((rank - 2) * fatorEscalaX) + 5), centroDaTela.transform.position.y + ((linha - 2 / 2) * fatorEscalaY) - 1, centroDaTela.transform.position.z); //Calcula posi??o da nova carta. O lado mant?m ou inverte o sinal do calculo da coordenada em x.

        GameObject novaCarta = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity)); //Instanica nova carta na posi??o calculada, para forma??o do baralho.

        novaCarta.name = "" + lado + "_" + letraCarta + "-" + nipeCarta; //configura nome da nova carta.

        string nomeCarta = "";
        nomeCarta = letraCarta + "_of_" + nipeCarta; // Nome da carta de acordo com o seu numero e o nipe.
        print(nomeCarta);

        Sprite s1 = (Sprite)Resources.Load<Sprite>("Images/Cartas/" + nomeCarta); //Busca a carta pelo nome nos resources, no path "Images/Cartas/"
        Tile tile = GameObject.Find("" + lado + "_" + letraCarta + "-" + nipeCarta).GetComponent<Tile>(); //Encontra tile com nome especifico
        tile.SetCartaOriginal(s1); //Configura a imagem da carta
        tile.SetBackColor(backColor); //Configura backColor da carta
    }

    //Cria um array de 16 numeros embaralhado para o baralho de 16 cartas apenas com letras e 4 nipes.
    public int[] CriaArrayEmbaralhado()
    {
        int[] array = new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 }; // Array ordenado
        int temp;
        for (int t = 0; t < 16; t++) // Embaralha o array de forma aleat?ria.
        {
            temp = array[t];
            int r = Random.Range(t, 16);
            array[t] = array[r];
            array[r] = temp;
        }
        return array;
    }

    public void CartaSelecionada(GameObject carta) //Chamada pelo Tile carta, notifica que uma carta especifica foi chamada, e caso a regra esteja valida, revela a carta.
    {
        if (!primeiraCartaSelecionada)
        {
            string lado = carta.name.Substring(0, 2); // Armazena a lado da carta selecionada.
            ladoCarta1 = lado;
            primeiraCartaSelecionada = true; // Indica que a carta foi selecionada
            carta1 = carta; // define a carta1 como a carta clicada.
            carta1.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string Lado2 = carta.name.Substring(0, 2); // Armazena o lado da carta selecionada.
            if (ladoCarta1 != Lado2) // Verifica se a segunda carta est? no outro baralho.
            {
                ladoCarta2 = Lado2;
                segundaCartaSelecionada = true; // Indica que a carta foi selecionada
                carta2 = carta; // define a carta1 como a carta clicada.
                carta2.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
                VerificaCartas();
            }
        }
    }

    public void VerificaCartas() //Dispara o timer para verificar as cartas e atualiza numero de tentativas na tela.
    {
        DisparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    public void DisparaTimer() //Aciona o timer.
    {
        timerAcionado = true;
    }

    void UpdateTentativas() //Atualiza numero de tentativas na tela.
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas + " / " + numMaximoTentativas;
    }

    void VerificaVitoriaEDerrota() //Verifica se, pela regra do jogo, o jogador venceu ou perdeu at? o momento. Caso n?o esteja em nenhuma das situa??es, n?o faz nada.
    {
        if (numAcertos >= 16)
        {  
            if ((numTentativas < ultimoRecorde) ^ (ultimoRecorde == 0)) //Caso o numero de tentativas seja um novo recorde, salva esse novo recorde no PlayerPrefs.
            {
                PlayerPrefs.SetInt(GameStrings.recordeModoC2, numTentativas); //Configura novo recorde no modo de jogo.
            }
            SceneManager.LoadScene(GameStrings.telaVitoria); //Chama tela de vit?ria
        }
        else if (numTentativas >= numMaximoTentativas)
        {
            SceneManager.LoadScene(GameStrings.telaDerrota); //Chama tela de derrota
        }
    }
}
