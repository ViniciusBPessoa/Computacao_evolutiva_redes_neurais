using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    public bool isDead = false;
    public int score;
    public NeuralNetwork rede_neural;
    public int inputSize;
    public int outputSize;
    [SerializeField] public int[] layerSizes;
    public float learningRate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rede_neural = new NeuralNetwork(inputSize, outputSize, layerSizes, learningRate);
        score = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDead) // Se o botão do mouse é pressionado (ou toque na tela)
        {
            Jump();
        }

        List<float> entrada = RecebeValores();
        entrada.Add(transform.position.y);
        double[] arreyEntrada = entrada.ConvertAll(input => (double)input).ToArray();

        if (rede_neural.Predict(arreyEntrada)[0] == 1 && !isDead){
            Jump();
        }

        if(transform.position.y <= -16)
        {
            turn_off_simutate();
        }
    }

    void Jump()
    {
        rb.velocity = Vector2.zero; // Zerar a velocidade para evitar pulos múltiplos com força acumulada
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar uma força para fazer o pássaro pular
    }
    public void Die()
    {
        isDead = true;
    }

    public void turn_off_simutate()
    {
        rb.simulated = false;
    }

    public List<float> RecebeValores()
    {
        GameObject[] cactosArray = GameObject.FindGameObjectsWithTag("cactos");

        Vector2 currentPos = transform.position;
        List<Vector2> distances = new List<Vector2>();

        float closestY = float.MaxValue;
        float closestXDistance = float.MaxValue;

        foreach (GameObject cacto in cactosArray)
        {
            Vector2 cactoPos = cacto.transform.position;
            float distanceX = Mathf.Abs(cactoPos.x - currentPos.x);

            if (distanceX < closestXDistance && distanceX > currentPos.x)
            {
                closestXDistance = distanceX;
                closestY = cactoPos.y;
            }
        }

        return new List<float> {closestXDistance, closestY};
    }


    public class NeuralNetwork{
        private int inputSize;
        private int outputSize;
        private int[] layerSizes;
        private float learningRate;
        public double[][] weights;
        public double[][] biases;

        public NeuralNetwork(int inputSize, int outputSize, int[] layerSizes, float learningRate)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            this.layerSizes = layerSizes;
            this.learningRate = learningRate;

            InitializeWeightsAndBiases();
        }

        private void InitializeWeightsAndBiases()
        {
            int totalLayers = layerSizes.Length + 1;
            weights = new double[totalLayers][];
            biases = new double[totalLayers][];

            int previousLayerSize = inputSize;

            for (int i = 0; i < layerSizes.Length; i++)
            {
                int currentLayerSize = layerSizes[i];

                weights[i] = new double[currentLayerSize * previousLayerSize];
                biases[i] = new double[currentLayerSize];

                // Inicialização dos pesos e bias com valores aleatórios
                for (int j = 0; j < currentLayerSize * previousLayerSize; j++)
                {
                    weights[i][j] = Random.Range(-1f, 1f);
                }

                for (int j = 0; j < currentLayerSize; j++)
                {
                    biases[i][j] = Random.Range(-1f, 1f);
                }

                previousLayerSize = currentLayerSize;
            }

            // Última camada para output
            weights[totalLayers - 1] = new double[outputSize * previousLayerSize];
            biases[totalLayers - 1] = new double[outputSize];

            // Inicialização dos pesos e bias com valores aleatórios para a camada de output
            for (int j = 0; j < outputSize * previousLayerSize; j++)
            {
                weights[totalLayers - 1][j] = Random.Range(-1f, 1f);
            }

            for (int j = 0; j < outputSize; j++)
            {
                biases[totalLayers - 1][j] = Random.Range(-1f, 1f);
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Mathf.Exp((float)-x));
        }

        public void Train()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    // Modifica aleatoriamente os pesos com base na taxa de aprendizado
                    weights[i][j] += Random.Range(-1f, 1f) * this.learningRate;
                }

                for (int j = 0; j < biases[i].Length; j++)
                {
                    // Modifica aleatoriamente as biases com base na taxa de aprendizado
                    biases[i][j] += Random.Range(-1f, 1f) * this.learningRate;
                }
            }
        }


        public double[] Predict(double[] input)
        {
            double[] layerOutput = input;

            for (int i = 0; i < weights.Length; i++)
            {
                double[] newLayerOutput = new double[biases[i].Length];

                for (int j = 0; j < biases[i].Length; j++)
                {
                    double neuronSum = biases[i][j];

                    for (int k = 0; k < layerOutput.Length; k++)
                    {
                        neuronSum += layerOutput[k] * weights[i][j * layerOutput.Length + k];
                    }

                    newLayerOutput[j] = Sigmoid(neuronSum);
                }

                layerOutput = newLayerOutput;
            }
            double[] binaryOutput = new double[layerOutput.Length];

            for (int i = 0; i < layerOutput.Length; i++)
            {
                // Aplicando um limiar para converter os valores em 0 ou 1
                binaryOutput[i] = (layerOutput[i] >= 0.5) ? 1 : 0;
            }

            return binaryOutput;
        }
    }
}
