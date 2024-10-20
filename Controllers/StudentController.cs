using atelier3.Controllers.Repositories;
using atelier3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace atelier3.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly ISchoolRepository schoolRepository; // Changer pour le bon nom de variable

        // Injection de dépendance dans le constructeur
        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository)
        {
            this.studentRepository = studentRepository;
            this.schoolRepository = schoolRepository; // Initialisation de schoolRepository
        }

        // GET: StudentController
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Chargement des écoles dans ViewBag pour le dropdown
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");

            var students = studentRepository.GetAll(); // Récupération de tous les étudiants
            return View(students); // Retourne la vue avec la liste des étudiants

          

        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            var student = studentRepository.GetById(id); // Récupération de l'étudiant par ID
            if (student == null)
            {
                return NotFound(); // Retourne NotFound si l'étudiant n'existe pas
            }

            // Chargement des écoles dans ViewBag pour le dropdown
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(student); // Retourne la vue avec les détails de l'étudiant
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            // Chargement des écoles dans ViewBag pour le dropdown
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(); // Retourne la vue pour créer un nouvel étudiant
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student newStudent)
        {
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            if (ModelState.IsValid) // Vérifie si le modèle est valide
            {
                studentRepository.Add(newStudent); // Ajoute le nouvel étudiant
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            return View(newStudent); // Retourne la vue avec l'étudiant en cas d'erreur
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            var student = studentRepository.GetById(id); // Récupère l'étudiant à éditer
            if (student == null)
            {
                return NotFound(); // Retourne NotFound si l'étudiant n'existe pas
            }

            // Chargement des écoles dans ViewBag pour le dropdown
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(student); // Retourne la vue d'édition avec les données de l'étudiant
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Student updatedStudent)
        {
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            if (ModelState.IsValid) // Vérifie si le modèle est valide
            {
                studentRepository.Edit(updatedStudent); // Met à jour l'étudiant
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            return View(updatedStudent); // Retourne la vue d'édition en cas d'erreur
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            var student = studentRepository.GetById(id); // Récupère l'étudiant à supprimer
            if (student == null)
            {
                return NotFound(); // Retourne NotFound si l'étudiant n'existe pas
            }
            return View(student); // Retourne la vue de confirmation de suppression
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
        {
            try
            {
                studentRepository.Delete(student); // Supprime l'étudiant
                return RedirectToAction(nameof(Index)); // Redirige vers l'index
            }
            catch
            {
                return View(student); // Retourne la vue en cas d'erreur
            }
        }

        // GET: StudentController/StudentsBySchool/5
        public ActionResult StudentsBySchool(int? schoolId)
        {
            if (schoolId == null)
            {
                return NotFound(); // Retourne NotFound si l'école n'existe pas
            }

            var students = studentRepository.GetStudentsBySchoolID(schoolId); // Récupère les étudiants par école
            return View(students); // Retourne la vue avec la liste des étudiants de l'école
        }

        // GET: StudentController/Search
        public ActionResult Search(string name, int? schoolid)
        {
            var result = studentRepository.GetAll(); // Récupération de tous les étudiants
            if (!string.IsNullOrEmpty(name))
            {
                result = studentRepository.FindByName(name); // Recherche par nom
            }
            else if (schoolid != null)
            {
                result = studentRepository.GetStudentsBySchoolID(schoolid); // Récupération par ID d'école
            }
            ViewBag.SchoolID = new SelectList(schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View("Index", result); // Retourne la vue Index avec les résultats filtrés
        }
    }
}
