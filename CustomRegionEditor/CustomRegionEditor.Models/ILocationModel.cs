using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public interface ILocationModel
    {
        string Id { get; set; }
        string Type { get; }
    }
}
