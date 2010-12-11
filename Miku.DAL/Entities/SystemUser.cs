using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;

namespace Miku.DAL.Entities
{
    [Table(Name = "SystemUser")]
    public class SystemUser
    {
        [Column(IsPrimaryKey=true)]
        public Guid UserID { get; set; }

        [Column]
        public string LoginName { get; set; }

        [Column]
        public string IPAddress { get; set; }

        [Column]
        public int Port { get; set; }

        [Column]
        public int UserState { get; set; }

        
    }
}
