using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Script responsavel pela animaçao da tela de creditos e sua duracao, com o texto subindo.
/// </summary>
public class TelaCreditos : MonoBehaviour
{

    public float Speed; //variavel indica a velocidade da transformacao da posicao 
    public int Fps; //variavel indica a taxa de atualizacao da minha tela

    private float _alturaTexto; // Altura do objeto de texto

    // Start is called before the first frame update
    private void Start()
    {
        QualitySettings.vSyncCount = 0; //O numero de VSyncs que devem passar entre cada quadro
        Application.targetFrameRate = Fps; //trava o FPS do jogo

        // Tenta calcular a altura do texto de creditos
        Text textoCreditos = GetComponent<Text>();
        int nLinhas = textoCreditos.text.Split('\n').Length;
        _alturaTexto = nLinhas * textoCreditos.fontSize;

        // Seta a posicao inicial do texto para logo abaixo da parte visivel
        Vector3 posicao = new Vector3(transform.position.x, Screen.height * (-1) + 500, transform.position.z);
        transform.position = posicao;
    }

    void Update()
    {
        LoadCreditos();
        if (VerificaCreditosSumiram()) { RetornarParaTelaInicial(); }
    }

    //Faz o movimento vertical dos creditos de acordo com o tempo
    void LoadCreditos()
    {
        Vector3 posicao;
        posicao = new Vector3(transform.position.x, transform.position.y + Speed, transform.position.z);
        transform.position = posicao;
    }

    // Verifica se todos os creditos ja rodaram, ou seja, se sumiram da tela
    bool VerificaCreditosSumiram()
    {
        return transform.position.y - _alturaTexto > 0;
    }

    // Retorna para a tela inicial
    void RetornarParaTelaInicial()
    {
        SceneManager.LoadScene(GameStrings.telaInicial);
    }
}
