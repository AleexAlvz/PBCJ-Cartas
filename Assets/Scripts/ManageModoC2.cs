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
    public int numMaximoTentativas;
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
        ultimoRecorde = PlayerPrefs.GetInt("Recorde", 0); // Seta o valor do ultimo recorde de menos tentativas no jogo.
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
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                ladoCarta1 = "";
                ladoCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostraCartas()
    {
        //Instantiate(carta, new Vector3(0,0,0),Quaternion.identity);
        //AdicionaCarta();
        int[] lista = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a primeira linha lado esquerdo
        int[] lista2 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a segunda linha lado esquerdo
        int[] lista3 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a terceira linha lado esquerdo
        int[] lista4 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a quarta linha lado esquerdo

        int[] lista5 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a primeira linha lado direto 
        int[] lista6 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a segunda linha lado direto 
        int[] lista7 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a terceira linha lado direto 
        int[] lista8 = CriaArrayEmbaralhado(); //Pega array embaralhado de 0 a 3 para a quarta linha lado direto 

        for (int i = 0; i < 4; i++)
        {            
            AdicionaCarta(0, 0, i, lista[i], linha1Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de clubs                            
            AdicionaCarta(0, 1, i, lista2[i], linha2Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de hearts            
            AdicionaCarta(0, 2, i, lista3[i], linha3Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de hearts            
            AdicionaCarta(0, 3, i, lista4[i], linha4Nipe, linha1BackColor); //Gera baralho da primeira linha com nipe de hearts            

            AdicionaCarta(1, 0, i, lista5[i], linha3Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de clubs                            
            AdicionaCarta(1, 1, i, lista6[i], linha4Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de hearts            
            AdicionaCarta(1, 2, i, lista7[i], linha2Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de hearts            
            AdicionaCarta(1, 3, i, lista8[i], linha1Nipe, linha2BackColor); //Gera baralho da primeira linha com nipe de hearts            
        }
    }

    //Método responsável por instanciar a carta na posição correta.
    void AdicionaCarta(int lado,int linha, int rank, int valor, string nipe, string backColor)
    {
        GameObject centroDaTela = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;        
        Vector3 novaPosicao = new Vector3(centroDaTela.transform.position.x + ((rank - 4 / 2) * fatorEscalaX) + (lado*12) -5, centroDaTela.transform.position.y + ((linha - 2 / 2) * fatorEscalaY)-1, centroDaTela.transform.position.z); //Calcula posição da nova carta.

        GameObject novaCarta = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity)); //Instanica nova carta na posição calculada, para formação do baralho.

        novaCarta.name = "" + lado + "_" + valor + "-" + nipe; //configra nome da nova carta.

        string nomeCarta = "";
        string numeroCarta = "";


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

        nomeCarta = numeroCarta + "_of_" + nipe; // Nome da carta de acordo com o seu numero e o nipe.
        Sprite s1 = (Sprite)Resources.Load<Sprite>("Images/Cartas/" + nomeCarta); //Busca a carta pelo nome nos resources, no path "Cartas/"
        print("S1: " + s1);
        Tile tile = GameObject.Find("" + lado + "_" + valor + "-" + nipe).GetComponent<Tile>(); //Encontra tile com nome especifico
        tile.SetCartaOriginal(s1); //Configura a imagem da carta
        tile.SetBackColor(backColor); //Configura backColor da carta
    }

    //Cria um array de 13 numeros embaralhado para o baralho.
    public int[] CriaArrayEmbaralhado()
    {
        int[] array = new int[] { 0, 10, 11, 12 }; // Array ordenado
        int temp;
        for (int t = 0; t < 4; t++) // Embaralha o array de forma aleatória.
        {
            temp = array[t];
            int r = Random.Range(t, 4);
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
            if (ultimoRecorde == 0) //Caso seja o primeiro placar, salva como recorde.
            {
                PlayerPrefs.SetInt("Recorde", numTentativas);
            }
            else if (numTentativas < ultimoRecorde) //Caso o numero de tentativas seja um novo recorde, salva esse novo recorde no PlayerPrefs.
            {
                PlayerPrefs.SetInt("Recorde", numTentativas);
            }
            SceneManager.LoadScene("Vitoria");
        }
        else if (numTentativas >= numMaximoTentativas)
        {
            SceneManager.LoadScene("Derrota");
        }
    }
}
