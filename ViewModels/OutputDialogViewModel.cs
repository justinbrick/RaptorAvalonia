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
using interpreter;

namespace RAPTOR_Avalonia_MVVM.ViewModels
{
    public class OutputDialogViewModel : ViewModelBase
    {
        public OutputDialogViewModel() {
            this.text = "";
        }
        public OutputDialogViewModel(Parallelogram p, Window w) {
            this.text = "";
            this.p = p;
            this.w = w;
        }
        public Parallelogram p;
        public Window w;
        public bool modified = false;
        public bool runningState = false;

        public string text = "";
        public string Text   // property
        {
            get { return text; }   // get method
            set { this.RaiseAndSetIfChanged(ref text,value); }  // set method
        }

        public string output = "";
        public string getOutput{
            get { return output; }
            set { this.RaiseAndSetIfChanged(ref output, value); }
        }

        public void OnDoneCommand(){
            p.text_str = getOutput;
            Text += "Done Output\n";
            w.Close();
            //Console.WriteLine("hi there dude");
        }

    }

}