using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using raptor;

namespace RAPTOR_Avalonia_MVVM.Views
{
    public partial class LoopDialog : Window
    {
        public LoopDialog(){

        }
        public LoopDialog(Loop l, bool modding)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new RAPTOR_Avalonia_MVVM.ViewModels.LoopDialogViewModel(l, this, modding);

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
