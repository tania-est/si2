using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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

        public override bool Equals(Object obj) => Equals(obj as SimplecohortDto);

        public bool Equals(SimplecohortDto obj)

        {

            return (this.Id == obj.Id

                && string.Equals(this.registrationRequirements, obj.registrationRequirements, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.graduationRequirements, obj.graduationRequirements, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.transferRequirements, obj.transferRequirements, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.Status, obj.Status, StringComparison.OrdinalIgnoreCase)

                && this.RowVersion.SequenceEqual(obj.RowVersion));

        }
    }
}
