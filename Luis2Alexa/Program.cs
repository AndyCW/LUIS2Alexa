using Luis2Alexa.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luis2Alexa
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> listEntities = new Dictionary<string, List<string>>();
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: Luis2Alexa luis_export_file.json");
            }
            else
            {
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine($"FILE NOT FOUND: {args[0]}. Usage: Luis2Alexa luis_export_file.json");
                }
                else
                {
                    FileInfo ipFileInfo = new FileInfo(args[0]);
                    var luis = JsonConvert.DeserializeObject<LuisExport>(File.ReadAllText(args[0]));


                    // First write out any list entities and synonyms TODO
                    foreach (var listentity in luis.closedLists)
                    {
                        var listsb = new StringBuilder();

                        foreach (var item in listentity.subLists)
                        {
                            listsb.AppendLine(item.canonicalForm);
                            // Also add to dictionary for later substitution in utterances
                            if (!listEntities.ContainsKey(listentity.name))
                            {
                                listEntities.Add(listentity.name, new List<string> { item.canonicalForm });
                            }
                            else
                            {
                                listEntities[listentity.name].Add(item.canonicalForm);
                            }
                        }
                        string listFilePath = Path.Combine(ipFileInfo.DirectoryName, $"{listentity.name}.values");

                        File.WriteAllText(listFilePath, listsb.ToString());
                        Console.WriteLine($"Written {listentity.name} Slot Values to {listFilePath}");
                    }

                    // Write the grammar file
                    string outputFilePath = Path.Combine(ipFileInfo.DirectoryName, ipFileInfo.Name.Replace(ipFileInfo.Extension, ".grammar"));

                    // Ensure output filename uniqueness
                    int count = 1;
                    while (File.Exists(outputFilePath))
                    {
                        var opFileInfo = new FileInfo(outputFilePath);
                        outputFilePath = Path.Combine(opFileInfo.DirectoryName, $"{opFileInfo.Name.Split(new char[] { '.', '(' })[0]}({count++}).grammar");
                    }

                    var grammersb = new StringBuilder();
                    
                    // Write the intents
                    foreach (var item in luis.utterances.OrderBy((utt) => utt.intent))
                    {
                        if (item.intent != "None")
                        {
                            var text = item.text.ToLower();
                            // Handle any entities
                            if (item.entities.Length > 0)
                            {
                                foreach (var entity in item.entities.OrderByDescending((ent) => ent.startPos))
                                {
                                    text = $"{text.Substring(0, entity.startPos)}{{{entity.entity}}}{text.Substring(entity.endPos+1)}";
                                }
                            }

                            // Substitute any closed list entities
                            foreach (var entitylist in listEntities)
                            {
                                foreach (var value in entitylist.Value)
                                {
                                    text = text.Replace(value, $"{{{entitylist.Key}}}");
                                }
                            }

                            text = text.Replace("please", "{|please}"); // make pleasantries optional

                            // Write resulting utterance
                            grammersb.AppendLine($"{item.intent}: {text}");
                        }
                    }

                    File.WriteAllText(outputFilePath, grammersb.ToString());
                    Console.WriteLine($"Written output to {outputFilePath}");
                }
            }



            Console.ReadLine();
        }
    }
}
