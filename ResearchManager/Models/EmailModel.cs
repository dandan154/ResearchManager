using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ResearchManager.Models
{
    public class EmailModel
    {
        // RIS signs on receipt of project
        // RIS request researcher signs it
        // RIS passes to associate dean for signing
        // Associate dean passes to Dean for final signature

        // 1 = RIS
        // 2 = Researcher
        // 3 = Associate dean
        // 4 = Dean

        [DataType(DataType.EmailAddress), Display(Name = "To")]
        [Required]
        public string ToEmail { get; set; }
        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string EMailBody { get; set; }
        [Display(Name = "Subject")]
        public string EmailSubject { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "CC")]
        public string EmailCC { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "BCC")]
        public string EmailBCC { get; set; }
    }
}