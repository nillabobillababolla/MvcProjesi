using System.Collections.Generic;

namespace TeknikServis.Models.ViewModels
{
    public class UpdateUserRoleVM
    {
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
