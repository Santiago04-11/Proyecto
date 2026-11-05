using System.Windows;
using System.Windows.Controls;
using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Desktop;

public partial class MainWindow : Window
{
    private readonly IProjectRepository _projectRepo;
    private readonly INoteRepository _noteRepo;
    private readonly ITaskRepository _taskRepo;
    private Project? _proyectoSeleccionado;

    public MainWindow(IProjectRepository projectRepo, INoteRepository noteRepo, ITaskRepository taskRepo)
    {
        InitializeComponent();
        _projectRepo = projectRepo;
        _noteRepo = noteRepo;
        _taskRepo = taskRepo;
        
        CargarProyectos();
    }

    private async void CargarProyectos()
    {
        var proyectos = await _projectRepo.GetAllAsync();
        LstProyectos.ItemsSource = proyectos;
    }

    private async void LstProyectos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _proyectoSeleccionado = LstProyectos.SelectedItem as Project;
        if (_proyectoSeleccionado == null) return;

        TxtTituloProyecto.Text = _proyectoSeleccionado.Name;
        await CargarTareasYNotas();
    }

    private async Task CargarTareasYNotas()
    {
        if (_proyectoSeleccionado == null) return;
        LstTareas.ItemsSource = await _taskRepo.GetAllAsync(_proyectoSeleccionado.Id);
        LstNotas.ItemsSource = await _noteRepo.GetAllAsync(_proyectoSeleccionado.Id);
    }

    private async void BtnNuevoProyecto_Click(object sender, RoutedEventArgs e)
    {
        var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre del proyecto:", "Nuevo Proyecto", "Nuevo Proyecto");
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            await _projectRepo.AddAsync(new Project { Name = nombre });
            CargarProyectos();
        }
    }

    private async void BtnNuevaTarea_Click(object sender, RoutedEventArgs e)
    {
        if (_proyectoSeleccionado == null || string.IsNullOrWhiteSpace(TxtNuevaTarea.Text)) return;

        await _taskRepo.AddAsync(new TaskItem { Title = TxtNuevaTarea.Text, ProjectId = _proyectoSeleccionado.Id });
        TxtNuevaTarea.Clear();
        await CargarTareasYNotas();
    }

    private async void BtnNuevaNota_Click(object sender, RoutedEventArgs e)
    {
        if (_proyectoSeleccionado == null || string.IsNullOrWhiteSpace(TxtNuevaNota.Text)) return;

        await _noteRepo.AddAsync(new Note { Title = TxtNuevaNota.Text, Content = "", ProjectId = _proyectoSeleccionado.Id });
        TxtNuevaNota.Clear();
        await CargarTareasYNotas();
    }
}
