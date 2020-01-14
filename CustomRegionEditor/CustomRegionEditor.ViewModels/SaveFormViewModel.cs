using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.ViewModels
{
    public class SaveFormViewModel
    {
        [Required(ErrorMessage = "The name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The description field is required")]
        public string Description { get; set; }

        public string Id { get; set; }
    }
}
