using System;
using System.IO;
using System.Threading;

namespace Lab_10__1_
{
    class Program
    {
        public struct comparison
        {
            public long Comparisons;
            public long Swaps;
            public TimeSpan runtime;
            public string type;
        }
        static void Main(string[] args)
        {
            comparison[] comprasions = new comparison[50];
            DateTime[] time = new DateTime[2];
            int f = 0;
            int[] arr = new int[100000];
            int[] arrcopy = new int[100000];
            Random rand = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rand.Next(100000);
                arrcopy[i] = arr[i];
            }
            while (true)
            {
                long counterSwap = 0, counterComparison = 0;
                Console.WriteLine("Выберите метод:\n" + "1. Метод слиянием\n" + "2. Метод пирамидальный\n" + "3. Метод быстрый\n" + "4. Проверить массив из файла\n" + "5. Вернуть первоночальный массив\n" + "6. Время сортировок\n" + "7. Отсортеровать в порядке возрастания\n");
                string choice = Console.ReadLine();
                if (choice == "0") { break; }
                switch (choice)
                {
                    case "1":
                        MergeSort(arr,0,arr.Length-1, comprasions, f, time,counterSwap,counterComparison);
                        Console.Clear();
                        Time(time, comprasions, f);
                        Console.WriteLine("Количество перстановок:{0}\nКоличество сравнений:{1}", comprasions[f].Swaps, comprasions[f].Comparisons);
                        WriteInFile(arr);
                        f += 1;
                        break;
                    case "2":
                        PyramidSort(arr, 0, arr.Length - 1, comprasions, f, time, counterSwap, counterComparison);
                        Console.Clear();
                        Time(time, comprasions, f);
                        Console.WriteLine("Количество перстановок:{0}\nКоличество сравнений:{1}", comprasions[f].Swaps, comprasions[f].Comparisons);
                        WriteInFile(arr);
                        f += 1;
                        break;
                        case "3":
                         QuickSort(arr,0,arr.Length-1, comprasions, f, time,counterSwap,counterComparison);
                         Console.Clear();
                         Time(time, comprasions, f);
                         Console.WriteLine("Количество перстановок:{0}\nКоличество сравнений:{1}", comprasions[f].Swaps, comprasions[f].Comparisons);
                         WriteInFile(arr);
                         f += 1;
                         break;
                    case "4":
                        CheckSort();
                        break;
                    case "5":
                        for (int i = 0; i < arr.Length - 1; i++)
                        {
                            arr[i] = arrcopy[i];
                        }
                        WriteInFile(arr);
                        break;
                    case "6":
                        ShowRuntime(comprasions, f);
                        break;
                    case "7":
                        Array.Reverse(arr);
                        Console.WriteLine("Масив отсартирован в обратном порядке.");
                        WriteInFile(arr);
                        break;
                }
            }
        }
        static int[] MergeSort(int[] arr, int left, int right, comparison[] comp, int f ,DateTime[] time,long counterSwap,long counterComparison)
        {
            if (right <= left)
            {
                return arr;
            }
            time[0] = DateTime.Now;
            int mid = (left + right) / 2;
            MergeSort(arr, left, mid, comp, f,time,counterSwap,counterComparison);
            MergeSort(arr, mid + 1, right,comp, f,time,counterSwap,counterComparison);
            Merge(arr, left, mid, right, ref counterComparison, ref counterSwap);
            time[1] = DateTime.Now;
            comp[f].Comparisons = counterComparison;
            comp[f].Swaps = counterSwap;
            comp[f].type = "Метод слиянием";
            return arr;
        }
        static void Merge(int[] arr, int left, int mid, int right, ref long counterComparison, ref long counterSwap)
        {
            int[] temp = new int[right - left + 1];

            int i = left, j = mid + 1;
            int k = 0;

            for (k = 0; k < temp.Length; k++)
            {
                counterComparison++;
                if (i > mid)
                {
                    temp[k] = arr[j++];
                }
                else if (j > right)
                {
                    temp[k] = arr[i++];
                }
                else
                {
                    counterComparison++;
                    if (arr[i] > arr[j]) 
                    {
                        temp[k] = arr[i++];
                    }
                    else
                    {
                        temp[k] = arr[j++];
                    }
                }
            }
            k = 0;
            i = left;
            while (k < temp.Length && i <= right)
            {
                counterSwap++;
                arr[i++] = temp[k++];
            }
        }
        public static int[] PyramidSort(int[] arr, int left, int right, comparison[] comp, int f, DateTime[] time, long counterSwap, long counterComparison)
        {
            time[0] = DateTime.Now;
            int N = right - left + 1;
            for (int i = right; i >= left; i--)
            {
                Heapify(arr, i, N, ref counterComparison, ref counterSwap);
            }
            while (N > 0)
            {
                counterSwap++;
                Swap(ref arr[left], ref arr[N - 1]);
                Heapify(arr, left, --N, ref counterComparison, ref counterSwap);
            }
            time[1] = DateTime.Now;
            comp[f].Comparisons = counterComparison;
            comp[f].Swaps = counterSwap;
            comp[f].type = "Метод пирамидальный";
            return arr;
        }

        public static void Heapify(int[] arr, int i, int N, ref long counterComparison, ref long counterSwap)
        {
            while (2 * i + 1 < N)
            {
                int k = 2 * i + 1;
                counterComparison++;
                if (2 * i + 2 < N && arr[2 * i + 2] <= arr[k]) 
                {
                    k = 2 * i + 2;
                }
                counterComparison++;
                if (arr[i] > arr[k])
                {
                    counterSwap++;
                    Swap(ref arr[i], ref arr[k]);
                    i = k;
                }
                else
                {
                    break;
                }
            }
        }
        public static int[] QuickSort(int[] arr, int left, int right, comparison[] comp, int f, DateTime[] time, long counterSwap, long counterComparison)
        {
            if (right <= left)
            {
                return arr;
            }
            time[0] = DateTime.Now;
            int partition = Partition(arr, left, right, ref counterComparison, ref counterSwap);
            QuickSort(arr, left, partition - 1,comp,f,time, counterSwap, counterComparison);
            QuickSort(arr, partition + 1, right, comp, f, time, counterSwap, counterComparison);
            time[1] = DateTime.Now;
            comp[f].Comparisons = counterComparison;
            comp[f].Swaps = counterSwap;
            comp[f].type = "Метод быстрый";
            return arr;
        }

        public static int Partition(int[] arr, int left, int right, ref long counterSwap,ref long counterComparison)
        {
            int pivot = arr[right];
            int i = left - 1, j = right;
            while (i < j)
            {
                while (arr[++i] > pivot) ;
                while (arr[--j] < pivot)
                {
                    counterComparison++;
                    if (j == left)
                        break;
                }
                counterComparison++;
                if (i < j)
                {
                    counterSwap++;
                    Swap(ref arr[i], ref arr[j]);
                }
                else
                {
                    break;
                }
            }
            counterSwap++;
            Swap(ref arr[i], ref arr[right]);
            return i;
        }
        static void CheckSort()
        {
            int[] arr = new int[100000];
            StreamReader readFromFile = new StreamReader(File.Open(@"C:\Users\MSI\source\repos\Lab10 (1)\Lab10 (1)\Properties\sorted.txt", FileMode.Open));
            int x = 0;
            int lengthOfArray = arr.Length;
            for (string line; (line = readFromFile.ReadLine()) != null && x < lengthOfArray; x++)
            {
                arr[x] = Convert.ToInt32(line);
            }
            readFromFile.Close();

            bool S = true;
            for (int i = 0; i < lengthOfArray - 1; i++)
            {
                if (arr[i] < arr[i + 1])
                {
                    S = false;
                    break;
                }
            }
            if (S)
            {
                Console.WriteLine("Массив отсортирован");
            }
            else
            {
                Console.WriteLine("Массив не отсортирован");
            }
        }
        static void ShowRuntime(comparison[] comp, int f)
        {
            for (int i = 0; i < f; i++)
            {
                Console.WriteLine(comp[i].runtime + "-" + comp[i].type);
            }
        }
        static void Time(DateTime[] arr, comparison[] comp, int f)
        {
            TimeSpan Diff = new TimeSpan(00, 00, 00);
            for (int i = 0; i < 2; i++)
            {
                if (i + 1 == 2) { continue; }
                Diff = arr[i + 1].Subtract(arr[i]);
                Console.WriteLine(Diff);
            }
            comp[f].runtime = Diff;
        }
        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        static void WriteInFile(int[] arr)
        {
            using (StreamWriter WrireinFile = new StreamWriter(@"C:\Users\MSI\source\repos\Lab10 (1)\Lab10 (1)\Properties\sorted.txt", true, System.Text.Encoding.Default))
            {
                foreach (int selection in arr)
                {
                    WrireinFile.Write(selection + " ");
                }
                WrireinFile.WriteLine();
            }
        }
    }
}