using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NewEmployeeDBFinal.Models;

namespace Employees
{
    class Serelisation
    {

        internal static void SerializeListOfUS(string pathToFile, List<UsModel> UserStoryList)
        {
            string jsonString = JsonSerializer.Serialize(UserStoryList);
            File.WriteAllText(pathToFile, jsonString);
        }

        internal static List<UsModel> DecerealiseListOfUS(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                string jsonOneString = File.ReadAllText(pathToFile);
                List<UsModel> UserStoryList = JsonSerializer.Deserialize<List<UsModel>>(jsonOneString)!;
                List<UsModel>  readyList = UserStoryList.OrderBy(v => v.Feature).ToList();
                return readyList;
            }
            var emptyUserStoryList = new List<UsModel>();
            return emptyUserStoryList;
        }

        internal static List<UsModel> DecerializeListOfUSFromCSv(string pathToFile)
        {
            List<UsModel> values = File.ReadAllLines(pathToFile)
                                           .Skip(1)
                                           .Select(v => UsModel.FromCsv(v))
                                           .OrderBy(v => v.Feature)
                                           .Where(v=> v.TaskType == "User Story" || v.TaskType == "Bug")
                                           .ToList();
            return values;
        }

        internal static List<UsModel> DecerializeListOfBugsFromCSv(string pathToFile)
        {
            List<UsModel> bugs = File.ReadAllLines(pathToFile)
                                           .Skip(1)
                                           .Select(v => UsModel.FromCsv(v))
                                           .OrderBy(v => v.Feature)
                                           .Where(v => v.TaskType == "Testing Bug")
                                           .ToList();
            return bugs;
        }
    }

}
