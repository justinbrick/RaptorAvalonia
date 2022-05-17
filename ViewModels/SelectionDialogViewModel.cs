using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using raptor;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Avalonia.Input;
using ReactiveUI;
using System.Reactive;

namespace RAPTOR_Avalonia_MVVM.ViewModels
{
    public class SelectionDialogViewModel : ViewModelBase
    {
        public SelectionDialogViewModel() {
            this.text = "";
        }

        public SelectionDialogViewModel(IF_Control i, Window w) {
            this.text = "";
            this.i = i;
            this.w = w;
        }
        public IF_Control i;
        public Window w;
        public bool modified = false;
        public bool runningState = false;

        public string text = "";
        public string Text   // property
        {
            get { return text; }   // get method
            set { this.RaiseAndSetIfChanged(ref text,value); }  // set method
        }

        public string selection = "";

        public string setSelection{
            get { return selection; }
            set {this.RaiseAndSetIfChanged(ref selection, value);}
        }

        public void OnDoneCommand(){
            i.text_str = setSelection;
            Text += "Done Selection\n";
            w.Close();
            //Console.WriteLine("hi there dude");
        }

    }

}