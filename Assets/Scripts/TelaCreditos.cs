using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Script respons�vel pela anima��o da tela de cr�ditos e sua dura��o, com o texto subindo.
/// </summary>
public class TelaCreditos : MonoBehaviour
{

    float speed = 0.1f;
    int time = 0;

    // Start is called before the first frame update
    void Update()
    {
        LoadCreditos();
        VerificaTempoDeCreditos();
        time++;
    }

    //Faz o movimento vertical dos cr�ditos de acordo com o tempo
    void LoadCreditos()
    {
        Vector3 posicao;
        posicao = new Vector3(transform.position.x , transform.position.y + speed, transform.position.z);
        transform.position = posicao;
    }

    //Define o tempo de exibi��o dos cr�ditos, para retornar ao inicio
    void VerificaTempoDeCreditos()
    {
        if(time>=12000)
        {
            SceneManager.LoadScene(GameStrings.telaInicial);
        }
    }
}
