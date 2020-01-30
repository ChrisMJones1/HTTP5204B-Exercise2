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
        public ActionResult Show(int? id)
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
            return View(species);
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
        public ActionResult Add(string SpeciesName)
        {
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);

            db.Database.ExecuteSqlCommand(query, sqlparams);


            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //need information about a particular pet
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Update(string SpeciesName, int SpeciesID)
        {

            Debug.WriteLine("I am trying to edit a species' name to " + SpeciesName + " and with an ID of " + SpeciesID);

            string query = "UPDATE species set Name = @SpeciesName WHERE speciesid = @SpeciesID";
            SqlParameter[] sqlparams = new SqlParameter[2]; 
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@SpeciesID", SpeciesID);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Delete(int SpeciesID, string DeleteSubmit)
        {

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