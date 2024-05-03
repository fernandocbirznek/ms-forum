using System.ComponentModel;

namespace ms_forum.Enum
{
    public enum ForumTopicoEnum
    {
        [Description("geral")]
        Geral = 0,
        [Description("conceito")]
        Conceito = 1,
        [Description("equação")]
        Equacao = 2,
        [Description("curiosidade")]
        Curiosidade = 3,
        [Description("exercício")]
        Exercicio = 4,
        [Description("vestibular")]
        Vestibular = 5,
        [Description("noticia")]
        Noticia = 6
    }
}
