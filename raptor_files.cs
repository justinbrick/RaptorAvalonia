﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using raptor;
using RAPTOR_Avalonia_MVVM;
using System.Collections;
using RAPTOR_Avalonia_MVVM.ViewModels;
using RAPTOR_Avalonia_MVVM.Views;
using System.Collections.ObjectModel;
using RAPTOR_Avalonia_MVVM.Controls;
using Avalonia.Threading;
using System.IO;
using Avalonia.Controls;
using numbers;

namespace RAPTOR_Avalonia_MVVM
{
	public class raptor_files
	{

		public static bool Input_Is_Redirected = false;
		public static bool Output_Is_Redirected = false;
		private static StreamReader input_stream;
		private static StreamWriter output_stream;

		public static void writeln(string text)
        {
			output_stream.Write(text);
		}
		public static void write(string text)
        {
			output_stream.WriteLine(text);
		}
		public static string read()
        {
			return input_stream.ReadLine();
        }
		public static void redirect_output(Value filename)
        {
			//if (Output_Is_Redirected)
   //         {
			//	Stop_Redirect_Output();
   //         }

			if (filename.o is string)
			{
				output_stream = new StreamWriter(filename.o as string);
				Output_Is_Redirected = true;
			}
			else
			{
				if (filename.ToDouble() == 1)
				{
					SaveFileDialog fileChooser = new SaveFileDialog();
					List<FileDialogFilter> Filters = new List<FileDialogFilter>();
					FileDialogFilter filter = new FileDialogFilter();
					List<string> extension = new List<string>();
					extension.Add("txt");
					filter.Extensions = extension;
					filter.Name = "Text Files";
					Filters.Add(filter);
					fileChooser.Filters = Filters;

					fileChooser.DefaultExtension = "txt";

					string ans = "";
					Dispatcher.UIThread.InvokeAsync(async () =>
					{
						ans = await fileChooser.ShowAsync(MainWindow.topWindow);

					}).Wait(-1);

					if (ans == null || ans == "")
					{
						return;
					}

					output_stream = new StreamWriter(ans);
				}
				else
				{
					Stop_Redirect_Output();
					Output_Is_Redirected = false;
				}

			}

		}

		public static void redirect_output_append(Value filename)
		{
			//if (Output_Is_Redirected)
			//{
			//	Stop_Redirect_Output();
			//}

			if (filename.o is string s)
			{
				output_stream = new StreamWriter(s, true);
				Output_Is_Redirected = true;
			}
			else
			{
				if (filename.ToDouble() == 1)
				{
					string file = openFileFunction();

					output_stream = new StreamWriter(file, true);
					Output_Is_Redirected = true;
				}
				else
				{
					Stop_Redirect_Output();
					Output_Is_Redirected = false;
				}

			}

		}

		public static void redirect_input(Value filename)
		{
            //if (Input_Is_Redirected)
            //{
            //    Stop_Redirect_Input();
            //}

            if (filename.o is string s)
			{
				input_stream = new StreamReader(s);
				Input_Is_Redirected = true;
			}
            else
            {
				if(filename.ToDouble() == 1) {

					string file = openFileFunction();

					input_stream = new StreamReader(file);
					Input_Is_Redirected = true;
				}
				else
				{
					Stop_Redirect_Input();
					Input_Is_Redirected = false;
				}

			}
		}

		public static string openFileFunction()
        {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filters.Add(new FileDialogFilter() { Name = "Text Files", Extensions = { "txt" } });
			dialog.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });
			dialog.AllowMultiple = false;

			string[] result = { };

			Dispatcher.UIThread.InvokeAsync(async () =>
			{
				result = await dialog.ShowAsync(MainWindow.topWindow);

			}).Wait(-1);

			if (result == null || result[0] == "")
			{
				return "";
			}

			return result[0];

		}

		public static bool output_redirected()
        {
			return Output_Is_Redirected;
        }

		public static bool input_redirected()
		{
			return Input_Is_Redirected;
		}

		public static void Stop_Redirect_Input()
		{
			if (Input_Is_Redirected)
			{
				Input_Is_Redirected = false;
				if (input_stream != null)
				{
					input_stream.Close();
				}
			}
		}

		public static void Stop_Redirect_Output()
		{
			if (Output_Is_Redirected)
			{
				Output_Is_Redirected = false;
				if (output_stream != null)
				{
					output_stream.Close();
				}
			}
		}

		public static void close_files()
        {
			Stop_Redirect_Input();
			Stop_Redirect_Output();
		}

	}
}

