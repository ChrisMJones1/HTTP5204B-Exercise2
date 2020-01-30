using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class ShowSpecies
    {
        //View model that allows for a singular species object and a List of Pet objects to be returned from a single controller call 
        public Species species { get; set; }
        public List<Pet> pets { get; set; }
    }
}