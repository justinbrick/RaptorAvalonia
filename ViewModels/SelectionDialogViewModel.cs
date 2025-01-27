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
    public class SelectionDialogViewModel : ViewModelBase
    {
        public SelectionDialogViewModel() {
            this.text = "";
        }

        public SelectionDialogViewModel(IF_Control i, Window w, bool modding) {
            this.text = "";
            this.i = i;
            this.w = w;
            this.modding = modding;

            if (modding)
            {
                selection = i.text_str;
            }
        }
        public IF_Control i;
        public Window w;
        public bool modified = false;
        public bool runningState = false;
        public bool modding;

        public string text = "";
        public string Text   // property
        {
            get { return text; }   // get method
            set { this.RaiseAndSetIfChanged(ref text,value);}  // set method
        }

        public string selection = "";

        public string setSelection{
            get { return selection; }
            set {   this.RaiseAndSetIfChanged(ref selection, value);
                    setSuggestions = getSuggestion();
                    if(setSuggestions.Count > 0){
                        setIndex = setSuggestions[0];
                    }    
                }
        }

        public string suggestionIndex = "";
        public string setIndex
        {
            get { return suggestionIndex; }
            set { this.RaiseAndSetIfChanged(ref suggestionIndex, value); }
        }

        public ObservableCollection<string> suggestions = new ObservableCollection<string>();
        public ObservableCollection<string> setSuggestions
        {
            get { return suggestions; }
            set { this.RaiseAndSetIfChanged(ref suggestions, value); }
        }

        public ObservableCollection<string> getSuggestion()
        {
            Suggestions s = new Suggestions(i, setSelection, false, getSubchart());
            return s.getSuggestions();
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

        public void OnDoneCommand(){
            Syntax_Result res = interpreter_pkg.conditional_syntax(setSelection);
            if(res.valid){
                Undo_Stack.Make_Undoable(getSubchart());
                i.text_str = setSelection;
                i.parse_tree = res.tree;
                Text += "Done Selection\n";
                MainWindowViewModel.GetMainWindowViewModel().modified = true;
                w.Close();
            } else {
                Text = res.message;
            }
        }

    }

}