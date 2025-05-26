using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringsWeek11
{
    class LogAnalyser
    {
        protected List<string> logLines = new List<string>();

        public void Run()
        {
            logLines.Clear();

            //loop while program runs
            while(true)
            {
                Console.WriteLine("Enter the name of the log file to process:");
                string? logName = Console.ReadLine();

                try
                {
                    StreamReader reader = new StreamReader($"Logs\\{logName}");

                    // Store the lines of the log; loop until file is empty
                    for (string? log = reader.ReadLine(); log != null; log = reader.ReadLine())
                    {
                        Console.WriteLine(log);
                        logLines.Add(log);
                    }

                    reader.Close();
                    Console.WriteLine("\n\n****\n\n");

                    while (true)
                    {
                        Console.WriteLine("Display: 'errors [year]', 'activity [year]', 'messages [name]', 'damage [name]'. 'exit'");
                        string?[] input = Console.ReadLine()
                            .ToLower()
                            .Split(' ');
                        if(input.Length == 0)
                        {
                            continue;
                        }

                        switch(input[0])
                        {
                            case "errors":
                                {
                                    //resets if the year is in the wrong format
                                    if (input.Length != 2)
                                    {
                                        continue;
                                    }
                                    int year;
                                    if (int.TryParse(input[1], out year) == false)
                                    {
                                        continue; // resets if invalid input
                                    }

                                    countErrors(year);
                                    break;
                                }

                            case "activity":
                                {
                                    //resets if the year is in the wrong format
                                    if (input.Length != 2)
                                    {
                                        continue;
                                    }
                                    int year;
                                    if (int.TryParse(input[1], out year) == false)
                                    {
                                        continue; // resets if invalid input
                                    }

                                    getActivity(year);
                                    break;
                                }
                            case "messages":
                                {
                                    //resets if the year is in the wrong format
                                    if (input.Length != 2)
                                    {
                                        continue;
                                    }

                                    getMessages(input[1]);
                                    break;
                                }

                            case "damage":
                                {
                                    //resets if the year is in the wrong format
                                    if (input.Length != 2)
                                    {
                                        continue;
                                    }

                                    getDamageInfo(input[1]);
                                    break;
                                }

                            case "exit":
                                return;
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public DateTime? extractDate(string log)
        {
            // extract date
            string timeStamp = log.Substring(0, 21).Trim(['[', ']']);

            DateTime time;
            if(DateTime.TryParse(timeStamp, out time) == false)
            {
                return null;
            }
            return time;
        }

        protected void countErrors(int year)
        {
            int errorCount = 0;
            StringBuilder errorInfo = new StringBuilder();

            foreach (string log in logLines)
            {
                DateTime? time = extractDate(log);
                if (time == null)
                {
                    continue; // skip if log is invalid date
                }

                //check message starts with error
                string message = log.Substring(22).Trim();
                if (time?.Year == year && message.StartsWith("ERROR") == true)
                {
                    errorInfo.AppendLine(log);
                    errorCount++;
                }
            }

            Console.WriteLine($"\nNumber of Errors: {errorCount}");
            Console.WriteLine(errorInfo);
            Console.WriteLine("\n****\n");
        }

        protected void getActivity(int year)
        {
            Dictionary<string, List<string>> activityInfo = new Dictionary<string, List<string>>();
            foreach (string log in logLines)
            {
                DateTime? time = extractDate(log);
                if(time == null)
                {
                    continue; //skip this log if invalid
                }

                //check correct year and message starts with "Player"
                string message = log.Substring(22).Trim(); //extract and remove whitespace
                if(time?.Year == year && message.StartsWith("Player"))
                {
                    string[] parts = message.Split(' ', 3);
                    parts[1] = parts[1].Trim('"'); //remove quotation marks

                    if (!activityInfo.ContainsKey(parts[1]))
                    {
                        activityInfo.Add(parts[1], new List<string>());
                    }
                    activityInfo[parts[1]].Add(parts[2]);
                }
            }

            Console.WriteLine("\n");
            foreach (string player in activityInfo.Keys)
            {
                Console.WriteLine($"{player}:");
                foreach(string message in activityInfo[player])
                {
                    Console.WriteLine($"\t{message}");
                }
            }
            Console.WriteLine("\n****\n");
        }

        protected void getMessages(string playerName)
        {
            StringBuilder output = new StringBuilder();
            foreach (string line in logLines)
            {
                string message = line.Substring(22).Trim();
                if(message.StartsWith("/") && message.ToLower().Contains(playerName))
                {
                    output.AppendLine(line);
                }
            }

            Console.WriteLine($"\n{output}");
            Console.WriteLine($"\n****\n");
        }

        protected void getDamageInfo(string playerName)
        {
            StringBuilder output = new StringBuilder();
            Regex combatLogPattern = new Regex(
                @"Combat Log: ""(?<attacker>.+?)"" hit ""(?<target>.+?)"" for (?<damage>\d+)"
                );

            foreach (string log in logLines)
            {
                Match match = combatLogPattern.Match(log);
                if(match.Success)
                {

                    string attacker = match.Groups["attacker"].Value.ToLower();
                    string target = match.Groups["target"].Value.ToLower();

                    if(attacker == playerName || target == playerName)
                    {
                        int damage = int.Parse(match.Groups["damage"].Value);
                        output.AppendLine($"{attacker} dealt {damage} damage to {target}");
                    }
                }
            }

            Console.WriteLine($"\n{output}");
            Console.WriteLine($"\n****\n");
        }
    }
}
