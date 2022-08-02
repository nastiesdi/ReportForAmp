using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Employees;

namespace NewEmployeeDBFinal.Models
{
    public class USModel
    {
        public static string jsonFileName = "AllUserStory.json";
        public static string csvFileName = "allUs.csv";

        [DisplayName("US Number")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Parrent is required.")]
        [DisplayName("Parrent")]
        public int Feature { get; set; }

        [Required(ErrorMessage = "UserStoryTitle is required.")]
        [DisplayName("Title")]
        public string UserStoryTitle { get; set; }

        [Required(ErrorMessage = "TaskType is required.")]
        public string TaskType { get; set; }

        [DisplayName("Assigned To")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "Status")]
        public string Status { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        [DisplayName("Tags")]
        public string Tags { get; set; }

        [DisplayName("Active Bugs")]
        public int Bugs { get; set; }

        public string Color { get; set; }


        public static USModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Replace("\"", "").Replace("\\", "").Split(',');
            USModel userStory = new USModel();
            userStory.Id = int.Parse(values[0]);
            if (!String.IsNullOrEmpty(values[1])) { userStory.Feature = int.Parse(values[1]); }
            userStory.TaskType = values[2];
            userStory.UserStoryTitle = values[3];
            userStory.AssignedTo = values[4].Split("<")[0];
            userStory.Status = values[5];
            userStory.Tags = values[7];
            if (values[7].Contains("Ready to be Released")){
                userStory.Comments = "Ready to be Released";
                userStory.Status = "Ready";
                userStory.Color = "green";
            } else
            {
                userStory.Color = "red";
            }

            return userStory;
        }

        public static List<USModel> GetAllUSFromCsv(string FileNameOptional = null)
        {
            string FileForUS = (string.IsNullOrWhiteSpace(FileNameOptional)) ? csvFileName : FileNameOptional;
            var AllUserStories = Serelisation.DecerializeListOfUSFromCSv(FileForUS);
            var allBugs = Serelisation.DecerializeListOfBugsFromCSv(FileForUS);
            foreach (var item in AllUserStories)
            {
                item.Bugs = allBugs.Where(v => v.Feature == item.Id).Count();
                if (item.Bugs > 0) { item.Status = "BUG FIX";
                    item.Color = "red";
                }
            }
            Serelisation.DecerializeListOfBugsFromCSv(FileForUS);
            Serelisation.SerializeListOfUS(jsonFileName, AllUserStories);
            return AllUserStories;
        }

        public static List<USModel> GetAllUSFromJson(string FileNameOptional = null)
        {
            string FileForUS = (string.IsNullOrWhiteSpace(FileNameOptional)) ? jsonFileName : FileNameOptional;
            var AllUserStories = Serelisation.DecerealiseListOfUS(FileForUS);
            return AllUserStories;
        }

        public static void AddUserStory(USModel UserStory, string FileNameOptional = null)
        {
            var USList = new List<USModel>();
            string FileForStory = (string.IsNullOrWhiteSpace(FileNameOptional)) ? jsonFileName : FileNameOptional;
            USList = File.Exists(FileForStory) ? Serelisation.DecerealiseListOfUS(jsonFileName) : USList;
            USList.Add(UserStory);
            Serelisation.SerializeListOfUS(FileForStory, USList);
        }

        public static void RemoveUserStory(int Id, string FileNameOptional = null)
        {
            string FileForUserStory = (string.IsNullOrWhiteSpace(FileNameOptional)) ? jsonFileName : FileNameOptional;
            var AllUserStory = Serelisation.DecerealiseListOfUS(FileForUserStory);
            var employeeForRemoving = AllUserStory.Single(employe => employe.Id == Id);
            AllUserStory.Remove(employeeForRemoving);
            Serelisation.SerializeListOfUS(FileForUserStory, AllUserStory);
        }

        public static void EditUserStory(USModel editedUS)
        {
            RemoveUserStory(editedUS.Id);
            USModel.AddUserStory(editedUS);
        }

        public static USModel SelectUserStory(int Id, string FileNameOptional = null)
        {
            string FileForUserStory = (string.IsNullOrWhiteSpace(FileNameOptional)) ? jsonFileName : FileNameOptional;
            var AllUserStory = Serelisation.DecerealiseListOfUS(FileForUserStory);
            var USForEditing= AllUserStory.Single(employe => employe.Id == Id);
            return USForEditing;
        }
    }
}
