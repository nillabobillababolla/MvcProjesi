using System.Collections.Generic;
using System.ComponentModel;

namespace TeknikServis.Models.ViewModels
{
    public class UpdateUserRoleVM
    {
        public string Id { get; set; }

        [DisplayName("Roller")]
        public List<string> Roles { get; set; }
    }
}
