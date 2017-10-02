using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serialize.Linq.Nodes;

namespace ProtobufDemo.Data.API
{
    public class FilterIncludeRequest
    {
        public FilterIncludeRequest(ExpressionNode filter, ExpressionNode[] includeProperties = null)
        {
            this.Filter = filter;
            this.IncludeProperties = includeProperties;
        }

        public ExpressionNode Filter { get; set; }
        public ExpressionNode[] IncludeProperties { get; set; }
    }
}
