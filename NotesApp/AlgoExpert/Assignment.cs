using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotesApp.AlgoExpert
{
    public class Singleton
    {
        private static Singleton instance = null;
        private static readonly object lockObj = new object();
        
        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get {
                lock (lockObj) {
                    if (instance == null) {
                        instance = new Singleton();
                    }
                }
                return instance;
            }
        }


    }


    public class Assignment
    {
        public static int Gap(int num)
        {
            int result = 0;

            string asBins = Convert.ToString(num, 2);
            string pattern = @"0+";

            MatchCollection matches = Regex.Matches(asBins, pattern);
            foreach (Match match in matches) {
                // Console.WriteLine(match.Value);
                result = Math.Max(result, match.Value.Length);
            }

            return result;
        }



        public static int find_it(int[] seq)
        {
            var distinctNums = seq.Distinct();
            foreach (var num in distinctNums) {
                int count = seq.Count(x => x == num);
                if (count % 2 != 0)
                    return num;
            }
            
            return 0;
        }


        public static int GetLongestPalindrome(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            char[] revArray = str.ToCharArray();
            Array.Reverse(revArray);
            string revString = new string(revArray);


            int longestPalindrom = 0;
            int currentPalindrom = 0;

            for (int i = 0; i < str.Length; i++) {
                int startIndex = revString.IndexOf(str[i]);

                if (startIndex >= 0) {
                    for (int j = startIndex; j < revString.Length; j++) {
                        if ((i+currentPalindrom) < str.Length && str[i + currentPalindrom] == revArray[j]) {
                            currentPalindrom++;
                        } else
                            break;
                    }
                }

                if (currentPalindrom > longestPalindrom) {
                    longestPalindrom = currentPalindrom;
                }
                currentPalindrom = 0;
            }

            return longestPalindrom;
        }



        public static List<int> RiverSizes(int[,] matrix)
        {
            List<int> sizes = new List<int>();
            bool[,] visitedMatrix = new bool[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i= 0; i < matrix.GetLength(0); i++) {
                for (int j=0; j < matrix.GetLength(1); j++) {
                    if (visitedMatrix[i, j])
                        continue;
                    traverseNode(i, j, matrix, visitedMatrix, sizes);
                }
            }

            return sizes;
        }

        private static void traverseNode(int i, int j, int[,] matrix, bool[,] visitedMatrix, List<int> sizes)
        {
            int currentRiverSize = 0;
            List<int[]> nodesToExplore = new List<int[]>();
            nodesToExplore.Add(new int[] { i, j });
            while (nodesToExplore.Count > 0) {
                int[] currentNode = nodesToExplore[nodesToExplore.Count - 1];
                nodesToExplore.RemoveAt(nodesToExplore.Count - 1);

                i = currentNode[0];
                j = currentNode[1];

                if (visitedMatrix[i, j])
                    continue;
                visitedMatrix[i, j] = true;

                if (matrix[i, j] == 0)
                    continue;

                currentRiverSize++;

                CheckNeighbours(i, j, matrix, visitedMatrix, nodesToExplore);
            }

            if (currentRiverSize > 0)
                sizes.Add(currentRiverSize);
        }

        private static void CheckNeighbours(int i, int j, int[,] matrix, bool[,] visitedMatrix, List<int[]> nodesToExplore)
        {
            // upper neighbour
            if (j > 0 && !visitedMatrix[i, j-1]) {
                nodesToExplore.Add(new int[] { i, j - 1 });
            }
            // lower neighbour
            if (j < matrix.GetLength(1) - 1 && !visitedMatrix[i, j + 1]) {
                nodesToExplore.Add(new int[] { i, j + 1 });
            }
            // left neighbour
            if (i > 0 && !visitedMatrix[i - 1, j]) {
                nodesToExplore.Add(new int[] { i - 1, j});
            }
            // right neighbour
            if (i < matrix.GetLength(0) - 1 && !visitedMatrix[i + 1, j]) {
                nodesToExplore.Add(new int[] { i + 1, j});
            }
        }

        public void Init()
        {
            /*
            //            int[] largest = new int[] { 1, 11, 3, 0, 15, 5, 2, 4, 10, 7, 12, 6 };
                        int[] largest = new int[] { 19, -1,
                            18, 17, 2,
                            10, 3, 12,
                            5, 16, 4,
                            11, 8, 7,
                            6, 15, 12,
                            12, 2, 1,
                            6, 13,
                            14
                            };

                            LargestRange(largest);
            */
        }

        /*
                public int[] LargestRange(int[] array)
                {
                    Dictionary<int, bool> myDict = new Dictionary<int, bool>();
                    int lowerRange = 0;
                    int upperRange = 0;

                    foreach (var num in array) {
                        if (!myDict.ContainsKey(num))
                            myDict.Add(num, false);
                    }

                    foreach (var num in array) {
                        if (myDict[num] == true) {
                            continue;
                        }
                        else {
                            myDict[num] = true;
                            int leftnumber = num;
                            while (myDict.ContainsKey(leftnumber)) {
                                myDict[leftnumber] = true;
                                leftnumber--;
                            }
                            int rightnumber = num;
                            while (myDict.ContainsKey(rightnumber)) {
                                myDict[rightnumber] = true;
                                rightnumber++;
                            }
                            leftnumber++;
                            rightnumber--;

                            if ((rightnumber - leftnumber) > (upperRange - lowerRange)) {
                                upperRange = rightnumber;
                                lowerRange = leftnumber;
                            }
                        }
                    }

                    var list = myDict.Keys.ToList();
                    list.Sort();

                    // Loop through keys.
                    foreach (var key in list) {
                        Console.WriteLine("{0}: {1}", key, myDict[key]);
                    }


                    Console.WriteLine($"Solution is {lowerRange},{upperRange}");

                    return new int[] {lowerRange, upperRange};
                }
                */


        //public int[] LargestRange(int[] array)
        //{
        //    Array.Sort(array);
        //    Console.WriteLine(String.Join(",", array));

        //    int curLen = 0;
        //    int curMaxLen = 0;
        //    int lowerCandidate = 0;
        //    int upperCandidate = 0;

        //    for (int i = 0; i < array.Length - 1; i++) {
        //        curLen = 0;
        //        int lastUpper = 0;

        //        for(int j = i+1; j < array.Length; j++) {
        //            if (array[j] == array[j - 1])
        //                continue;
        //            if (array[j] == array[j-1] + 1) {
        //                curLen++;
        //                lastUpper = array[j];
        //            }
        //            else {
        //                break;
        //            }
        //        }
        //        if (curLen > curMaxLen) {
        //            curMaxLen = curLen;
        //            lowerCandidate = array[i];
        //            upperCandidate = lastUpper;
        //        }
        //    }

        //    Console.WriteLine($"Result: ({lowerCandidate},{upperCandidate})");
        //    int[] result = new int[] { lowerCandidate, upperCandidate };
        //    return result;
        //}



    }
}
