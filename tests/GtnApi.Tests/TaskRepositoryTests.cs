using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Tests;

public class TaskRepositoryTests : IDisposable
{
    private readonly TestDb _db = new();
    private readonly ProjectRepository _projects;
    private readonly TaskRepository _tasks;

    public TaskRepositoryTests()
    {
        _projects = new ProjectRepository(_db);
        _tasks = new TaskRepository(_db);
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task Add_DefaultsToPending()
    {
        var task = await _tasks.AddAsync(new TaskItem { Title = "Hacer algo" });
        var loaded = await _tasks.GetByIdAsync(task.Id);

        Assert.Equal(TaskItemStatus.Pending, loaded!.Status);
        Assert.Null(loaded.ProjectId);
    }

    [Fact]
    public async Task Add_BlankTitle_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _tasks.AddAsync(new TaskItem { Title = "" }));
    }

    [Fact]
    public async Task Add_UnknownProject_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _tasks.AddAsync(new TaskItem { Title = "t", ProjectId = 999 }));
    }

    [Fact]
    public async Task GetAll_FiltersByProjectAndStatus()
    {
        var project = await _projects.AddAsync(new Project { Name = "P" });
        await _tasks.AddAsync(new TaskItem { Title = "pendiente", ProjectId = project.Id });
        await _tasks.AddAsync(new TaskItem
        {
            Title = "completada",
            ProjectId = project.Id,
            Status = TaskItemStatus.Completed
        });
        await _tasks.AddAsync(new TaskItem { Title = "suelta" });

        var all = await _tasks.GetAllAsync();
        var inProject = await _tasks.GetAllAsync(projectId: project.Id);
        var completed = await _tasks.GetAllAsync(project.Id, TaskItemStatus.Completed);

        Assert.Equal(3, all.Count);
        Assert.Equal(2, inProject.Count);
        Assert.Single(completed);
        Assert.Equal("completada", completed[0].Title);
    }

    [Fact]
    public async Task Update_ChangesStatusAndFields()
    {
        var task = await _tasks.AddAsync(new TaskItem { Title = "t" });

        var ok = await _tasks.UpdateAsync(new TaskItem
        {
            Id = task.Id,
            Title = "t2",
            Description = "detalle",
            Status = TaskItemStatus.Completed
        });
        var loaded = await _tasks.GetByIdAsync(task.Id);

        Assert.True(ok);
        Assert.Equal("t2", loaded!.Title);
        Assert.Equal("detalle", loaded.Description);
        Assert.Equal(TaskItemStatus.Completed, loaded.Status);
    }

    [Fact]
    public async Task Delete_RemovesTask()
    {
        var task = await _tasks.AddAsync(new TaskItem { Title = "t" });

        Assert.True(await _tasks.DeleteAsync(task.Id));
        Assert.Null(await _tasks.GetByIdAsync(task.Id));
        Assert.False(await _tasks.DeleteAsync(task.Id));
    }
}
