using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Armazena enums de várias strings, evitando o hard code durante a programação e diminuindo a refatoração futura.
/// </summary>
public static class GameStrings
{
    public const string ultimoModojogado = "UltimoModoJogado"; //Salva string ultimoModoJogado

    public const string telaInicial = "TelaInicial"; //Armazena nome da tela inicial
    public const string telaVitoria = "Vitoria"; //Armazena nome da tela de vitória
    public const string telaDerrota = "Derrota"; //Armazena nome da tela de derrota

    public const string modoNormal = "CartasModo1"; //String do modo normal
    public const string modoC1 = "CartasModo2"; //String do modo C1
    public const string modoC2 = "CartasModo3"; //String do modo C2

    public const string recordeModoNormal = "RecordeModoNormal"; //String para recorde no modo normal
    public const string recordeModoC1 = "RecordeModoC1"; //String para recorde no modo c1
    public const string recordeModoC2 = "RecordeModoC2"; //String para recorde no modo c2

    public const string nipeClubs = "clubs"; //String do nipe clubs
    public const string nipeHearts = "hearts"; //String do nipe hearts

    public const string redColor = "red"; //backColor da carta na cor vermelha
    public const string blueColor = "blue"; //backColor da carta na cor azul
}
