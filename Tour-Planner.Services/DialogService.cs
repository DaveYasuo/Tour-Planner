using System;
using System.Collections.Generic;
using System.Windows;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class DialogService : IDialogService
    {
        private readonly Window _owner;

        public DialogService(Window owner)
        {
            _owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public IDictionary<Type, Type> Mappings { get; }


        public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }
            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialog dialog = (IDialog)Activator.CreateInstance(viewType)!;

            void Handler(object? sender, DialogCloseRequestedEventArgs e)
            {
                viewModel.CloseRequested -= Handler;
                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            }

            viewModel.CloseRequested += Handler;
            dialog.DataContext = viewModel;
            dialog.Owner = _owner;
            return dialog.ShowDialog();
        }
    }

}
