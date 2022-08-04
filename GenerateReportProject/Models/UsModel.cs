using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Employees;

namespace NewEmployeeDBFinal.Models
{
    public class UsModel
    {
        private const string JsonFileName = "Files/AllUserStory.json";
        private const string CsvFileName = "allUs.csv";

        [DisplayName("US Number")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Parent is required.")]
        [DisplayName("Parent")]
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


        public static UsModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Replace("\"", "").Replace("\\", "").Split(',');
            var userStory = new UsModel();
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

        public static List<UsModel> GetAllUsFromCsv(string fileNameOptional = null)
        {
            var fileForUs = (string.IsNullOrWhiteSpace(fileNameOptional)) ? CsvFileName : fileNameOptional;
            var allUserStories = Serelisation.DecerializeListOfUSFromCSv(fileForUs);
            var allBugs = Serelisation.DecerializeListOfBugsFromCSv(fileForUs);
            foreach (var item in allUserStories)
            {
                item.Bugs = allBugs.Count(v => v.Feature == item.Id);
                if (item.Bugs > 0) 
                {
                    item.Status = "BUG FIX";
                    item.Color = "red";
                }
            }
            Serelisation.DecerializeListOfBugsFromCSv(fileForUs);
            Serelisation.SerializeListOfUS(JsonFileName, allUserStories);
            return allUserStories;
        }

        public static List<UsModel> GetAllUsFromJson(string fileNameOptional = null)
        {
            var fileForUs = (string.IsNullOrWhiteSpace(fileNameOptional)) ? JsonFileName : fileNameOptional;
            var allUserStories = Serelisation.DecerealiseListOfUS(fileForUs);
            return allUserStories;
        }

        public static void AddUserStory(UsModel userStory, string fileNameOptional = null)
        {
            var usList = new List<UsModel>();
            string FileForStory = (string.IsNullOrWhiteSpace(fileNameOptional)) ? JsonFileName : fileNameOptional;
            usList = File.Exists(FileForStory) ? Serelisation.DecerealiseListOfUS(JsonFileName) : usList;
            usList.Add(userStory);
            Serelisation.SerializeListOfUS(FileForStory, usList);
        }

        public static void RemoveUserStory(int id, string fileNameOptional = null)
        {
            var fileForUserStory = (string.IsNullOrWhiteSpace(fileNameOptional)) ? JsonFileName : fileNameOptional;
            var allUserStory = Serelisation.DecerealiseListOfUS(fileForUserStory);
            var userStoryForRemoving = allUserStory.Single(userStory => userStory.Id == id);
            allUserStory.Remove(userStoryForRemoving);
            Serelisation.SerializeListOfUS(fileForUserStory, allUserStory);
        }

        public static void EditUserStory(UsModel editedUs)
        {
            RemoveUserStory(editedUs.Id);
            AddUserStory(editedUs);
        }

        public static UsModel SelectUserStory(int id, string fileNameOptional = null)
        {
            var fileForUserStory = (string.IsNullOrWhiteSpace(fileNameOptional)) ? JsonFileName : fileNameOptional;
            var allUserStory = Serelisation.DecerealiseListOfUS(fileForUserStory);
            var usForEditing= allUserStory.Single(userStory => userStory.Id == id);
            return usForEditing;
        }
    }
}
