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


[HttpPost ("Recipe/UpdateAsync")]
public async Task<IActionResult> UpdateAsync(RecipeModel recipe)
{
    if (recipe == null)
    {
        return View("Error", "Recipe data is invalid.");
    }

    try
    {
        var foundRecipe = _dbContext.recipeModels
            .FirstOrDefault(f => f.Id == recipe.Id);

        if (foundRecipe == null)
        {
            return View("Error", "Recipe not found.");
        }

        foundRecipe.Title = recipe.Title;
        foundRecipe.Instructions = recipe.Instructions;
        foundRecipe.ingredients = recipe.ingredients;

        await _dbContext.SaveChangesAsync();

        // Optionally, you can redirect to the view page to see the updated recipe
        return RedirectToAction("RecipeView", new { id = recipe.Id });
    }
    catch (Exception e)
    {
        return View("Error", "Error updating recipe: " + e.Message);
    }
}
[Authorize]
    public IActionResult UpdateRecipe(Guid id) {
        try {
            var recipe=_dbContext.recipeModels.FirstOrDefault(r=>r.Id==id);
            return View(recipe);
        }
        catch(Exception e) {
            return View(e.Message);
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