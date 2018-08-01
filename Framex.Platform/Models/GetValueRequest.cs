using Framex.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Framex.Platform.Models
{
    public class GetValueRequest : IFramexRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
