using BethanysPieShop.Models;
using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.ViewModels;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {

        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        //Constructor Injection. Instances of MockedPieRepository and MockCategoryRepository will be injected by the
        //Dependecy injection container (Look to Program.cs)
        public PieController(
                IPieRepository pieRepository, 
                ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult List()
        {
            //ViewBag.CurrentCategory = "Cheese cakes";
            //return View(_pieRepository.AllPies);

            PieListViewModel pieListViewModel = new PieListViewModel(
                _pieRepository.AllPies, "Cheese Cakes"
                );

            return View(pieListViewModel);

        }
    }
}
