using atelier3.Controllers.Repositories;
using atelier3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace atelier3.Controllers
{
    
    [Authorize(Roles = "Admin,Manager")]
    public class SchoolController : Controller
    {
        private readonly ISchoolRepository schoolRepository;





        public SchoolController(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        // GET: SchoolController
        [AllowAnonymous]
        
        public ActionResult Index()
        {
            var schools = schoolRepository.GetAll(); // Récupération de toutes les écoles
            return View(schools); // Retourne la vue avec la liste des écoles
        }

        // GET: SchoolController/Details/5
        public ActionResult Details(int id)
        {
            var school = schoolRepository.GetById(id); // Récupération de l'école par ID
            if (school == null)
            {
                return NotFound(); // Retourne NotFound si l'école n'existe pas
            }

            var averageAge = schoolRepository.StudentAgeAverage(id); // Récupération de la moyenne d'âge des étudiants
            var studentCount = schoolRepository.StudentCount(id); // Récupération du nombre d'étudiants

            ViewBag.AverageAge = averageAge;
            ViewBag.StudentCount = studentCount;

            return View(school); // Retourne la vue avec les détails de l'école et les données des étudiants
        }

        // GET: SchoolController/Create
        public ActionResult Create()
        {
            return View(); // Retourne la vue pour créer une nouvelle école
        }

        // POST: SchoolController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School newSchool)
        {
            try
            { 
                schoolRepository.Add(newSchool); // Ajoute la nouvelle école
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            catch(Exception)
                { 
            return View();
            }// Retourne la vue avec l'école en cas d'erreur
        }

        // GET: SchoolController/Edit/5
        public ActionResult Edit(int id)
        {
            var school = schoolRepository.GetById(id); // Récupère l'école à éditer
            if (school == null)
            {
                return NotFound(); // Retourne NotFound si l'école n'existe pas
            }
            return View(school); // Retourne la vue d'édition avec les données de l'école
        }

        // POST: SchoolController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, School updatedSchool)
        {
            if (ModelState.IsValid) // Vérifie si le modèle est valide
            {
                schoolRepository.Edit(updatedSchool); // Met à jour l'école
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            return View(updatedSchool); // Retourne la vue d'édition en cas d'erreur
        }

        // GET: SchoolController/Delete/5
        public ActionResult Delete(int id)
        {
            var school = schoolRepository.GetById(id); // Récupère l'école à supprimer
            if (school == null)
            {
                return NotFound(); // Retourne NotFound si l'école n'existe pas
            }
            return View(school); // Retourne la vue de confirmation de suppression
        }

        // POST: SchoolController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, School school)
        {
            try
            {
                schoolRepository.Delete(school); // Supprime l'école
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            catch
            {
                return View(school); // Retourne la vue en cas d'erreur
            }
        }
    }
}
