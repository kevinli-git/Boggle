using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boggle
{
    class Program
    {
        static void Main(string[] args)
        {
            new Boggle().Solve("rtsnaeiopteleesh");

            Console.ReadLine();
        }
    }


    class Boggle
    {
        //16 dices
        readonly Dictionary<int, int[]> nextDiceList = new Dictionary<int, int[]> {
            {0, new []{1, 4, 5}},
            {1, new []{0, 2, 4, 5, 6}},
            {2, new []{1, 3, 5, 6, 7}},
            {3, new []{2, 6, 7}},
            {4, new []{0, 1, 5, 8, 9}},
            {5, new []{0, 1, 2, 4, 6, 8, 9, 10}},
            {6, new []{1, 2, 3, 5, 7, 9, 10, 11}},
            {7, new []{2, 3, 6, 10, 11}},
            {8, new []{4, 5, 9, 12, 13}},
            {9, new []{4, 5, 6, 8, 10, 12, 13, 14}},
            {10, new []{5, 6, 7, 9, 11, 13, 14, 15}},
            {11, new []{6, 7, 10, 14, 15}},
            {12, new []{8, 9, 13}},
            {13, new []{8, 9, 10, 12, 14}},
            {14, new []{9, 10, 11, 13, 15}},
            {15, new []{10, 11, 14}},
        };

        public void Solve(string dices)
        {
            SpellChecker checker = new SpellChecker();

            var chains = new List<List<int>>[16];

            for (int level = 0; level < 16; level++)
            {
                chains[level] = new List<List<int>>();

                if (level == 0)
                {
                    foreach (var key in nextDiceList.Keys)
                    {
                        var chain = new List<int>();
                        chain.Add(key);

                        //save chain for current level
                        chains[level].Add(chain);
                    }
                }
                else
                {
                    //previous level chains
                    var previouChains = chains[level - 1];
                    foreach (var chain in previouChains)
                    {
                        var nextDices = nextDiceList[chain.Last()];

                        foreach (var nextDice in nextDices)
                        {
                            if (!chain.Contains(nextDice))
                            {
                                var newChain = new List<int>(chain);
                                newChain.Add(nextDice);

                                chains[level].Add(newChain);

                                //output
                                if (newChain.Count > 2)
                                {
                                    foreach (int i in newChain)
                                    {
                                        Console.Write(i);
                                        Console.Write(" ");
                                    }

                                    StringBuilder sb = new StringBuilder();
                                    foreach (int i in newChain)
                                    {
                                        sb.Append(dices[i]);
                                    }

                                    if (checker.CheckSpell(sb.ToString()))
                                    {
                                        Console.Write(" ---- ");

                                        Console.Write(sb.ToString());
                                    }

                                    Console.WriteLine();
                                }

                            }
                        }
                    }
                    //avoid memery overflow
                    chains[level - 1] = null;
                }
            }

        }

    }


    class SpellChecker
    {
        private NetSpell.SpellChecker.Spelling oSpell = null;
        public SpellChecker()
        {
            var oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();

            oDict.DictionaryFile = "en-US.dic";
            oDict.Initialize();

            oSpell = new NetSpell.SpellChecker.Spelling();
            oSpell.Dictionary = oDict;
        }

        public bool CheckSpell(string input)
        {
            return oSpell.TestWord(input);
        }
    }


}
