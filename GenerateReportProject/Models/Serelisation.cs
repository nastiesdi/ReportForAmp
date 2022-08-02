using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NewEmployeeDBFinal.Models;

namespace Employees
{
    class Serelisation
    {

        internal static void SerializeListOfUS(string pathToFile, List<USModel> UserStoryList)
        {
            string jsonString = JsonSerializer.Serialize(UserStoryList);
            File.WriteAllText(pathToFile, jsonString);
        }

        internal static List<USModel> DecerealiseListOfUS(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                string jsonOneString = File.ReadAllText(pathToFile);
                List<USModel> UserStoryList = JsonSerializer.Deserialize<List<USModel>>(jsonOneString)!;
                List<USModel>  readyList = UserStoryList.OrderBy(v => v.Feature).ToList();
                return readyList;
            }
            var emptyUserStoryList = new List<USModel>();
            return emptyUserStoryList;
        }

        internal static List<USModel> DecerializeListOfUSFromCSv(string pathToFile)
        {
            List<USModel> values = File.ReadAllLines(pathToFile)
                                           .Skip(1)
                                           .Select(v => USModel.FromCsv(v))
                                           .OrderBy(v => v.Feature)
                                           .Where(v=> v.TaskType == "User Story" || v.TaskType == "Bug")
                                           .ToList();
            return values;
        }

        internal static List<USModel> DecerializeListOfBugsFromCSv(string pathToFile)
        {
            List<USModel> bugs = File.ReadAllLines(pathToFile)
                                           .Skip(1)
                                           .Select(v => USModel.FromCsv(v))
                                           .OrderBy(v => v.Feature)
                                           .Where(v => v.TaskType == "Testing Bug")
                                           .ToList();
            return bugs;
        }
    }

}
