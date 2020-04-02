using si2.dal.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static si2.common.Enums;

namespace si2.dal.Entities
{
    [Table("Simplecohort")]
    public class Simplecohort : Si2BaseDataEntity<Guid>, IAuditable
    {
        //The type should be Requirement[] for all the attributes, but we put it string for now
        //since the Requirement may be changed from being an interface

        [Required]
        public string[] registrationRequirements { get; set; }
        [Required]
        public string[] graduationRequirements { get; set; }
        [Required]
        public string[] transferRequirements { get; set; }

        [Required]
        public SimplecohortStatus Status { get; set; }

    }
}
