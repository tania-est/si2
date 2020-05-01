using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static si2.common.Enums;

namespace si2.bll.Dtos.Results.Dataflow
{
    public class DataflowDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string Status { get; set; }

        public byte[] RowVersion { get; set; }

        public override bool Equals(Object obj) => Equals(obj as DataflowDto);

        public bool Equals(DataflowDto obj)

        {

            return (this.Id == obj.Id

                && string.Equals(this.Title, obj.Title, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.Name, obj.Name, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.Tag, obj.Tag, StringComparison.OrdinalIgnoreCase)

                && string.Equals(this.Status, obj.Status, StringComparison.OrdinalIgnoreCase)

                && this.RowVersion.SequenceEqual(obj.RowVersion));

        }

    }

}