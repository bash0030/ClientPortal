using System.Collections.Generic;
using System.Web.Mvc;

namespace ClientPortal.Models
{
    public class UserInputDefinition
    {
        public string Type { set; get; }
        public string[] Label { set; get; }
        public IEnumerable<SelectListItem> Options { set; get; }
        public string[] Model { set; get; }
        public bool[] Required { set; get; }
        public string[] Tooltip { set; get; }
        public string[] InputType { set; get; }
    }
    public class Question
    {
        public string QuestionText { set; get; }
        public string HelpInfo { set; get; }
        public UserInputDefinition UserInputDefinition { set; get; }
    }
}