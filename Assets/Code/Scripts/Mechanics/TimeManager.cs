using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Space(5)] public Times timeData = new Times();

    [System.Serializable]
    public class Times
    {
        public float currentTime;
        public float maxTime;
        public float speedTime;

        public bool DawnTime;
        public bool morningTime;
        public bool afternoonTime;
        public bool duskTime;
        public bool nightTime;
    }

    private void Update()
    {
        UpdateTime(); // Atualiza o tempo com base na condição de aumento ou reinício

        // Adiciona condições para definir os valores booleanos com base nas porcentagens do tempo
        timeData.DawnTime = IsTimeInRange(0f, 0.1f);
        timeData.morningTime = IsTimeInRange(0.1f, 0.3f);
        timeData.afternoonTime = IsTimeInRange(0.3f, 0.6f);
        timeData.duskTime = IsTimeInRange(0.6f, 0.7f);
        timeData.nightTime = IsTimeInRange(0.7f, 1.0f);
    }

    // Verifica se o tempo deve ser aumentado ou reiniciado
    private void UpdateTime()
    {
        timeData.currentTime = ShouldIncreaseTime() ? IncreaseTime() : ResetTime();
    }

    // Verifica se o tempo atual é menor ou igual ao tempo máximo
    private bool ShouldIncreaseTime()
    {
        return timeData.currentTime <= timeData.maxTime;
    }

    // Aumenta o tempo atual
    private float IncreaseTime()
    {
        return timeData.currentTime + timeData.speedTime;
    }

    // Reinicia o tempo para zero
    private float ResetTime()
    {
        return 0f;
    }

    // Verifica se o tempo está dentro de uma determinada faixa percentual
    private bool IsTimeInRange(float minPercentage, float maxPercentage)
    {
        float percentage = timeData.currentTime / timeData.maxTime;
        return percentage >= minPercentage && percentage < maxPercentage;
    }
}
