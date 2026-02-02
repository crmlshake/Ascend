using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Ascend.Data;
using Ascend.Helpers;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace Ascend.MVVM.ViewModel
{
    public partial class TodoViewModel : INotifyPropertyChanged
    {
        private readonly LoginDbContext _dbContext;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<TodoItem> Todos { get; set; } = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> FavoriteTodos { get; set; } = new ObservableCollection<TodoItem>();

        public ICommand AddTodoCommand { get; }
        public ICommand RemoveTodoCommand { get; }
        public ICommand CompleteTodoCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }

        public string NewTodoText { get; set; }

        private int _currentXp;
        public int CurrentXp
        {
            get => _currentXp;
            set
            {
                _currentXp = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(XpProgress));
                OnPropertyChanged(nameof(XpDisplayText));
            }
        }

        private int _currentLevel;
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                OnPropertyChanged();
            }
        }

        private int _xpToNextLevel;
        public int XpToNextLevel
        {
            get => _xpToNextLevel;
            set
            {
                _xpToNextLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(XpProgress));
            }
        }

        public double XpProgress => (double)CurrentXp / XpToNextLevel * 100;
        public string XpDisplayText => $"{CurrentXp} / {XpToNextLevel} XP";

        public TodoViewModel()
        {
            _dbContext = new LoginDbContext();
            AddTodoCommand = new RelayCommand(AddTodo);
            RemoveTodoCommand = new RelayCommand<TodoItem>(RemoveTodo);
            CompleteTodoCommand = new RelayCommand<TodoItem>(CompleteTodo);
            ToggleFavoriteCommand = new RelayCommand<TodoItem>(ToggleFavorite);

            LoadTodos();
            LoadUserProgress();
        }

        private void LoadTodos()
        {
            var userID = SessionManager.CurrentUserID;
            var todosFromDb = _dbContext.Todos.Where(t => t.UserID == userID).ToList();

            Todos.Clear();
            FavoriteTodos.Clear();

            foreach (var todo in todosFromDb)
            {
                var item = new TodoItem
                {
                    ID = todo.ID,
                    Text = todo.Text,
                    IsFavorite = todo.IsFavorite
                };

                if (todo.IsFavorite)
                    FavoriteTodos.Add(item);
                else
                    Todos.Add(item);
            }
        }

        private void LoadUserProgress()
        {
            var userID = SessionManager.CurrentUserID;
            var progress = _dbContext.UserProgress.FirstOrDefault(p => p.UserID == userID);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    UserID = userID,
                    CurrentXp = 0,
                    CurrentLevel = 1,
                    XpToNextLevel = 100
                };

                _dbContext.UserProgress.Add(progress);
                _dbContext.SaveChanges();
            }

            CurrentXp = progress.CurrentXp;
            CurrentLevel = progress.CurrentLevel;
            XpToNextLevel = progress.XpToNextLevel;
        }

        private void AddTodo()
        {
            if (!string.IsNullOrWhiteSpace(NewTodoText))
            {
                var userID = SessionManager.CurrentUserID;
                var newTodo = new Todo
                {
                    UserID = userID,
                    Text = NewTodoText,
                    CreatedAt = DateTime.Now,
                    IsFavorite = false
                };

                _dbContext.Todos.Add(newTodo);
                _dbContext.SaveChanges();

                var newItem = new TodoItem
                {
                    ID = newTodo.ID,
                    Text = newTodo.Text,
                    IsFavorite = false
                };

                Todos.Add(newItem);

                NewTodoText = string.Empty;
                OnPropertyChanged(nameof(NewTodoText));
            }
        }

        private void RemoveTodo(TodoItem todo)
        {
            var todoToRemove = _dbContext.Todos.FirstOrDefault(t => t.ID == todo.ID);
            if (todoToRemove != null)
            {
                _dbContext.Todos.Remove(todoToRemove);
                _dbContext.SaveChanges();

                Todos.Remove(todo);
                FavoriteTodos.Remove(todo);
            }
        }

        private void CompleteTodo(TodoItem todo)
        {
            var todoToComplete = _dbContext.Todos.FirstOrDefault(t => t.ID == todo.ID);
            if (todoToComplete != null)
            {
                _dbContext.Todos.Remove(todoToComplete);
                _dbContext.SaveChanges();

                Todos.Remove(todo);
                FavoriteTodos.Remove(todo);

                GiveXpToUser(20);
            }
        }

        private void ToggleFavorite(TodoItem todo)
        {
            var todoToUpdate = _dbContext.Todos.FirstOrDefault(t => t.ID == todo.ID);
            if (todoToUpdate != null)
            {
                todoToUpdate.IsFavorite = !todoToUpdate.IsFavorite;
                _dbContext.SaveChanges();

                if (todoToUpdate.IsFavorite)
                {
                    Todos.Remove(todo);
                    FavoriteTodos.Add(todo);
                }
                else
                {
                    FavoriteTodos.Remove(todo);
                    Todos.Add(todo);
                }

                todo.IsFavorite = todoToUpdate.IsFavorite;
                OnPropertyChanged(nameof(Todos));
                OnPropertyChanged(nameof(FavoriteTodos));
            }
        }

        private void GiveXpToUser(int xpAmount)
        {
            var userID = SessionManager.CurrentUserID;
            var progress = _dbContext.UserProgress.FirstOrDefault(p => p.UserID == userID);

            if (progress == null)
                return;

            progress.CurrentXp += xpAmount;

            if (progress.CurrentXp >= progress.XpToNextLevel)
            {
                progress.CurrentXp -= progress.XpToNextLevel;
                progress.CurrentLevel += 1;
                progress.XpToNextLevel = CalculateXpRequirement(progress.CurrentLevel);
            }

            _dbContext.SaveChanges();

            var updatedProgress = _dbContext.UserProgress.FirstOrDefault(p => p.UserID == userID);

            App.Current.Dispatcher.Invoke(() =>
            {
                if (updatedProgress != null)
                {
                    CurrentXp = updatedProgress.CurrentXp;
                    CurrentLevel = updatedProgress.CurrentLevel;
                    XpToNextLevel = updatedProgress.XpToNextLevel;

                    OnPropertyChanged(nameof(CurrentXp));
                    OnPropertyChanged(nameof(CurrentLevel));
                    OnPropertyChanged(nameof(XpToNextLevel));
                    OnPropertyChanged(nameof(XpDisplayText));
                    OnPropertyChanged(nameof(XpProgress));
                }
            });
        }

        private int CalculateXpRequirement(int level)
        {
            return 100 + (level - 1) * 25;
        }
    }

    public class TodoItem : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set { _isFavorite = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}