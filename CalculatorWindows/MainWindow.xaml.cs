using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.Globalization;
using System;
using System.IO;
using System.Windows;

namespace CalculatorWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        float MemoriVariable = new float();
        float Totalizator = new float();
        bool CascadaState = false;
        bool isDigitGouping ;
        bool digitGoupingUk ;

        //private System.Windows.Forms.NotifyIcon MyNotifyIcon;

        operation PastOperation = operation.Null;
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("Calculator.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            menu.MenuItems.Add("Exit", new EventHandler(Exit));
            ni.ContextMenu = menu;

          
            string line;
            int index = 0;
            System.IO.StreamReader file =
                new System.IO.StreamReader("../../Config.txt");

            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                if(index==0)
                {
                    isDigitGouping = bool.Parse(line);

                }
                if(index==1)
                {
                    digitGoupingUk = bool.Parse(line);
                }
                index++;

            }

            file.Close();
            System.Console.ReadLine();
            
        }
        private void UpdateDigit()
        {

            List<String> list = new List<string>();
            list.Add(isDigitGouping.ToString());
            list.Add(digitGoupingUk.ToString());

            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine("E:/MVP/CalculatorWindows/CalculatorWindows", "Config.txt")))
            {
                foreach (string l in list)
                    outputFile.WriteLine(l);
            }

        }
        private void Set_Ro(object sender, RoutedEventArgs e)
        {
            digitGoupingUk = false;
            UpdateDigit();
        }

        private void Set_Uk(object sender, RoutedEventArgs e)
        {
            digitGoupingUk = true;
            UpdateDigit();
        }

        private void Set_DG_Off(object sender, RoutedEventArgs e)
        {
            isDigitGouping = false;
            UpdateDigit();
        }

        private void Set_DG_On(object sender, RoutedEventArgs e)
        {
            isDigitGouping = true;
            UpdateDigit();
        }

        private void Exit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
        private enum operation{ Null=0,Plus = 1, Minus= 2, Div=3, Prod=4,Equals=5}

        private void doOperation(operation curentOp, float curentValue)
        {
            if(PastOperation.Equals(operation.Null))
            {
                Console.WriteLine(Totalizator);

                Totalizator = curentValue;
            }else
           
            if (PastOperation.Equals(operation.Plus))
            {
                Console.WriteLine("initial" + Totalizator);

                Totalizator = Totalizator + curentValue;
                Console.WriteLine("final" + Totalizator);

            }else
            if (PastOperation.Equals(operation.Minus))
            {
                Console.WriteLine("initial" + Totalizator);

                Totalizator = Totalizator - curentValue;
                Console.WriteLine("final" + Totalizator);

            }else
            if (PastOperation.Equals(operation.Div))
            {
                Console.WriteLine("initial" + Totalizator);

                Totalizator = Totalizator / curentValue;
                Console.WriteLine("final" + Totalizator);


            }else
            if (PastOperation.Equals(operation.Prod))
            {
                Console.WriteLine("initial" + Totalizator);

                Totalizator = Totalizator * curentValue;
                Console.WriteLine("final" + Totalizator);
            }
         
            PastOperation = curentOp;


        }

        private void doEqual(float curentValue)
        {
            
                doOperation(PastOperation, curentValue);
                PastOperation = operation.Null;

                display.Text = digitGroupingEqal(Totalizator.ToString());
            
        }

        private String digitGrouping(String text)
        {
            String result;
            if(isDigitGouping==true)
            {
                if(digitGoupingUk==true)
                {
                    result = double.Parse(text).ToString("#,##0", CultureInfo.CreateSpecificCulture("en-UK"));
                    return result;
                }
                else
                {
                    result= double.Parse(text).ToString("#.##0", CultureInfo.CreateSpecificCulture("ro-RO"));
                    return result;
                }
            }
            return text;
        }

        private String digitGroupingEqal(String text)
        {
            String result;
            if (isDigitGouping == true)
            {
                if (digitGoupingUk == true)
                {
                    result = double.Parse(text).ToString("N1", CultureInfo.CreateSpecificCulture("en-UK"));
                    return result;
                }
                else
                {
                    result = double.Parse(text).ToString("N1", CultureInfo.CreateSpecificCulture("ro-RO"));
                    return result;
                }
            }
            return text;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


        Button button = (Button)sender;
            //display.Text += button.Content.ToString();
            if(CascadaState!=true)
            {

                if (button.Content.ToString() == "÷")
                {

                    doOperation(operation.Div, float.Parse(display.Text.ToString()));


                    display.Clear();

                }
                else
                if (button.Content.ToString() == "*")
                {
                    doOperation(operation.Prod, float.Parse(display.Text.ToString()));
                    display.Clear();

                }
                else
                if (button.Content.ToString() == "-")
                {
                    doOperation(operation.Minus, float.Parse(display.Text.ToString()));
                    display.Clear();

                }
                else
                if (button.Content.ToString() == "+")
                {
                    doOperation(operation.Plus, float.Parse(display.Text.ToString()));
                    display.Clear();
                }
                else
                if (button.Content.ToString() == "√")
                {


                    double rad = double.Parse(display.Text.ToString());
                    display.Clear();
                    if (rad < 0)
                    {
                        display.Text = "Nu se poate executa radical din nr <0";
                    }
                    else
                    {
                        Totalizator = float.Parse(Math.Sqrt(d: rad).ToString());
                        display.Text = Totalizator.ToString();

                    }

                }
                else
                {
                    display.Text += button.Content.ToString();
                    display.Text = digitGrouping(display.Text);

                }
            }
            else
            {
                display.Text += button.Content.ToString();
            }
        }

        private void Off_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void R_Click(object sender, RoutedEventArgs e)
        {

            if (display.Text.Length > 0)
            {
                display.Text = display.Text.Substring(0, display.Text.Length - 1);
            }
        }

        private void ChangeSign_Click(object sender, RoutedEventArgs e)
        {
            if (display.Text.Length > 0)
            {
                string text = display.Text;
                float numVal = float.Parse(text);
                

                    numVal = numVal * (-1);

                
                display.Text = numVal.ToString();
            }
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            String text = display.Text;
            for(int i = text.Length-1; i>=0;i--)
            {
                Console.WriteLine(text[i]);
                if(IsDigitsOnly(text[i].ToString())==false)
                {
                    break;
                }
                display.Text = display.Text.Substring(0, display.Text.Length - 1);
            }
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            display.Clear();
        }

        private void Result_click(object sender, RoutedEventArgs e)
        {
            if (CascadaState == true)
            {
                try
                {
                    calculate();
                }
                catch (Exception exception)
                {
                    display.Text = "Error!";
                }
            }
            else
            {
                doEqual( float.Parse(display.Text.ToString()));
            }
        }
        private Stack<String> sign  = new Stack<string>();
        private Stack<float> numbers= new Stack<float>();
        private void calculate()
        {
            
               String imputText = display.Text;
            String numberTmp ="";
            for(Int32 i =0;i<imputText.Length;i++)
            {

                
                if (IsDigitsOnly(imputText[i] + ""))
                {
                    numberTmp = numberTmp + imputText[i];
                    Console.WriteLine(numberTmp);
                }
                else
                {
                    float nr = float.Parse(numberTmp);
                    numbers.Push(nr);
                    numberTmp = "";
                    if (!sign.Count.Equals(0))
                    {
                        if(sign.Peek().Equals("*")|| sign.Peek().Equals("÷"))
                        {
                            float val2 = numbers.Peek();
                            numbers.Pop();
                            float val1 = numbers.Peek();
                            numbers.Pop();
                            String operation = sign.Peek();
                            sign.Pop();
                            if (operation.Equals("*"))
                                val1 = val1 * val2;
                            else
                                val1 = val1 / val2;
                            numbers.Push(val1);
                            

                        }
                    }
                    sign.Push(imputText[i].ToString());

                }

            }
            float nr2 = int.Parse(numberTmp);
            numbers.Push(nr2);
            for (Int32 i = 0; i < sign.Count; i++)
            {
                float val2 = numbers.Peek();
                numbers.Pop();
                float val1 = numbers.Peek();
                numbers.Pop();
                String operation = sign.Peek();
                sign.Pop();
                if (operation.Equals("*"))
                {
                    val1 = val1 * val2;
                }
                if (operation.Equals("÷"))
                {
                    val1 = val1 / val2;
                }
                if (operation.Equals("+"))
                {
                    val1 = val1 + val2;
                }
                if (operation.Equals("-"))
                {
                    val1 = val1 - val2;
                }
                numbers.Push(val1);
            }



               
            display.Text = imputText + "=" + numbers.Peek();
        }

        private void Reciproc_click(object sender, RoutedEventArgs e)
        {
            string text = display.Text;
            if(IsDigitsOnly(text) ==true)
            {
                display.Clear();
                float numVal;
                try
                {
                    numVal = float.Parse(text);
                    if (numVal < 1)
                    {
                        display.Text = Reciprocal(numVal).ToString();
                    }
                    else
                        display.Text = (numVal = 1 / numVal).ToString();
                }
                catch
                {
                    display.Text = "Nu se poate executa!";
                }
                
            }
            else
            {
                display.Clear();
                display.Text = "Error!";
            }
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    if (c != '.')
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        bool IsDigitsOnlyFloat(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    if(c!='.')
                        return false;
            }

            return true;
        }
        Dictionary<double, double> reciprocals = new Dictionary<double, double>();

        private double Reciprocal(double val)
        {
            double reciprocal;
            if (reciprocals.TryGetValue(val, out reciprocal))
            {
                return reciprocal;
            }

            reciprocal = 1 / val;
            reciprocals[reciprocal] = val;
            return reciprocal;
        }

        private void MC_Click(object sender, RoutedEventArgs e)
        {
            MemoriVariable = new float();
        }

        private void MR_Click(object sender, RoutedEventArgs e)
        {
            display.Clear();
            display.Text = MemoriVariable.ToString();
        }

        private void MPLUS_Click(object sender, RoutedEventArgs e)
        {
            MemoriVariable = MemoriVariable + float.Parse(display.Text);
        }

        private void MMINUS_Click(object sender, RoutedEventArgs e)
        {
            MemoriVariable = MemoriVariable - float.Parse(display.Text);
        }

        private void MS_Click(object sender, RoutedEventArgs e)
        {
            MemoriVariable = float.Parse(display.Text);
        }

        private void M_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pow_Click(object sender, RoutedEventArgs e)
        {
            if(CascadaState==false)
            {
                String text = display.Text;
                float num = float.Parse(text);
                num = num * num;
                display.Text = num.ToString();
            }
            else
            {
                String text = display.Text;
                float num = float.Parse(text);
                num = num * num;
                display.Text += "²=" + num.ToString();
            }
           
        }

        private void Display_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Off_Cascade(object sender, RoutedEventArgs e)
        {
           
            MenuItem menuItem = (MenuItem)sender;
            if(menuItem.IsChecked.Equals(true))
            {
                CascadaState = true;
            }

            if (menuItem.IsChecked.Equals(false))
            {
                CascadaState = false;
            }
        }

        

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            if(Totalizator!=0)
            {

            
                string text = display.Text;
                if (IsDigitsOnly(text) == true)
                {
                    display.Clear();
                    float numVal;
                    try
                    {
                        numVal = float.Parse(text);
                    
                            display.Text = (Totalizator*(numVal/100)).ToString();
                   
                   
                    }
                    catch
                    {
                        display.Text = "Nu se poate executa!";
                    }

                }
                else
                {
                    display.Clear();
                    display.Text = "Error!";
                }

            }
            else
            {
                display.Text = "0";
            }
        }
    }
}
