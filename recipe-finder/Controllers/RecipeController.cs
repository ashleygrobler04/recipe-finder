using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using recipe_finder.Data;
using recipe_finder.Models;

public class RecipeController:Controller {
    private readonly ILogger<RecipeController> _logger;
    //For inserting the recipes
    private readonly ApplicationDbContext _dbContext;

    public RecipeController(ILogger<RecipeController> logger, ApplicationDbContext context)
    {
        _logger=logger;
        _dbContext=context;
    }

    public IActionResult Index() {
        return View();
    }

[Authorize]
    public IActionResult AddRecipe() {
        return View();
    }

[Authorize]
[HttpPost]
    public async Task<IActionResult> Create(RecipeModel recipe) {
        try {
            _dbContext.Add(recipe);
            await _dbContext.SaveChangesAsync();
            return View();
        }
        catch(Exception e) {
            return View("Error "+e.Message);
        }
    }

    public IActionResult RecipeView(Guid id) {
        var recipe=_dbContext.recipeModels.FirstOrDefault(r=>r.Id==id);
        if (recipe!=null) {
            return View(recipe);
        }
        else {
            return View("Recipe not found");
        }
    }

    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}