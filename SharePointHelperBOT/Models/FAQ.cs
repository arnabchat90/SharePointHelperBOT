using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SharePointHelperBOT.Models
{
    [Serializable]
    public class FAQ
    {
        [Key]
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string Task { get; set; }

        public string Subject { get; set; }

        public string Platform { get; set; }

        public string DataStructure { get; set; }

        public string Location { get; set; }
        public string Action { get; set; }

        public string Answer { get; set; }
        public string Classification { get; set; }

    }
}