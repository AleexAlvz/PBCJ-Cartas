using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Faz o controle dos botoes do jogo inteiro
/// </summary>
public class ManageButtons : MonoBehaviour
{
    //Carrega o modo de jogo normal, feito em aula
    public void IniciaModoNormalDeJogo()
    {
        SceneManager.LoadScene(GameStrings.modoNormal);
        PlayerPrefs.SetString( GameStrings.ultimoModojogado , GameStrings.modoNormal); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega o primeiro modo customizado
    public void IniciaModoCustomizado1()
    {
        SceneManager.LoadScene(GameStrings.modoC1);
        PlayerPrefs.SetString(GameStrings.ultimoModojogado, GameStrings.modoC1); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega o segundo modo customizado
    public void IniciaModoCustomizado2()
    {
        SceneManager.LoadScene(GameStrings.modoC2);
        PlayerPrefs.SetString(GameStrings.ultimoModojogado, GameStrings.modoC2); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega tela de inicio
    public void VoltarParaInicio()
    {
        SceneManager.LoadScene(GameStrings.telaInicial);
    }

    // Busca nas Prefs o ultimo modo jogado e inicia ele, quando o botão replay é clicado.
    public void IniciaUltimoModoJogado()
    {
        string ultimoModoJogado = PlayerPrefs.GetString(GameStrings.ultimoModojogado);
        if (ultimoModoJogado == GameStrings.modoNormal) IniciaModoNormalDeJogo();
        else if (ultimoModoJogado == GameStrings.modoC1) IniciaModoCustomizado1();
        else if (ultimoModoJogado == GameStrings.modoC2) IniciaModoCustomizado2();
    }
}
