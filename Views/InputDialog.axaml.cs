using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using raptor;

namespace RAPTOR_Avalonia_MVVM.Views
{
    public partial class InputDialog : Window
    {
        public InputDialog(){

        }
        public InputDialog(Parallelogram p)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new RAPTOR_Avalonia_MVVM.ViewModels.InputDialogViewModel(p, this);

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
