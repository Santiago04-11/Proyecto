using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Tests;

public class ProjectRepositoryTests : IDisposable
{
    private readonly TestDb _db = new();
    private readonly ProjectRepository _projects;
    private readonly NoteRepository _notes;
    private readonly TaskRepository _tasks;

    public ProjectRepositoryTests()
    {
        _projects = new ProjectRepository(_db);
        _notes = new NoteRepository(_db);
        _tasks = new TaskRepository(_db);
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task Add_AssignsIdTrimsNameAndSetsCreatedAt()
    {
        var project = await _projects.AddAsync(new Project { Name = "  Alpha  " });

        Assert.True(project.Id > 0);
        Assert.Equal("Alpha", project.Name);
        Assert.True(project.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Add_BlankName_Throws(string name)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _projects.AddAsync(new Project { Name = name }));
    }

    [Fact]
    public async Task GetAll_ReturnsProjectsOrderedByName()
    {
        await _projects.AddAsync(new Project { Name = "Zeta" });
        await _projects.AddAsync(new Project { Name = "Alpha" });

        var all = await _projects.GetAllAsync();

        Assert.Equal(new[] { "Alpha", "Zeta" }, all.Select(p => p.Name).ToArray());
    }

    [Fact]
    public async Task Update_ChangesFields()
    {
        var project = await _projects.AddAsync(new Project { Name = "Old" });

        var ok = await _projects.UpdateAsync(new Project { Id = project.Id, Name = "New", Description = "Desc" });
        var loaded = await _projects.GetByIdAsync(project.Id);

        Assert.True(ok);
        Assert.Equal("New", loaded!.Name);
        Assert.Equal("Desc", loaded.Description);
    }

    [Fact]
    public async Task Update_UnknownId_ReturnsFalse()
    {
        var ok = await _projects.UpdateAsync(new Project { Id = 999, Name = "X" });
        Assert.False(ok);
    }

    [Fact]
    public async Task Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(await _projects.DeleteAsync(999));
    }

    [Fact]
    public async Task Delete_KeepsNotesAndTasksWithoutProject()
    {
        var project = await _projects.AddAsync(new Project { Name = "P" });
        var note = await _notes.AddAsync(new Note { Title = "n", Content = "c", ProjectId = project.Id });
        var task = await _tasks.AddAsync(new TaskItem { Title = "t", ProjectId = project.Id });

        var deleted = await _projects.DeleteAsync(project.Id);

        Assert.True(deleted);
        Assert.Null(await _projects.GetByIdAsync(project.Id));
        Assert.Null((await _notes.GetByIdAsync(note.Id))!.ProjectId);
        Assert.Null((await _tasks.GetByIdAsync(task.Id))!.ProjectId);
    }
}
