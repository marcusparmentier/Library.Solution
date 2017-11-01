
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/categories/form")]
    public ActionResult CategoryForm()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult AddCategory()
    {
      Category newCategory = new Category(Request.Form["category-name"]);
      newCategory.Save();
      List<Category> allCategories = Category.GetAll();
      return View("Categories", allCategories);
    }

    [HttpGet("/categories")]
    public ActionResult Categories()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/{id}")]
    public ActionResult CategoryDetail(int id)
    //id = 1
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTasks = selectedCategory.GetTasks();
      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTasks);
      return View(model);
    }

    [HttpGet("/categories/{id}/tasks/new")]
    public ActionResult CategoryTaskForm(int id)
    {

      Category selectedCategory = Category.Find(id);

       return View(selectedCategory);
    }

    [HttpPost("/categories/{id}/tasklist")]
    public ActionResult AddedTask(int id)
    {

      Task newTask = new Task(Request.Form["task-description"],(Request.Form["dueDate"]), id);
      newTask.Save();
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTasks = selectedCategory.GetTasks();
      model.Add("tasks", categoryTasks);
      model.Add("category", selectedCategory);
      return View("CategoryDetail", model);
    }

    [HttpGet("/categories/{id}/tasklist")]
    public ActionResult ViewTaskList(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id); //Category is selected as an object
      List<Task> categoryTasks = selectedCategory.GetTasks(); //Tasks are displayed in a list

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTasks);

      //return the task list for selected category
      return View("CategoryDetail", model);
    }

    [HttpGet("/tasks/{id}/edit")]
    public ActionResult TaskEdit(int id)
    {
      Task thisTask = Task.Find(id);
      return View(thisTask);
    }

    [HttpPost("/tasks/{id}/edit")]
    public ActionResult TaskEditConfirm(int id)
    {
      Task thisTask = Task.Find(id);
      thisTask.UpdateDescription(Request.Form["new-name"]);
      return RedirectToAction("Index");
    }

    [HttpGet("/tasks/{id}/delete")]
    public ActionResult TaskDeleteConfirm(int id)
    {
      Task thisTask = Task.Find(id);
      thisTask.DeleteTask();
      return RedirectToAction("Index");
    }
  }


}
