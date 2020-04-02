using System;
using System.Collections.Generic;
using System.Text;
using static si2.common.Enums;

namespace si2.bll.Dtos.Results.Simplecohort
{
    public class SimplecohortDto
    {
        public Guid Id { get; set; }
        public string registrationRequirements { get; set; }
        public string graduationRequirements { get; set; }
        public string transferRequirements { get; set; }
        public string Status { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
