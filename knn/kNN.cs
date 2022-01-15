using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace knn
{
    public class KNN
    {
        #region Переменные для работы в классе
        List<double[]> trainData;
        List<double[]> testData;
        int numtich, numtest, statval;
        #endregion
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="trainData">Обучающая выборка</param>
        /// <param name="testData">Тестовая выборка</param>
        /// <param name="numtich">Количество векторов в обучающей выборке</param>
        /// <param name="numtest">Количество векторов в тестовой выборке</param>
        /// <param name="statval">Количество переменных в векторе</param>
        public KNN(List<double[]> trainData, List<double[]> testData, int numtich, int numtest, int statval)
        {
            this.trainData = trainData;
            this.testData = testData;
            this.numtich = numtich;
            this.numtest = numtest;
            this.statval = statval;
        }
        /// <summary>
        /// Запуск распознавания
        /// </summary>
        /// <param name="listBox">Переменная листа для записи</param>
        /// <param name="k">Количество соседей</param>
        public void Start(ListBox listBox, int k = 1)
        {
            double[,] train = ConvertToDoubleArray(this.trainData, numtich);
            double[,] test = ConvertToDoubleArray(this.testData, numtest);
            List<int> errorTest = Testing(test, train, 15, k);
            double accuracy = Math.Round((((double)numtest - (double)errorTest.Count()) / (double)numtest), 2, MidpointRounding.AwayFromZero) * 100;
            listBox.Items.Add("Количество ошибок: " + errorTest.Count);
            listBox.Items.Add("Точность: " + accuracy.ToString() + "%");
        }
        /// <summary>
        /// Преобразование из List в double[,]
        /// </summary>
        /// <param name="tmp">Лист с данными</param>
        /// <param name="count">Количество элементов в листе</param>
        /// <returns>Матрица double[,]</returns>
        double[,] ConvertToDoubleArray(List<double[]> tmp, int count = 0)
        {
            double[,] result = new double[statval + 1, count];
            int tp = 0;
            foreach (double[] tmpItem in tmp)
            {
                for (int j = 0; j < statval + 1; j++)
                    result[j, tp] = tmpItem[j];
                tp++;
            }
            return result;
        }
        /// <summary>
        /// Запуска проверки тестовой выборки
        /// </summary>
        /// <param name="testData">Тестовые данные</param>
        /// <param name="trainData">Обучающая выборка</param>
        /// <param name="numClasses">Количество классов</param>
        /// <param name="k">Количество соседей</param>
        /// <param name="numrow">Исключаемый параметр</param>
        /// <returns>Список векторов с неверным распознанным классом</returns>
        List<int> Testing(double[,] testData, double[,] trainData, int numClasses, int k, int numrow = -1)
        {
            List<int> errortest = new List<int>();
            double[] tmp = new double[statval];
            if (numrow == -1)
            {
                for (int i = 0; i < numtest; i++)
                {
                    for (int j = 0; j < statval; j++)
                    {
                        tmp[j] = testData[j, i];
                    }
                    int t = Classify(tmp, trainData, 15, k);
                    if (t != testData[statval, i])
                        errortest.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < numtest; i++)
                {
                    for (int j = 0; j < statval; j++)
                    {
                        tmp[j] = testData[j, i];
                    }
                    int t = Classify(tmp, trainData, 15, 1);
                    if (t != testData[statval, i])
                        errortest.Add(i);
                }
            }
            return errortest;
        }
        /// <summary>
        /// Метод классификации
        /// </summary>
        /// <param name="unknown">Вектор с неизвестным классом</param>
        /// <param name="trainData">Обучающая выборка</param>
        /// <param name="numClasses">Количество классов</param>
        /// <param name="k">Количество соседей</param>
        /// <returns>Номер класса</returns>
        int Classify(double[] unknown, double[,] trainData, int numClasses, int k)
        {
            IndexAndDistance[] info = new IndexAndDistance[numtich];
            double[] data = new double[statval];
            for (int i = 0; i < numtich; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                for (int j = 0; j < statval; j++)
                    data[j] = trainData[j, i];
                curr.idx = i;
                curr.distance = Distance(unknown, data);
                info[i] = curr;
            }
            Array.Sort(info);  // sort by distance
            int result = Vote(info, trainData, numClasses, k);
            return result;
        }
        /// <summary>
        /// Выбор соседей из списка по необходимому количестве
        /// </summary>
        /// <param name="info">Массив индексов векторов с евклидовым расстоянием</param>
        /// <param name="trainData">Обучающая выборка</param>
        /// <param name="numClasses">Количество классов</param>
        /// <param name="k">Количество соседей</param>
        /// <returns>Класс с наибольшим количеством соседей</returns>
        int Vote(IndexAndDistance[] info, double[,] trainData, int numClasses, int k)
        {
            int[] votes = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; ++i) // Just first k
            {
                int idx = info[i].idx;            // Which train item
                int c = (int)trainData[statval, idx];   // Class in last cell
                ++votes[c];
            }
            int mostVotes = -1;
            int classWithMostVotes = -1;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }
        /// <summary>
        /// Дистанция (Евклидовое)
        /// </summary>
        /// <param name="unknown">Вектор с неизвестным классом</param>
        /// <param name="data">Вектор из обучабщей выборки</param>
        /// <returns>Дистанция между значениями векторов</returns>
        double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; ++i)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
    }
    /// <summary>
    /// Класс для хранения индексов и дистанций
    /// </summary>
    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        #region Перменные для записи
        public int idx;
        public double distance;
        #endregion
        public int CompareTo(IndexAndDistance other)
        {
            if (this.distance < other.distance) return -1;
            else if (this.distance > other.distance) return +1;
            else return 0;
        }
    }
}
