using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WorkApp.Models
{
    public class Worker
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return this.Name + "(" + this.Phone + ")";
        }
    }
}
