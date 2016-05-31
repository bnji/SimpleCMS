using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS
{
    public interface IHasPerson
    {
        int? PersonId { get; set; }
        Person Person { get; set; }
    }
}
