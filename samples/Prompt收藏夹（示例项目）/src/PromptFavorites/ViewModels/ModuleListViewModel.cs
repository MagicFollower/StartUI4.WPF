using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PromptFavorites.Models;
using PromptFavorites.Services;
using StartUI4Controls;

namespace PromptFavorites.ViewModels
{
    public class ModuleListViewModel : ViewModelBase
    {
        private readonly IPromptService _service;
        private List<PromptModule> _allModules;

        public ObservableCollection<PromptModule> Modules { get; private set; }

        private PromptModule _selectedModule;
        public PromptModule SelectedModule
        {
            get { return _selectedModule; }
            set
            {
                if (SetProperty(ref _selectedModule, value))
                    SelectedModuleChanged?.Invoke(value);
            }
        }

        private ModuleSortMode _currentSort = ModuleSortMode.CreatedAt;
        public ModuleSortMode CurrentSort
        {
            get { return _currentSort; }
            set
            {
                if (_currentSort == value) return;
                _currentSort = value;
                RefreshModules();
            }
        }

        public ICommand AddModuleCommand { get; private set; }

        public event Action<PromptModule> SelectedModuleChanged;
        public event Action<string> RequestNewModuleName;

        public ModuleListViewModel(IPromptService service)
        {
            _service = service;
            _allModules = new List<PromptModule>();
            Modules = new ObservableCollection<PromptModule>();
            AddModuleCommand = new RelayCommand(OnAddModule);
        }

        public void LoadModules()
        {
            _allModules = _service.LoadModules().ToList();
            RefreshModules();
        }

        /// <summary>
        /// 按当前排序重建可见集合。重建会让 UI4ListBox 通过 SelectedItem 双向绑定把选中项冲成 null，
        /// 因此这里静默还原到同名模块：不触发 SelectedModuleChanged，否则每次换排序都会重加载中栏
        /// 条目，还可能弹「有未保存更改」。
        /// </summary>
        private void RefreshModules()
        {
            var selectedName = _selectedModule != null ? _selectedModule.Name : null;

            IEnumerable<PromptModule> ordered = _currentSort == ModuleSortMode.Name
                ? _allModules.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase)
                : _allModules.OrderByDescending(m => m.CreatedAt)
                    .ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase);

            Modules.Clear();
            foreach (var m in ordered)
                Modules.Add(m);

            if (selectedName == null) return;

            var match = Modules.FirstOrDefault(m => m.Name == selectedName);
            if (!ReferenceEquals(match, _selectedModule))
            {
                _selectedModule = match;
                RaisePropertyChanged("SelectedModule");
            }
        }

        public void SelectModule(string name)
        {
            var module = Modules.FirstOrDefault(m => m.Name == name);
            if (module != null)
                SelectedModule = module;
            else if (Modules.Count > 0)
                SelectedModule = Modules[0];
        }

        public void RefreshModuleCounts()
        {
            var modules = _service.LoadModules();
            foreach (var m in modules)
            {
                var existing = Modules.FirstOrDefault(x => x.Name == m.Name);
                if (existing != null)
                    existing.EntryCount = m.EntryCount;
            }
        }

        private void OnAddModule()
        {
            RequestNewModuleName?.Invoke(null);
        }

        public void AddModule(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            name = name.Trim();

            var rootPath = ((PromptService)_service).RootPath;
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                UI4MessageBox.Show(
                    "\u8BF7\u5148\u9009\u62E9 Prompt \u6839\u76EE\u5F55\u3002",
                    "\u63D0\u793A", UI4MessageBoxButtons.OK, 360);
                return;
            }

            if (_service.ModuleExists(name)) return;

            var modulePath = new FileSystemRepository(rootPath)
                .CreateModule(name);

            _allModules.Add(new PromptModule
            {
                Name = name,
                FullPath = modulePath,
                EntryCount = 0,
                CreatedAt = DateTime.Now
            });
            RefreshModules();

            var added = Modules.FirstOrDefault(m => m.Name == name);
            if (added != null)
                SelectedModule = added;
        }

        public void RenameModule(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) return;
            newName = newName.Trim();
            if (oldName == newName) return;
            if (_service.ModuleExists(newName)) return;

            _service.RenameModule(oldName, newName);
            LoadModules();
            SelectModule(newName);
        }

        public void DeleteModule(string name)
        {
            _service.DeleteModule(name);
            LoadModules();
            if (Modules.Count > 0)
                SelectedModule = Modules[0];
            else
                SelectedModule = null;
        }
    }
}
