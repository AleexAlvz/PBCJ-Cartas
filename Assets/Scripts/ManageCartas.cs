using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script gerenciador das cartas, responsável por gera-las e garantir o funcionamento do game do modo normal.
/// </summary>
public class ManageCartas : MonoBehaviour
{
    public GameObject carta; //Carta a ser descartada;
    public int numMaximoTentativas = 60;

    private GameObject carta1, carta2; //Armazena cartas reveladas;
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; //Indicadores para cada carta escolhida em uma linha.
    private string linhaCarta1, linhaCarta2; //Armazena a linha da primeira carta selecionada e da segunda carta selecionada.

    private string linha1Nipe = GameStrings.nipeClubs; //Define nipe da linha 1
    private string linha2Nipe = GameStrings.nipeHearts; //Define nipe da linha 2

    private string linha1BackColor = GameStrings.redColor; //Define backColor da linha 1 para vermelho
    private string linha2BackColor = GameStrings.redColor; //Define backColor da linha 2 para azul

    bool timerAcionado; //Boolean para o timer
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
        ultimoRecorde = PlayerPrefs.GetInt(GameStrings.recordeModoNormal, 0); // Seta o valor do ultimo recorde de menos tentativas no jogo.
        PlayerPrefs.SetString(GameStrings.ultimoModojogado, GameStrings.modoNormal); //Salva o ultimo modo jogado no PlayerPrefs

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
                timerAcionado = false;
                if (carta1.tag == carta2.tag) //Verifica se as cartas possuem mesmo valor no jogo da memória, para destrui-las.
                {
                    Destroy(carta1); //Caso acerte as cartas, destrói os gameObjects das duas cartas.
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
        linhaCarta1 = "";
        linhaCarta2 = "";
        timer = 0;
    }

    void MostraCartas()
    {
        int[] lista = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a primeira linha
        int[] lista2 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 12 para a segunda linha

        for (int i = 0; i < 13; i++)
        {
            AdicionaCarta(0, i, lista[i], linha1Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de clubs
            AdicionaCarta(1, i, lista2[i], linha2Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de hearts
        }
    }

    //Método responsável por instanciar a carta na posição correta.
    void AdicionaCarta(int linha, int rank, int valor, string nipe, string backColor)
    {
        GameObject centroDaTela = GameObject.Find("centroDaTela"); //Encontra o gameObject centro da tela
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;

        Vector3 novaPosicao = new Vector3(centroDaTela.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centroDaTela.transform.position.y + ((linha - 2 / 2) * fatorEscalaY), centroDaTela.transform.position.z); //Calcula posição da nova carta.
        GameObject novaCarta = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity)); //Instanica nova carta na posição calculada, para formação do baralho.
        novaCarta.tag = "" + valor; //configura tag da nova carta.
        novaCarta.name = "" + linha + "_" + valor; //configra nome da nova carta.

        string nomeCarta = "";
        string numeroCarta = "";

        if (valor == 0) //Configura a variavel numeroCarta de acordo com seu valor.
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

        nomeCarta = numeroCarta + "_of_" + nipe; // Nome da carta de acordo com o seu numero e o nipe.
        Sprite s1 = (Sprite)Resources.Load<Sprite>("Images/Cartas/" + nomeCarta); //Busca a carta pelo nome nos resources, no path "Cartas/"
        print("S1: " + s1);
        Tile tile = GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>(); //Encontra tile com nome especifico
        tile.SetCartaOriginal(s1); //Configura a imagem da carta
        tile.SetBackColor(backColor); //Configura backColor da carta
    }

    //Cria um array de 13 numeros embaralhado para o baralho.
    public int[] CriaArrayEmbaralhado()
    {
        int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; // Array ordenado
        int temp;
        for (int t = 0; t < 13; t++) // Embaralha o array de forma aleatória.
        {
            temp = array[t];
            int r = Random.Range(t, 13);
            array[t] = array[r];
            array[r] = temp;
        }
        return array;
    }

    //Chamada pelo Tile, avisando que uma carta especifica foi selecionada. Verifica se a carta é valdia para ser revelada.
    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada) //Verifica se foi a primeira carta selecionada
        {
            string linha = carta.name.Substring(0, 1); // Armazena a linha da carta selecionada.
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true; // Indica que a carta foi selecionada
            carta1 = carta; // define a carta1 como a carta clicada.
            carta1.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada) //Verifica se a primeira carta foi selecionada e a segunda não foi selecionada.
        {
            string linha2 = carta.name.Substring(0, 1); // Armazena a linha da carta selecionada.
            if (linhaCarta1 != linha2) // Verifica se a segunda carta está no outro baralho.
            {
                linhaCarta2 = linha2;
                segundaCartaSelecionada = true; // Indica que a carta foi selecionada
                carta2 = carta; // define a carta1 como a carta clicada.
                carta2.GetComponent<Tile>().RevelaCarta(); //Revela a carta clicada
                VerificaCartas();
            }
        }
    }

    public void VerificaCartas() //Aumenta o número de tentativas e ativa o timer para verificar as cartas.
    {
        DisparaTimer();
        numTentativas++; //Aumenta n onumero de tentativas.
        UpdateTentativas(); //Atualiza na tela as tentativas.
    }

    public void DisparaTimer() //Aciona o Timer quando as duas cartas forem escolhidas, para o usuario poder ver o que escolheu e tentar decorar.
    {
        timerAcionado = true;
    }


    void UpdateTentativas() //Atualiza número de tentativas na tela
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas + " / " + numMaximoTentativas;
    }


    void VerificaVitoriaEDerrota()  //Verifica se o jogador ganhou ou perdeu, direcionando para as respectivas telas.
    {
        if (numAcertos >= 13)
        {

            if ((numTentativas < ultimoRecorde) ^ (ultimoRecorde == 0)) //Caso o numero de tentativas seja um novo recorde, salva esse novo recorde no PlayerPrefs.
            {
                PlayerPrefs.SetInt(GameStrings.recordeModoNormal, numTentativas);
            }
            SceneManager.LoadScene(GameStrings.telaVitoria); //Leva para tela de vitoria
        }
        else if (numTentativas >= numMaximoTentativas) //Caso ultrapasse o limite de tentativas, vai para tela de derrota.
        {
            SceneManager.LoadScene(GameStrings.telaDerrota); //Leva para tela de derrota
        }
    }
}
