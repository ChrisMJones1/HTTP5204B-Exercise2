using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using PetGrooming.Models.ViewModels;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List()
        {
            //what data do we need?
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();

            return View(myspecies);
        }
        public ActionResult Show(int? id) //when showing a specific species, we also want to retrieve the list of pets with that species to display using the ShowSpecies viewmodel
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.SqlQuery("select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            List<Pet> selectedpets = db.Pets.SqlQuery("select * from pets where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).ToList();
  

            ShowSpecies showspecies = new ShowSpecies();

            showspecies.pets = selectedpets;
            showspecies.species = species;

            return View(showspecies);
        }
        public ActionResult Add()
        {
            //STEP 1: PUSH DATA!
            //What data does the Add.cshtml page need to display the interface?
            //A list of species to choose for a pet

            //alternative way of writing SQL -- will learn more about this week 4
            //List<Species> Species = db.Species.ToList();

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        [HttpPost]
        public ActionResult Add(string SpeciesName) //upon post submission of the Species/Add page form, run an insert statement using the provided name
        {
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);

            db.Database.ExecuteSqlCommand(query, sqlparams);


            //return to the list of species
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //To fill the existing name into the entry form, we grab the species from the database by its id
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @id", new SqlParameter("@id", id)).FirstOrDefault();

            
            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Update(string SpeciesName, int SpeciesID) //taking the new Species name and the ID of the species you want to change, run an update query
        {

            Debug.WriteLine("I am trying to edit a species' name to " + SpeciesName + " and with an ID of " + SpeciesID);

            string query = "UPDATE species set Name = @SpeciesName WHERE speciesid = @SpeciesID";
            SqlParameter[] sqlparams = new SqlParameter[2]; 
        
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@SpeciesID", SpeciesID);


            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id) //grab the info to display on the delete page confirmation
        {
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Delete(int SpeciesID, string DeleteSubmit) //upon clicking the delete button, run a delete command on the database for that ID
        {
            //This delete assumes that orphaned data in the Pets table would be better suited to be handled by server triggers
            //if server triggers were not to be used, a delete statement for where the speciesID exists as a foreign key in other
            //tables would be issued before the delete statement where it is a primary key
            Debug.WriteLine("I am trying to delete a species with an ID of " + SpeciesID);

            string query = "DELETE FROM species WHERE speciesid = @SpeciesID";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesID", SpeciesID);
            if (DeleteSubmit == "Delete Species?")
            {
                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("List");
        }

        //TODO:
        // 
    }
}