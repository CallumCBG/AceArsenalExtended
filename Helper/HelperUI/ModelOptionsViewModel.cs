﻿using System.ComponentModel;

namespace HelperUI
{
    public class ModelOptionsViewModel : INotifyPropertyChanged
    {
        public const string NewOptionName = "newOption";

        private string name;
        private bool isEditable;
        private string value;

        public ModelOptionsViewModel(ModelViewModel parent, string name, bool isAutomatic, string value)
        {
            this.name = name;
            this.IsAutomatic = isAutomatic;
            this.isEditable = !isAutomatic;
            this.value = value;
            Parent = parent;
        }

        public string Name 
        {
            get 
            { 
                return name; 
            } 
            set
            {
                Parent.GetGroup().RenameOption(name, value);
            }
        }
        internal void SetName(string newName)
        {
            name = newName;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                Parent.SetMetadataAndSave();
            }
        }

        public bool IsAutomatic { get; set; }

        public bool IsEditable
        {
            get { return isEditable; }
            set
            {
                if (isEditable != value)
                {
                    isEditable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
                }
            }
        }


        public bool CanBeIgnored => string.IsNullOrEmpty(Name) || (string.IsNullOrEmpty(Value) && Name == NewOptionName);

        public ModelViewModel Parent { get; }


        public event PropertyChangedEventHandler? PropertyChanged;


    }
}