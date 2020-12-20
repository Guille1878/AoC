using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApp2
{
    class Program
    {
        static List<string> props = new List<string>
            {
                "byr","iyr","eyr","hgt","hcl","ecl","pid","cid"
            };
        static List<string> eyeColors = new List<string>
            {
                "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
            };

        static char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static bool IsEachCharValidNumber(string text)
        {
            text = text.Trim();
            if (text.Length == 9)
            {
                return !text.Any(c => !numbers.Contains(c));
            }

            return false;
        }

        static DateTime started = DateTime.Now;
        static StreamWriter sr = new StreamWriter(@"C:\Guille\day13_C.txt", true);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            sr.WriteLine("==================");
            sr.WriteLine($"Started: {started}");
            WriteLine("==================");
            WriteLine($"Started: {started}");

            try
            {
                //WriteLine(string.Join(',',  GetFactors2(5876813119)));
                //WriteLine(string.Join(',',  GetFactors2(37)));
                //WriteLine(Hcf(66193, 47));
                WriteLine(LargestPrimeFactorOf(48));

                //Day10();
                //Day13();
                //Day14();

                sr.WriteLine($"Ended: {DateTime.Now}. Duration: {DateTime.Now - started}");
                WriteLine($"Ended: {DateTime.Now}. Duration: {DateTime.Now - started}");
            }
            catch (Exception ex)
            {
                sr.WriteLine(ex.ToString());
                WriteLine(ex.ToString());
            }
            finally
            {
                sr.Close();
                sr.Dispose();
            }
        }

        static IEnumerable<long> GetFactors2(long n)
        {
            var list = new HashSet<long>();
            for (long index = 1; index < n; index++)
            {
                if (n % index == 0)
                    list.Add(index);
            }
            return list;
        }

        //public static int Hcf(int n1, int n2)
        //{
        //    if (n2 != 0)
        //        return Hcf(n2, n1 % n2);
        //    else
        //        return n1;
        //}

        public static long Factorial_Recursion(int number)
        {
            long i, fact;
            fact = number;
            for (i = number - 1; i >= 1; i--)
            {
                fact = fact * i;
            }
            return fact;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            sr.WriteLine($"Ended: {DateTime.Now}. Duration: {DateTime.Now - started}");
            sr.Close();
            sr.Dispose();
        }

        #region day14

        private static void Day14()
        {
            var list = Puzzles.PuzzleList;
            //var list = Puzzles.IntroList;

            var bits36 = new List<Tuple<string, Tuple<int,long>[]>>();
            var current = new List<Tuple<int, long>>();
            var mask = "";
            var memory = new Dictionary<long, long>();

            //var vad1 = GetAllPossibleBits(2);
            //var vad2 = GetAllPossibleBits(3);
            //var vad3 = GetAllPossibleBits(4);

            foreach (var item in list)
            {
                if (item.StartsWith("mask"))
                {
                    if (mask != "")
                    {
                        bits36.Add(new Tuple<string, Tuple<int, long>[]> (mask, current.ToArray()));
                        current.Clear();
                    }
                    mask = item[7..];

                }
                else
                {
                    var memNum = int.Parse(item[(item.IndexOf('[') + 1)..item.IndexOf(']')]);
                    var num = long.Parse(item[(item.IndexOf('=') + 2)..]);
                    current.Add(new Tuple<int, long>(memNum, num));
                }
            }
            bits36.Add(new Tuple<string, Tuple<int, long>[]>(mask, current.ToArray()));

            foreach (var block in bits36)
            {
                foreach (var bitNum in block.Item2)
                {
                    var address = convertToBit36(bitNum.Item1);
                    var address1Changed = applyMask1(block.Item1, address);

                    var vad = GetAllPossibleMasks(block.Item1, address1Changed);
                    foreach (var possAddr in GetAllPossibleMasks(block.Item1, address1Changed))
                    {
                        if (memory.ContainsKey(possAddr))
                            memory[possAddr] = bitNum.Item2;
                        else
                            memory.Add(possAddr, bitNum.Item2);

                    };
                }
            }

            WriteLine($"Sum: { memory.Values.Sum() }");

        }

        static long[] GetAllPossibleMasks(string mask, string bitValue)
        {
            var xCount = mask.Count(b => b == 'X');
            var addresses = new HashSet<long>();

            foreach (var rootMask in GetAllPossibleBits(xCount))
            {
                var newAdr = "";
                var bitAddrIndex = 0;
                for (int index = 0; index < bitValue.Length; index++)
                {
                    newAdr += (bitValue[index] == 'X') ? rootMask[bitAddrIndex++] : bitValue[index] == '1' ? '1' : bitValue[index];
                }
                addresses.Add(convertFromBit36(newAdr));
            }
            
            return addresses.ToArray();
        }

        static string[] GetAllPossibleBits(int len)
        {
            var ways = new HashSet<string>();
            int turn = 0;
            bool trying0 = false;

            var max = Convert.ToInt64("".PadLeft(len, '1'), 2);
            for (int index = 0; index < Convert.ToInt64("".PadLeft(len, '1'), 2); index++)
            {
                ways.Add(Convert.ToString(index, 2).PadLeft(len, '0'));
            }
            ways.Add("".PadLeft(len, '1'));

            //ways.Add("".PadRight(len,'0'));
            //do
            //{
            //    var newBit = "".PadRight(turn, (trying0 ? '0' : '1'));
            //    for (int index = turn; index < len; index++)
            //    {
            //        //var newRow = newBit + "".PadRight(index - turn, trying0 ? '1' : '0') + (trying0 ? '0' : '1') + "".PadRight(len - index - 1, trying0 ? '1' : '0');
            //        ways.Add(newBit + "".PadRight(index - turn, trying0 ? '1' : '0') + (trying0 ? '0' : '1') + "".PadRight(len - index - 1, trying0 ? '1' : '0'));
            //    }
            //    turn++;

            //    if (turn == len)
            //    {
            //        if (trying0)
            //            break;
            //        else
            //        {
            //            trying0 = true;
            //            turn = 0;
            //        }
            //    }

            //} while (true); //(ways.Count < len * len);

            return ways.ToArray();
        }


        static string applyMask1(string mask, string bitValue)
        {
            var newBit = "";
            for (int index = 0; index < bitValue.Length; index++)
            {
                newBit += mask[index] == '1' ? '1' : mask[index] == 'X' ? mask[index] : bitValue[index];
            }
            return newBit;
        }
        static string applyMask(string mask, string bitValue)
        {
            var newBit = "";
            for (int index = 0; index < bitValue.Length; index++)
            {
                newBit += mask[index] == 'X' ? bitValue[index] : mask[index];
            }
            return newBit;
        }

        static string convertToBit36(long num)
        {
            return Convert.ToString(num, 2).PadLeft(36,'0');
        }

        static long convertFromBit36(string num)
        {
            return Convert.ToInt64(num, 2);
        }

        #endregion day14




        #region day13

        static long hits = 0;
        static long closed = 0;
        static bool done = false;
        //static string[] list = Puzzles.PuzzleList.ToArray();
        static string[] list = Puzzles.IntroList.ToArray();

        static Tuple<int, int>[] busses;
        static Tuple<int, int>[] sortedBusses;


        static  long bussesCount;
        static  long allBussesCount;
        static long sortedBussesCount;

        static long firstBusId;
        static Tuple<int, int> largestBus;
        static Tuple<int, int> nextLargestBus;

        static long showHits = 0;

        private static void Day13()
        {
            busses = list[1].Split(",").Select((b, i) => new Tuple<string, int>(b, i)).Where(b => b.Item1 != "x").Select(b => new Tuple<int, int>(int.Parse(b.Item1), b.Item2)).ToArray();
            sortedBusses = busses[1..].OrderByDescending(b => b.Item1).ToArray();
            largestBus = busses.First(l => l.Item1 == busses.Max(i => i.Item1));
            nextLargestBus = busses.First(l => l.Item1 == busses.Where(v => v.Item1 < largestBus.Item1).Max(i => i.Item1));

            bussesCount = busses.Length;
            allBussesCount = list.Length;
            sortedBussesCount = sortedBusses.Length;
            firstBusId = busses[0].Item1;

            var parralellProc = 1;

            long min = 0; // 28137811179; // 113335045207669; // 113335045207669; //0; // 100000000000001;

            long time = min;

            long max = 1;
            foreach (var item in busses)
            {
                max *= item.Item1;
            }
            var interval = max - min;

            var tick = interval / parralellProc;

            if (tick < firstBusId)
                tick = firstBusId + 1;

            sr.WriteLine($"mul: {max}  diff: { interval} tick: {tick} ");

            var startTimes = new List<Tuple<long,long>>();
            for (long sTime = min; sTime < max; sTime += tick)
            {
                var diff = sTime % firstBusId;
                sTime -= diff;

                var maxN = sTime + tick + diff;
                var diff2 = maxN % firstBusId;

                maxN += (firstBusId - diff2) + firstBusId;


                startTimes.Add(new Tuple<long, long>(sTime, maxN));
            }

            showHits = firstBusId * largestBus.Item1 * nextLargestBus.Item1 * 1000;

            Parallel.ForEach(startTimes, startTime => runProcess(startTime.Item1, startTime.Item2));

        }

        public static int Hcf(int n1, int n2)
        {
            if (n2 != 0)
                return Hcf(n2, n1 % n2);
            else
                return n1;
        }
        static long LargestPrimeFactorOf(long n)
        {
            long lastPrimeFactor = 0;

            for (long i = 2; i < n; i++)
            {
                if (isPrime(i) && n % i == 0)
                {
                    lastPrimeFactor = i;
                    Console.WriteLine(i + " is a prime factor of " + n);
                }
            }

            return lastPrimeFactor;
        }

        static bool isPrime(long n)
        {
            if (n == 2) return true;
            if ((n > 2 && n % 2 == 0) || n == 1) return false;

            for (long i = 2; i <= Math.Floor(Math.Sqrt(n)); ++i)
            {
                if (n % i == 0) return false;
            }

            return true;
        }
        static void runProcess2(long rTime, long max)
        {

            do
            {
                var gotOut = false;
                foreach (var sortedBus in sortedBusses)
                {
                    if ((sortedBus.Item2 + rTime) % sortedBus.Item1 != 0)
                    {
                        gotOut = true;
                        break;
                    }                    
                }

                if (!gotOut)
                {
                    WriteLine($"===============");
                    WriteLine($"T-time: {rTime} Duration: {DateTime.Now - started}");
                    WriteLine($"===============");
                    return;
                }

                hits++;
                rTime += firstBusId;
                //rTime--;

                if (rTime % (showHits) == 0) //100000000
                    WriteLine($"--> hits: {hits} closed: {closed} time: {rTime} -> Duration: {DateTime.Now - started}");

                if (done)
                {
                    return;
                }

                if (rTime > max)
                {
                    closed++;
                    WriteLine($"Done in time {rTime}");
                    return;
                }

            } while (true);

        }

        static void runProcess(long rTime, long max)
        {

            do
            {
                hits++;
                rTime += firstBusId;

                if (rTime % (showHits) == 0)
                    WriteLine($"--> hits: {hits} closed: {closed} time: {rTime} -> Duration: {DateTime.Now - started}");


                if (CheckIfAllT3(rTime, 0))
                {
                    sr.WriteLine($"===============");
                    sr.WriteLine($"T-time: {rTime} Duration: {DateTime.Now - started}");
                    WriteLine($"T-time: {rTime} Duration: {DateTime.Now - started}");
                    sr.WriteLine($"===============");
                    return;
                }

                if (rTime > max)
                {
                    closed++;
                    sr.WriteLine($"Done in time {rTime}");
                    return;
                }

            } while (true);

        }

        static bool CheckIfAllT3(long inTime, int index)
        {
            int busId = sortedBusses[index].Item1;
            int busIndex = sortedBusses[index].Item2;

            if ((inTime + busIndex) % busId == 0)
            {
                if (index == sortedBussesCount - 1)
                    return true;

                return (CheckIfAllT(inTime, index + 1));

            }
            return false;
        }

        static bool CheckIfAllT2(long inTime, int index, int skip)
        {
            if (skip == index)
                return (CheckIfAllT(inTime, index + 1));

            int busId = busses[index].Item1;
            int busIndex = busses[index].Item2;

            if ((inTime + busIndex) % busId == 0)
            {
                if (index == bussesCount - 1)
                    return true;

                return (CheckIfAllT(inTime, index + 1));

            }
            return false;
        }


        static bool CheckIfAllT(long inTime, int index)
        {
            int busId = busses[index].Item1;
            int busIndex = busses[index].Item2;

            if ((inTime + busIndex) % busId == 0)
            {
                if (index == bussesCount - 1)
                    return true;

                return (CheckIfAllT(inTime, index + 1));

            }
            return false;
        }

        #endregion day13

        #region day12
        private static void Day12()
        {

            var wayTimes = new int[4,2]
            {
                { 1, 1 },
                { -1, 1 },
                { -1, -1 },
                { -1, 1 }
            };

            var wayPosIndex = 0;

            string dirs = "ESWN";
            var startedPosX = 10;
            var startedposY = 1;

            var sheepPosX = 0;
            var sheepPosY = 0;

            var posX = startedPosX;
            var posY = startedposY;
            var dir = 'E';
            
            var listMain = Puzzles.PuzzleList;

            foreach (var item in listMain)
            {
                
                var val = int.Parse(item[1..]);

                switch (item[0])
                {
                    case 'F':
                        sheepPosX += (posX * val);
                        sheepPosY += (posY * val);
                        //sheepPosX += (posX * val * wayTimes[wayPosIndex, 0]);
                        //sheepPosY += (posY * val * wayTimes[wayPosIndex, 1]);
                        break;

                    case 'R':
                        changeDir(1, val);
                        break;
                    case 'L':
                        changeDir(-1, val);
                        break;

                    default:
                        moveWayPoint(item[0], val);
                        break;
                }
            }

            WriteLine($"X {sheepPosX} y {sheepPosY} sum { Math.Abs(sheepPosX) + Math.Abs(sheepPosY) }");


            void changeDir(int toValue, int value)
            {
                var currentX = posX;

                if (toValue == -1 && value == 90)
                    value = 270;
                else if (toValue == -1 && value == 270)
                    value = 90;

                switch (value)
                {
                    case 90:
                        posX = posY;
                        posY = currentX * -1;
                        break;
                    case 180:
                        posX *= -1;
                        posY *= -1;
                        break;
                    case 270:
                        posX = posY * -1;
                        posY = currentX;
                        break;
                    default:
                        break;
                }


                

                //var dig = value / 90;

                //wayPosIndex += (toValue * dig);

                //if (wayPosIndex < 0)
                //    wayPosIndex += 4;

                //if (wayPosIndex > 3)
                //    wayPosIndex -= 4;

                //dir = dirs[wayPosIndex];

                //if (wayPosIndex == 0)
                //    return;

                //if (wayPosIndex % 2 == 1)
                //{
                //    var currentX = posX;
                //    posX = posY * wayTimes[wayPosIndex, 1];
                //    posY = currentX * wayTimes[wayPosIndex, 0];
                //    return;
                //}
                
                //posX *= wayTimes[wayPosIndex, 0];
                //posY *= wayTimes[wayPosIndex, 1];
            }
                   

            void moveWayPoint(char dir, int value)
            {
                switch (dir)
                {
                    case 'N':
                        posY += value;
                        break;
                    case 'S':
                        posY -= value;
                        break;
                    case 'E':
                        posX += value;
                        break;
                    case 'W':
                        posX -= value;
                        break;
                }
            }

        }

        static long counter = 0;
        
        private static short lastItem;
        private static short firstItem;
        private static char[] _changable;
        private static short column = 0;
        //private static Dictionary<long, HashSet<string>> _arragement;
        private static Dictionary<short, Dictionary<short, HashSet<string>>> _arragement;

        private static void Day10()
        {
            var listMain = Puzzles.IntroList.Select(n => (char)int.Parse(n)).OrderBy(n => n).ToList();
            //var listMain = Puzzles.PuzzleList.Select(n => (char)short.Parse(n)).OrderBy(n => n).ToList();

            listMain.Insert(0, (char)0);
            var list = listMain.ToArray();


            long charger = 0;

            var nonChange = new List<char>();

            for (int index = 0; index < list.Length; index++)
            {
                var len = list.Length < index + 3 ? list.Length : index + 3;

                var item = list[index];
                var diffe = list[index..len]?.Min(n => n) ?? 0;

                if (diffe - charger == 3)
                    nonChange.Add(item);

                charger = item;
            }

            var errors = 0;

            _changable = list.Except(nonChange).ToArray()[1..^1];

            nonChange.Clear();

            var loopStarted = DateTime.Now;
            WriteLine($"Looped started: : {loopStarted}");
            //sr.WriteLine($"Looped started: : {loopStarted}");

            addArrang(list.ToArray(), 0);

            WriteLine($"Looped ended: {DateTime.Now}. Duration: {DateTime.Now - loopStarted}");
          //  sr.WriteLine($"Looped ended: {DateTime.Now}. Duration: {DateTime.Now - loopStarted}");

            WriteLine($"hits: {counter}");
            //sr.WriteLine($"hits: {counter}");

            void addArrang(char[] list, int pIndex)
            {
                //if (!correctSep(list))
                //{
                //    //errors++;
                //    return;
                //}

                counter++;

                if (counter % 1000000 == 0)
                    WriteLine($"hits: {counter} Duration: {DateTime.Now - started}");

                for (int index = pIndex; index < list.Length - 1; index++)
                    if (_changable.Contains(list[index]))
                        if (list[index + 1] - list[index - 1] < 4)
                            addArrang(list.Where((v, i) => i != index).ToArray(), index);
            }

            //bool correctSep(char[] list)
            //{
            //    for (int i = 1; i < list.Length; i++)
            //        if (list[i] - list[i - 1] > 3)
            //            return false;

            //    return true;
            //}

        }


        static long getDiff(long charger, long number)
        {
            if (number - charger == 1)
            {
                return 1;
            }
            else if (number - charger == 3)
            {
                return 3;
            }
            else
                return 0;
        }
        #endregion day12

        #region old days

        private static void Day9()
        {
            var isInPreamble = false;
            var preambleIndex = 25;
            long bug = 0;
            var bugIndex = 0;

            var list = Puzzles.PuzzleList.Select(n => long.Parse(n)).ToList();

            for (int i = preambleIndex; i < list.Count; i++)
            {
                var item = list[i];
                isInPreamble = false;

                for (int j = i - preambleIndex; j < i; j++)
                {
                    var prim = list[j];
                    for (int x = i - preambleIndex; x < i; x++)
                    {
                        if (x == j)
                            continue;

                        var secn = list[x];
                        if (item == prim + secn)
                        {
                            isInPreamble = true;
                            break;
                        }
                    }
                    if (isInPreamble)
                        break;
                }
                
                if (isInPreamble)
                    continue;
                else if (!isInPreamble)
                { 
                    bug = list[i];
                    bugIndex = i;
                    break;
                }
            }

            Console.WriteLine($"bug: {bug} pre: {preambleIndex}");

            for (int i = 0; i < bugIndex; i++)
            {
                var item = list[i];
                var numbersIn = new List<long>();
                long sum = 0;

                for (int j = i; j < list.Count; j++)
                {
                    numbersIn.Add(list[j]);
                    sum += list[j];
                    if (sum == bug)
                    {
                        Console.WriteLine($"small: {numbersIn.Min()} largest: {numbersIn.Max()} adding: {numbersIn.Min() + numbersIn.Max()}");
                        return;
                    } 
                    else if (sum > bug)
                    {
                        numbersIn.Clear();
                        sum = 0;
                        break;
                    }
                       
                }
            }           

        }

        static void Day8()
        {

            var list = Puzzles.PuzzleList;
            var totalSteps = 0;
            var index = 0;
            
            var indexList = new List<int>();

            var tried = new List<int>();
            var traing = -1;

            while (index < list.Count) 
            {
                if (indexList.Contains(index))
                {
                    tried.Add(traing);
                    traing = -1;
                    index = 0;
                    indexList.Clear();
                    totalSteps = 0;
                    continue;
                }

                indexList.Add(index);

                var command = list[index][..3];
                var steps = int.Parse(list[index][4..]);

                switch (command)
                {
                    case "acc":
                        index++;
                        totalSteps += steps;
                        break;

                    case "jmp":
                        
                        if ((!tried.Any(t => t == index)) && traing == -1)
                        {
                            traing = index;
                            index++;
                        }
                        else
                            index += steps;
                        break;

                    case "nop":
                        if (!tried.Any(t => t == index) && steps != 0 && traing == -1)
                        {
                            traing = index;
                            index += steps;
                        }
                        else
                            index++;
                        
                        break;
                }

            } 

            Console.WriteLine(totalSteps);

        }

        static void Day7()
        {
            var bagText = "shiny gold";

            Console.WriteLine(CountBags(bagText));

            


            // part 1
            var list = Puzzles.PuzzleList;

            var myBags = new List<string>()
            {
                bagText
            };

            myBags.AddRange(list.Where(i => i.Split("contain")[1].Contains(bagText)));


            while (myBags.Count < list.Count * 100)
            {
                var moreBags = new List<string>();
                foreach (var item in myBags)
                {
                    var bag = (item.Contains("contain")) ?
                        item.Split("contain")[0].Replace("bags", "").Replace("bag", "").Trim() : item;

                    //moreBags.AddRange(list.Where(i => i.Split("contain")[1].Contains(bag) && !i.Split("contain")[0].Contains(bagText)));
                    moreBags.AddRange(list.Where(i => i.Split("contain")[1].Contains(bag)));
                }
                myBags.AddRange(moreBags);
            }



            //var more = new List<string>();

            //foreach (var item in allBags)
            //{
            //    var bag = (item.Contains("contain")) ? item.Split("contain")[0].Replace("bags", "").Replace("bag", "").Trim() : item;

            //    more.AddRange(list.Where(i => i.Split("contain")[1].Contains(bag) && !i.Split("contain")[0].Contains(bagText)));

            //}

            //total += Puzzles.IntroList.Count(i => i.Split("contain")[1].Contains(bag));
            //Console.WriteLine(Puzzles.IntroList.Count(i => i.Split("contain")[1].Contains(bag)));

            var diff = myBags.Distinct();
            // 25 or 242

            Console.WriteLine(diff.Count() - 1);
            


        }

        static int CountBags(string bagName)
        {
            bagName = bagName.Trim();

            if (string.IsNullOrEmpty(bagName))
                return 0;

            var bag = Puzzles.PuzzleList.Find(b => b.StartsWith(bagName));
            //var bag = Puzzles.IntroList.Find(b => b.StartsWith(bagName));

            var bagIn = bag.Split("contain")[1].Split(",");

            var content = bagIn.ToDictionary(v => v.Trim() == "no other bags." ? "": v.Trim().Replace(v.Trim().Split(" ")[0].Trim(), ""),
                                             k => k.Trim() == "no other bags." ? 0 : int.Parse(k.Trim().Split(" ")[0].Trim()));

            var qty = content.Sum(c => c.Value + c.Value * CountBags(c.Key.Replace(".", "").Replace("bags", "").Replace("bag", "")));

            return qty;
        }
                
        static void Day6()
        {
            var _group = "";
            var _persons = 0;
            var allyes = 0;
            var allyesList = new List<int>();
            foreach (var person in Puzzles.PuzzleList)
            {
                if (person == "")
                {
                    allyes = AnswerYes(ToGrouped(_group), _persons);

                    Console.WriteLine($"{allyes} : {_persons}");

                    _group = "";
                    _persons = 0;
                    allyesList.Add(allyes);

                }
                else
                {
                    _group += person;
                    _persons++;
                }
            }

            allyes = AnswerYes(ToGrouped(_group), _persons);

            Console.WriteLine($"{allyes} : {_persons}");

            allyesList.Add(allyes);

            Console.WriteLine(allyesList.Sum());
        }

        static void Day5()
        {
            var seats = new List<Tuple<int, int, int>>();

            var hiegestSeatId = 0;

            foreach (var seat in Puzzles.PuzzleList)
            {
                var rowId = 0;

                var fromId = 0;
                var toId = 127;
                for (var rowIndex = 0; rowIndex < 7; rowIndex++)
                {
                    if (seat[rowIndex] == 'B')
                        fromId = fromId + ((toId - fromId) / 2) + 1;

                    if (seat[rowIndex] == 'F')
                        toId = fromId + (toId - fromId) / 2;
                }
                rowId = fromId;

                fromId = 0;
                toId = 7;
                var columnId = 0;
                for (var rowIndex = 7; rowIndex < 10; rowIndex++)
                {
                    if (seat[rowIndex] == 'R')
                        fromId = fromId + ((toId - fromId) / 2) + 1;

                    if (seat[rowIndex] == 'L')
                        toId = fromId + (toId - fromId) / 2;
                }
                columnId = fromId;

                var seatId = (rowId * 8) + columnId;

                seats.Add(new Tuple<int, int, int>(rowId, columnId, seatId));

                Console.WriteLine($"row: {rowId}, column: {columnId}, seatId: {seatId}");

                if (seatId > hiegestSeatId)
                    hiegestSeatId = seatId;

            }

            Console.WriteLine($"hiegestSeatId: {hiegestSeatId}");

            var missedIds = new List<Tuple<int, int, int>>();
            for (var rowId = 0; rowId < 127; rowId++)
            {
                for (var columnId = 0; columnId < 7; columnId++)
                {
                    if (!seats.Any(s => s.Item1 == rowId && s.Item2 == columnId))
                    {
                        missedIds.Add(new Tuple<int, int, int>(rowId, columnId, (rowId * 8) + columnId));

                    }
                }
            }

            missedIds.ForEach(sId => Console.WriteLine(sId));
        }

        #endregion old days


        static Dictionary<char,int> ToGrouped(string group)
        {
            var result = new Dictionary<char, int>();

            foreach (var l in group)
            {
                if (result.ContainsKey(l))
                    result[l]++;
                else
                    result.Add(l, 1);
            }
            return result;
        }

        static int AnswerYes(Dictionary<char, int> group, int howmany)
        {
            var result = 0;
            foreach (var answer in group)
            {
                if (answer.Value == howmany)
                    result++;
            }
            return result;
        }

        static Dictionary<string,string> GetDic(string pass)
        {
            var currentprops = pass.Split(" ").Where(p => p != "");
            return currentprops.ToDictionary( p => p.Split(":")[0], p => p.Split(":")[1]);
        }

        static bool IsValid(string pass)
        {

            var currentprops = pass.Split(" ").Where(p => p != "");
            var currentkeys = currentprops.Select(p => p.Split(":")[0]);

            var count = currentkeys.Count();
            if (count < 7 || count > 8)
                return false;

            var diff = props.Except(currentkeys).ToList();

            if (!diff.Any())
                return true;
            else if (diff.Count() == 1 && diff.First() == "cid")
                return true;

            return false;

        }

        static bool IsValidEcl(string color)
        {
            return eyeColors.Any(c => c == color);
        }

        static bool IsValidByr(string year)
        {
            if (year.Length == 4)
            {
                try
                {
                    var intYear = int.Parse(year);
                    return intYear >= 1920 && intYear <= 2002;
                }
                catch 
                {
                    return false;
                }
            }
            return false;
        }

        static bool IsValidIyr(string year)
        {
            if (year.Length == 4)
            {
                try
                {
                    var intYear = int.Parse(year);
                    return intYear >= 2010 && intYear <= 2020;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        static bool IsValidEyr(string year)
        {
            if (year.Length == 4)
            {
                try
                {
                    var intYear = int.Parse(year);
                    return intYear >= 2020 && intYear <= 2030;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        static bool IsValidHgt(string text)
        {
            var twoLast = text[^2..];
            if (twoLast == "in")
            {
                try
                {
                    var intnum = int.Parse(text.Replace("in",""));
                    return intnum >= 59 && intnum <= 76;
                }
                catch
                {
                    return false;
                }
            }
            else if (twoLast == "cm")
            {
                try
                {
                    var intnum = int.Parse(text.Replace("cm", ""));
                    return intnum >= 150 && intnum <= 193;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        static char[] numbersR = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9','a','b','c','d','e','f' };
        static bool IsValidHcl(string text)
        {
            text = text.Trim();
            if (text[0] == '#')
            {
                text = text.Replace("#", "");
                if (text.Length == 6)
                {
                    return !text.Any(c => !numbersR.Contains(c));
                }
            }

            return false;
        }
    }
}
