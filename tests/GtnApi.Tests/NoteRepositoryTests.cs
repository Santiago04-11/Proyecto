using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Tests;

public class NoteRepositoryTests : IDisposable
{
    private readonly TestDb _db = new();
    private readonly ProjectRepository _projects;
    private readonly NoteRepository _notes;

    public NoteRepositoryTests()
    {
        _projects = new ProjectRepository(_db);
        _notes = new NoteRepository(_db);
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task Add_WithoutProject_Persists()
    {
        var note = await _notes.AddAsync(new Note { Title = "Idea", Content = "Texto" });
        var loaded = await _notes.GetByIdAsync(note.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Idea", loaded.Title);
        Assert.Equal("Texto", loaded.Content);
        Assert.Null(loaded.ProjectId);
    }

    [Fact]
    public async Task Add_BlankTitle_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _notes.AddAsync(new Note { Title = " ", Content = "x" }));
    }

    [Fact]
    public async Task Add_UnknownProject_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _notes.AddAsync(new Note { Title = "n", Content = "c", ProjectId = 999 }));
    }

    [Fact]
    public async Task GetAll_FiltersByProject()
    {
        var project = await _projects.AddAsync(new Project { Name = "P" });
        await _notes.AddAsync(new Note { Title = "dentro", Content = "", ProjectId = project.Id });
        await _notes.AddAsync(new Note { Title = "fuera", Content = "" });

        var all = await _notes.GetAllAsync();
        var inProject = await _notes.GetAllAsync(project.Id);

        Assert.Equal(2, all.Count);
        Assert.Single(inProject);
        Assert.Equal("dentro", inProject[0].Title);
    }

    [Fact]
    public async Task Update_ChangesFieldsAndUpdatedAt()
    {
        var note = await _notes.AddAsync(new Note { Title = "a", Content = "b" });

        var ok = await _notes.UpdateAsync(new Note { Id = note.Id, Title = "a2", Content = "b2" });
        var loaded = await _notes.GetByIdAsync(note.Id);

        Assert.True(ok);
        Assert.Equal("a2", loaded!.Title);
        Assert.Equal("b2", loaded.Content);
        Assert.True(loaded.UpdatedAt >= loaded.CreatedAt);
    }

    [Fact]
    public async Task Update_UnknownId_ReturnsFalse()
    {
        Assert.False(await _notes.UpdateAsync(new Note { Id = 999, Title = "x", Content = "y" }));
    }

    [Fact]
    public async Task Delete_RemovesNote()
    {
        var note = await _notes.AddAsync(new Note { Title = "a", Content = "b" });

        Assert.True(await _notes.DeleteAsync(note.Id));
        Assert.Null(await _notes.GetByIdAsync(note.Id));
        Assert.False(await _notes.DeleteAsync(note.Id));
    }
}
