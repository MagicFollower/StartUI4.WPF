using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PromptFavorites.Models;
using PromptFavorites.Services;

namespace PromptFavorites.ViewModels
{
    public class EntryListViewModel : ViewModelBase
    {
        private readonly IPromptService _service;
        private List<PromptItem> _allEntries;
        private string _currentModuleName;

        public ObservableCollection<PromptItem> Entries { get; private set; }

        private PromptItem _selectedEntry;
        public PromptItem SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                if (SetProperty(ref _selectedEntry, value))
                    SelectedEntryChanged?.Invoke(value);
            }
        }

        private bool _isFavoriteFilter;
        public bool IsFavoriteFilter
        {
            get { return _isFavoriteFilter; }
            set
            {
                if (SetProperty(ref _isFavoriteFilter, value))
                    RefreshDisplay();
            }
        }

        private SortMode _currentSort = SortMode.UseCount;
        public SortMode CurrentSort
        {
            get { return _currentSort; }
            set
            {
                if (SetProperty(ref _currentSort, value))
                    RefreshDisplay();
            }
        }

        private bool _isInSearchMode;
        public bool IsInSearchMode
        {
            get { return _isInSearchMode; }
            set { SetProperty(ref _isInSearchMode, value); }
        }

        public ICommand AddEntryCommand { get; private set; }
        public ICommand ToggleFavoriteFilterCommand { get; private set; }
        public ICommand SetSortCommand { get; private set; }
        public ICommand ToggleEntryFavoriteCommand { get; private set; }
        public ICommand QuickCopyCommand { get; private set; }

        public event Action<PromptItem> SelectedEntryChanged;
        public event Action<PromptItem> RequestQuickCopy;
        public event Action<string> RequestNewEntryName;

        /// <summary>中栏星标切换收藏后通知外部（文件路径 + 新状态），用于右栏联动。</summary>
        public event Action<string, bool> FavoriteToggled;

        public EntryListViewModel(IPromptService service)
        {
            _service = service;
            _allEntries = new List<PromptItem>();
            Entries = new ObservableCollection<PromptItem>();

            AddEntryCommand = new RelayCommand(OnAddEntry);
            ToggleFavoriteFilterCommand = new RelayCommand(() => IsFavoriteFilter = !IsFavoriteFilter);
            SetSortCommand = new RelayCommand<SortMode>(mode => CurrentSort = mode);
            ToggleEntryFavoriteCommand = new RelayCommand<PromptItem>(OnToggleEntryFavorite);
            QuickCopyCommand = new RelayCommand<PromptItem>(item =>
            {
                if (item != null) RequestQuickCopy?.Invoke(item);
            });
        }

        public void LoadEntries(string moduleName, bool selectFirst = true)
        {
            _currentModuleName = moduleName;
            IsInSearchMode = false;
            _allEntries = _service.LoadEntries(moduleName).ToList();
            RefreshDisplay();
            SelectedEntry = selectFirst ? Entries.FirstOrDefault() : null;
        }

        /// <summary>右栏切换收藏后把状态同步到列表实例；筛选开启时被取消收藏的行会立即消失。</summary>
        public void ApplyFavorite(string filePath, bool value)
        {
            var match = _allEntries.FirstOrDefault(e => e.FilePath == filePath);
            if (match == null || match.Favorite == value) return;

            match.Favorite = value;
            ApplyFavoriteToView();
        }

        private void OnToggleEntryFavorite(PromptItem item)
        {
            if (item == null) return;

            _service.ToggleFavorite(item);
            FavoriteToggled?.Invoke(item.FilePath, item.Favorite);
            ApplyFavoriteToView();
        }

        /// <summary>
        /// 收藏状态变化后刷新视图。行内星标靠 PromptItem 自身的属性通知更新，无需重建列表；
        /// 只有收藏筛选开着时才需要重建（被取消收藏的那一行要立即消失）。重建会让 UI4ListBox
        /// 通过 SelectedItem 双向绑定把 SelectedEntry 回写成 null，因此这里显式保住选中项。
        /// </summary>
        private void ApplyFavoriteToView()
        {
            if (!_isFavoriteFilter) return;

            var selectedPath = SelectedEntry != null ? SelectedEntry.FilePath : null;
            RefreshDisplay();

            if (selectedPath == null) return;

            var visible = Entries.FirstOrDefault(e => e.FilePath == selectedPath);
            SelectedEntry = visible;
        }

        public void ShowSearchResults(IReadOnlyList<PromptItem> results)
        {
            IsInSearchMode = true;
            _allEntries = results.ToList();
            RefreshDisplay();
            SelectedEntry = Entries.FirstOrDefault();
        }

        public void RestoreModuleView()
        {
            if (!string.IsNullOrEmpty(_currentModuleName))
                LoadEntries(_currentModuleName);
        }

        public void RefreshCurrentModule()
        {
            if (!string.IsNullOrEmpty(_currentModuleName) && !IsInSearchMode)
            {
                var selectedTitle = SelectedEntry != null ? SelectedEntry.Title : null;
                _allEntries = _service.LoadEntries(_currentModuleName).ToList();
                RefreshDisplay();
                if (selectedTitle != null)
                {
                    SelectedEntry = Entries.FirstOrDefault(e => e.Title == selectedTitle);
                }
            }
        }

        private void RefreshDisplay()
        {
            var filtered = _allEntries.AsEnumerable();
            if (_isFavoriteFilter)
                filtered = filtered.Where(e => e.Favorite);

            var sorted = _service.ApplySort(filtered.ToList(), _currentSort);

            Entries.Clear();
            foreach (var entry in sorted)
                Entries.Add(entry);
        }

        private void OnAddEntry()
        {
            if (string.IsNullOrEmpty(_currentModuleName) || IsInSearchMode) return;
            RequestNewEntryName?.Invoke(null);
        }

        public void AddEntry()
        {
            if (string.IsNullOrEmpty(_currentModuleName)) return;
            var item = _service.CreateEntry(_currentModuleName);
            _allEntries.Add(item);
            RefreshDisplay();
            SelectedEntry = item;
        }

        public void RenameEntry(PromptItem item, string newTitle)
        {
            if (item == null || string.IsNullOrWhiteSpace(newTitle)) return;
            newTitle = newTitle.Trim();
            if (item.Title == newTitle) return;

            _service.RenameEntry(item, newTitle);
            RefreshCurrentModule();
        }

        public void DeleteEntry(PromptItem item)
        {
            if (item == null) return;
            _service.DeleteEntry(item);
            _allEntries.Remove(item);
            RefreshDisplay();
            SelectedEntry = Entries.FirstOrDefault();
        }
    }
}
