using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Avalonia.OpenGL.Surfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using raptor;
using ReactiveUI;

namespace numbers
{
    public enum Value_Kind
    {
        Number_Kind, String_Kind, Character_Kind, Object_Kind,
        Ref_1D, Ref_2D
    };

    public class value
    {

    }

    public class Value
    {
        public object? o;

        public Value(Value copy)
        {
            switch (copy.o)
            {
                case string s:
                    o = new string(s);
                    break;
                case double d:
                    o = d;
                    break;
                case char c:
                    o = c;
                    break;
                default:
                    o = copy.o;
                    break;
            }
        }

        public Value()
        {
            o = null; 
        }

        public Value(object? instance)
        {
            switch (instance)
            {
                case int i:
                    o = (double)i;
                    break;
                case long l:
                    o = (double)l;
                    break;
                case float f:
                    o = (double)f;
                    break;
                case bool b:
                    o = b ? 1.0 : 0.0;
                    break;
                default:
                    o = instance;
                    break;
            }
        }

        // Conversion Functions
        public object? ToObject()
        {
            return o;
        }

        public int ToInteger()
        {
            switch (o)
            {
                case char c:
                    return (int)c;
                case double d:
                    return (int)d;
                default:
                    return 0;
            }
        }

        public char ToCharacter()
        {
            if (o is char c) return c;
            return ' ';
        }

        override public string ToString()
        {
            return o?.ToString() ?? "";
        }

        public double ToDouble()
        {
            switch (o)
            {
                case char c:
                    return (double)c;
                case double d:
                    return d;
                default:
                    return 0;
            }
        }

        public bool IsNumber()
        {
            return o is double;
        }

        public bool IsCharacter()
        {
            return o is char;
        }

        public bool IsString()
        {
            return o is string;
        }

        public bool IsInteger()
        {
            const double epsilon = 0.00001;
            if (o is double d)
            {
               return ((int)d-d) < epsilon;
            }
            return false;
        }

        // Comparison Operators
        public static bool operator ==(Value first, Value second)
        {
            if (first.o is null || second.o is null) return first.o == second.o;
            if (first.o.GetType() != second.o.GetType()) return false;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue == (double)second.o;
                case string firstValue:
                    return firstValue == (string)second.o;
                case char firstValue:
                    return firstValue == (char)second.o;
            }
            return false;
        }
        public static bool operator !=(Value first, Value second)
        {
            if (first.o is null || second.o is null) return first.o != second.o;
            if (first.o.GetType() != second.o.GetType()) return true;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue != (double)second.o;
                case string firstValue:
                    return firstValue != (string)second.o;
                case char firstValue:
                    return firstValue != (char)second.o;
            }
            return false;
        }

        public static bool operator >(Value first, Value second)
        {
            if (first.o is null || second.o is null) return false;
            if (first.o.GetType() != second.o.GetType()) return false;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue > (double)second.o;
                case string firstValue:
                    return firstValue.CompareTo((string)second.o) > 0;
                case char firstValue:
                    return firstValue > (char)second.o;
            }
            return false;
        }

        public static bool operator <(Value first, Value second)
        {
            if (first.o is null || second.o is null) return false;
            if (first.o.GetType() != second.o.GetType()) return false;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue < (double)second.o;
                case string firstValue:
                    return firstValue.CompareTo((string)second.o) < 0;
                case char firstValue:
                    return firstValue < (char)second.o;
            }
            return false;
        }

        public static bool operator >=(Value first, Value second)
        {
            if (first.o is null || second.o is null) return false;
            if (first.o.GetType() != second.o.GetType()) return false;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue >= (double)second.o;
                case string firstValue:
                    return firstValue.CompareTo((string)second.o) >= 0;
                case char firstValue:
                    return firstValue >= (char)second.o;
            }
            return false;
        }

        public static bool operator <=(Value first, Value second)
        {
            if (first.o is null || second.o is null) return false;
            if (first.o.GetType() != second.o.GetType()) return false;

            switch (first.o)
            {
                case double firstValue:
                    return firstValue <= (double)second.o;
                case string firstValue:
                    return firstValue.CompareTo((string)second.o) <= 0;
                case char firstValue:
                    return firstValue <= (char)second.o;
            }
            return false;
        }

        public override bool Equals(object? obj)
        {
            return (obj is Value v) && v == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Arithmetic Operators

        public static Value operator+(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot add null value");
            switch (first.o)
            {
                case double d:
                    if (second.o is double) return new Value(d + (double)second.o);
                    if (second.o is string) return new Value(d + ((string)second.o).Replace("\"", ""));
                    if (second.o is char) return new Value((char)(d+(char)second.o));
                    break;
                case char c:
                    if (second.o is double) return new Value((char)(c + (int)second.o));
                    if (second.o is string) return new Value(c + (string)second.o);
                    if (second.o is char) return new Value(c + (char)second.o);
                    break;
                case string s:
                    s = s.Replace("\"", "");
                    if (second.o is double) return new Value(s + (double)second.o);
                    if (second.o is string) return new Value(s + ((string)second.o).Replace("\"", ""));
                    if (second.o is char) return new Value(s + (char)second.o);
                    break;
                default:
                    break;
            }
            throw new Exception($"Cannot add values of type {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value operator-(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot add null value");
            if ((first.o is double || first.o is char) && (second.o is double || second.o is char)) return new Value((double)first.o + (double)second.o);
            throw new Exception($"Cannot add values of type {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value operator*(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot multiply null value");
            if (first.o is double && second.o is double) return new Value((double)first.o * (double)second.o);
            throw new Exception($"Cannot multiply values of type {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value operator /(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot divide null value");
            if (first.o is double && second.o is double) return new Value((double)first.o / (double)second.o);
            throw new Exception($"Cannot divide values of type {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value operator %(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot divide null value");
            if (first.o is double && second.o is double) return new Value((double)first.o % (double)second.o);
            throw new Exception($"Cannot modulo values of type {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        // Non-overloads

        public Value Pow(Value exp)
        {
            if (this.o is null || exp.o is null) throw new Exception("Cannot use null exponent!");
            if (this.o is double && exp.o is double) return new Value(Math.Pow((double)this.o, (double)exp.o));
            throw new Exception($"Cannot exponent values of type {this.o.GetType().Name} and {exp.o.GetType().Name}");
        }
    }

    public class Numbers
    {
        private const int DEFAULT_PRECISION = 4;
        private static bool Precision_Set = false;
        private static int Precision = DEFAULT_PRECISION;
        static void Set_Precision(int i)
        {
            if (i<0)
            {
                Precision_Set = false;
            }
            else
            {
                Precision_Set = true;
                Precision = i;
            }
        }
        public static Value Zero = new Value(0);
        public static Value One = new Value(1);
        public static Value Pi = new Value(Math.PI);
        public static Value E = new Value(Math.E);
        public static Value Two_Pi = new Value(Math.PI * 2);
        public static Value Null_Ptr = new Value();

        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static object? object_of(Value v)
        {
            return v.ToObject();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static int integer_of(Value v)
        {
            return v.ToInteger();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static int character_of(Value v)
        {
            return v.ToCharacter();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static string string_of(Value v)
        {
            return v.ToString();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static double long_float_of(Value v)
        {
            return v.ToDouble();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static bool is_number(Value v)
        {
            return v.IsNumber();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static bool is_string(Value v)
        {
            return v.IsString();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static bool is_integer (Value v)
        {
            return v.IsInteger();
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static string number_string(Value v)
        {
            string format = Precision_Set ? "{0:" + Precision + "f}" : "{0:f}"; // Either a formatted string with precision, or a formatted string with no precision.
            var d = v.ToDouble();
            return String.Format(format, d);            
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_2d_ref(object o)
        {
            return new Value(o);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_1d_ref(object o)
        {
            return new Value(o);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static string object_image(Value v)
        {
            return "[" + (v.o?.GetHashCode() ?? 0) + "]";
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static string msstring_view_image(Value v)
        {
            switch (v.o)
            {
                case double d:
                    return $"\"{d}\"";
                case char c:
                    return $"'{c}'";
                default:
                    return v.ToString();
            }
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static string msstring_console_view_image(Value v)
        {

            switch (v.o)
            {
                case double d:
                    return number_string(v);
                default:
                    return v.ToString();
            }
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_correct_number_value_type(string s)
        {
            try
            {
                if (s.Contains(".")) return new Value(Convert.ToDouble(s));
                return new Value(Convert.ToInt32(s));
            }
            catch (Exception e)
            {
                return new Value(s);
            }

            throw new Exception("Bad string");
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_value__5(string s)
        {
            return new Value(Convert.ToDouble(s));
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_value__4(bool b)
        {
            return new Value(b ? 1 : 0);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]
        public static Value make_value__3(int index)
        {
            return new Value(index);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static Value make_value__2(double v)
        {
            return new Value(v);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static Value make_object_value(object v)
        {
            return new Value(v);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static bool Oeq(Value first, Value second)
        {
            return first == second;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static bool Ogt(Value first, Value second)
        { 
            return first > second;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static bool Oge(Value first, Value second)
        {
            return first >= second;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static bool Olt(Value first, Value second)
        {
            return first < second;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static bool Ole(Value first, Value second)
        {
            return first <= second;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static int length_of(Value variable_Value)
        {
            return variable_Value.ToString().Length;
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static Value make_string_value(string new_string)
        {
            return new Value(new_string);
        }
        [Obsolete("This function is no longer in use, please use the methods found on the Value instance.")]

        public static Value make_character_value(char new_char)
        {
            return new Value(new_char);
        }

        public static bool is_character(Value f)
        {
            return f.IsCharacter();
        }

        public static void copy(Value src, Value dest) {
            // If these are struct values, we need to copy them over. Otherwise, they maintain an object reference.
            switch (src.o)
            {
                case string s:
                    dest.o = new string(s);
                    break;
                case char c:
                    dest.o = c;
                    break;
                case double d:
                    dest.o = d;
                    break;
                default:
                    dest.o = src.o;
                    break;
            }
        }

        public static Value addValues(Value first, Value second)
        {
            return first + second;
        }
        
        public static Value subValues(Value first, Value second)
        {
            return first - second;
        }

        public static Value negValue(Value first)
        {
            if (first.o is double d)
            {
                return new Value(-d);
            }
            throw new Exception("Cannot negate type: [" + first.o?.GetType().Name ?? "null" + "]");
        }

        public static Value multValues(Value first, Value second)
        {
            return first * second;
        }

        public static Value exponValues(Value first, Value second)
        {
            return first.Pow(second);
        }

        public static Value divValues(Value first, Value second)
        {
            return first / second;
        }

        public static Value modValues(Value first, Value second)
        {
            return first % second;
        }

        public static Value remValues(Value first, Value second)
        {
            return first % second;
        }

        public static Value findMax(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot find max of null value");
            if (first.IsNumber() && second.IsNumber()) return new Value(Math.Max((double)first.o, (double)second.o));
            throw new Exception($"Can't find the max of values with types {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value findMin(Value first, Value second)
        {
            if (first.o is null || second.o is null) throw new Exception("Cannot find min of null value");
            if (first.IsNumber() && second.IsNumber()) return new Value(Math.Min((double)first.o, (double)second.o));
            throw new Exception($"Can't find the Min of values with types {first.o.GetType().Name} and {second.o.GetType().Name}");
        }

        public static Value Sinh(Value first) {
            if (first.o is double d) return new Value(Math.Sinh(d));
            throw new Exception($"Cannot find Sinh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Tanh(Value first)
        {
            if (first.o is double d) return new Value(Math.Tanh(d));
            throw new Exception($"Cannot find Tanh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Cosh(Value first)
        {
            if (first.o is double d) return new Value(Math.Cosh(d));
            throw new Exception($"Cannot find Cosh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcSinh(Value first)
        {
            if (first.o is double d) return new Value(Math.Asinh(d));
            throw new Exception($"Cannot find Asinh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcTanh(Value first)
        {
            if (first.o is double d) return new Value(Math.Atanh(d));
            throw new Exception($"Cannot find Atanh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcCosh(Value first)
        {
            if (first.o is double d) return new Value(Math.Acosh(d));
            throw new Exception($"Cannot find Acosh of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Coth(Value first)
        {
            if (first.o is double d) return new Value(1/Math.Tanh(d));
            throw new Exception($"Cannot find Coth of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcCoth(Value first)
        {
            if (first.o is double d) return new Value(Math.Atanh(d));
            throw new Exception($"Cannot find Acoth of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Sqrt(Value first)
        {
            if (first.o is double d) return new Value(Math.Sqrt(d));
            throw new Exception($"Cannot find Sqrt of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Floor(Value first)
        {
            if (first.o is double d) return new Value(Math.Floor(d));
            throw new Exception($"Cannot find Floor of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Ceiling(Value first)
        {
            if (first.o is double d) return new Value(Math.Ceiling(d));
            throw new Exception($"Cannot find Ceiling of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Abs(Value first)
        {
            if (first.o is double d) return new Value(Math.Abs(d));
            throw new Exception($"Cannot find Abs of object of type {first.o?.GetType().Name ?? "null"}");
        }

        public static Value Log(Value first, Value second)
        {
            if (first.o is double d) return new Value(Math.Log(d));
            throw new Exception($"Cannot find Log of objects of type {first.o?.GetType().Name ?? "null"} and {second.o?.GetType().Name ?? "null"}");
        }

        public static Value Sin(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Sin((double)first.o));
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Sin(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find Sin of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value Cos(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Cos(d1));
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Cos(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find Cos of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value Tan(Value first, Value? second = null)
        {

            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Tan(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Tan(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find Tan of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value Cot(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(1 / Math.Tan(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(1 / Math.Tan(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find Cot of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcSin(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Asin(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Asin(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find ArcSin of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcCos(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Acos(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Acos(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find ArcCos of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcTan(Value first, Value? second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(Math.Atan(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(Math.Atan(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find ArcTan of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

        public static Value ArcCot(Value first, Value second = null)
        {
            if (first.o is double d1)
            {
                if (second is null)
                {
                    return new Value(1 / Math.Atan(d1)); //Math.Sin?
                }
                if (second.o is double d2)
                {
                    return new Value(1 / Math.Atan(d1 / d2 * Math.PI * 2));
                }
            }
            throw new Exception($"Cannot find ArcTan of objects of type {first.o?.GetType().Name ?? "null"} and {second?.o?.GetType().Name ?? "null"}");
        }

    }
}
