using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public System.Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public ExtendedUserModel ExtendedUserModel { get; set; }
    }
}
