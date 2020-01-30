using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class UpdatePet
    {
        //View model that allows for a singular pet object and a List of species objects to be returned from a single controller
        public Pet pet { get; set; }
        public List<Species> species { get; set; }
    }
}