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
    public class CallDialogViewModel : ViewModelBase
    {
        public CallDialogViewModel() {
            this.text = "";
        }

        public CallDialogViewModel(Rectangle r, Window w, bool modding){
            this.text = "";
            this.r = r;
            this.w = w;
            this.modding = modding;

            if (modding)
            {
                procedure = r.text_str;
            }
        }
        public Rectangle r;
        public Window w;
        public bool modding;
        public bool modified = false;
        public bool runningState = false;

        public string procedure = "";

        public string setProcedure{
            get { return procedure; }
            set {   this.RaiseAndSetIfChanged(ref procedure, value);
                    setSuggestions = getSuggestion();
                    if(setSuggestions.Count > 0){
                        setIndex = setSuggestions[0];
                    }
                }
        }
        public string text = "";
        public string Text   // property
        {
            get { return text; }   // get method
            set { this.RaiseAndSetIfChanged(ref text,value); }  // set method
        }

        public ObservableCollection<string> suggestions = new ObservableCollection<string>();
        public ObservableCollection<string> setSuggestions
        {
            get { return suggestions; }
            set { this.RaiseAndSetIfChanged(ref suggestions, value); }
        }

        public Subchart getSubchart(){
            MainWindowViewModel mw = MainWindowViewModel.GetMainWindowViewModel();
            ObservableCollection<Variable> vars = mw.theVariables;
            ObservableCollection<Subchart> sc = mw.theTabs;
            if(sc.Count == 1){
                return sc[0];
            }
            else{
                int i = mw.viewTab;
                return sc[i];
            }
        }

        public string suggestionIndex = "";
        public string setIndex
        {
            get { return suggestionIndex; }
            set { this.RaiseAndSetIfChanged(ref suggestionIndex, value); }
        }

        public ObservableCollection<string> getSuggestion()
        {
            Suggestions s = new Suggestions(r, setProcedure, false, getSubchart());
            return s.getSuggestions();
        }

        public void OnDoneCommand(){
            Syntax_Result res = interpreter_pkg.call_syntax(setProcedure);
            if(res.valid){
                Undo_Stack.Make_Undoable(getSubchart());
                r.text_str = setProcedure;
                r.parse_tree = res.tree;
                Text += "Done Call\n";
                MainWindowViewModel.GetMainWindowViewModel().modified = true;
                w.Close();
            } else {
                Text = res.message;
            }
        }

    }

}