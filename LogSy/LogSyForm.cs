using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

namespace LogSy
{
    public partial class LogSyForm : Form
    {
        List<HashSet<String>> conjList = new List<HashSet<String>>();
        HashSet<char> alfabeto = new HashSet<char>();
        List<char> operaciones = new List<char>();

        Stack<int> returnParen = new Stack<int>();
        Stack<HashSet<String>> auxs = new Stack<HashSet<String>>();

        String expresion = "";
        int compensation = 0;

        HashSet<String> setReturn = new HashSet<String>();
        public LogSyForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('⋃');
                getConjuntos();
                InfoBox.Text += " ⋃ ";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('⋂');
                getConjuntos();
                InfoBox.Text += " ⋂ ";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('-');
                getConjuntos();
                InfoBox.Text += " - ";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count-1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('∆');
                getConjuntos();
                InfoBox.Text += " ∆ ";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('ᶜ');
                getConjuntos();
                InfoBox.Text += " ᶜ ";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('x');
                getConjuntos();
                InfoBox.Text += " x ";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HashSet<string> set = operate();
            string salida = String.Join(", ", set);
            string salidaCheck = salida.Distinct().ToString();

            //alfabeto = AlfabetoBox.Text.ToHashSet();
            if (comprobarLenguaje(set.ToArray(), AlfabetoBox.Text)) SalidaBox.Text = salida;
            else SalidaBox.Text = "Error: Los elementos del lenguaje no coinciden con el alfabeto";

            //String salida = String.Join(", ", operate());
            //HashSet<char> salidaCheck = String.Join("", salida.Split(',', ' ')).ToHashSet();
            //salidaCheck = (HashSet<char>)salidaCheck.Distinct();
            //alfabeto = AlfabetoBox.Text.ToHashSet();
            //if (lexicCheck(salidaCheck)) SalidaBox.Text = salida;
            //else SalidaBox.Text = "Error: Los elementos del lenguaje no coinciden con el alfabeto";

            expresion = InfoBox.Text;
            InfoBox.Clear();
            setReturn.Clear();
            conjList.Clear();
            operaciones.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            conjList.Add(new HashSet<string> { "(" });
            InfoBox.Text += "(";
            compensation++;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            getConjuntos();
            conjList.Add(new HashSet<string> { ")" });
            InfoBox.Text += ")";
            compensation++;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('*');
                getConjuntos();
                InfoBox.Text += " * ";
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (EntradaBox1.Text.Length > 0 || conjList.ElementAt(conjList.Count - 1).ElementAt(0).Equals(")"))
            {
                operaciones.Add('+');
                getConjuntos();
                InfoBox.Text += " + ";
            }
        }

        private void getConjuntos()
        {
            if (EntradaBox1.Text.Length > 0) {
                /*String[] conjuntos = EntradaBox1.Lines;
                int count = conjuntos.Length;
                while (count > 0 && EntradaBox1.Lines[count - 1].Trim(' ', '\t') == "") {
                    count--;
                }
                for (int i = 0; i < count; i++) {
                    conjList.Add(conjuntos[i].Split(' ', ',').ToHashSet());
                }*/
                conjList.Add(EntradaBox1.Text.Split(' ', ',').ToHashSet());
                //conjList.Add(EntradaBox1.Text.Split(';').ToHashSet());
                InfoBox.Text += EntradaBox1.Text;
                EntradaBox1.Clear();
            }
            
        }



        private HashSet<String> operate()
        {
            if(EntradaBox1.Text.Length>0)getConjuntos();
            setReturn.Clear();
            HashSet<String> aux = new HashSet<String>();
            int prepare = 0;
            int j = 0;
            int i = 0;
            int limit = conjList.Count;
            do
            {

                if (conjList.ElementAt(i).ElementAt(0).Equals(")"))
                {
                    conjList.RemoveAt(i);
                    conjList.Insert(i, setReturn);
                    i = 0;
                    j = returnParen.Pop();
                    setReturn = auxs.Pop();

                }
                else if (conjList.ElementAt(i).ElementAt(0).Equals("("))
                {
                    conjList.RemoveAt(i);
                    auxs.Push(setReturn);
                    returnParen.Push(j);
                    setReturn = conjList.ElementAt(i);
                    if (conjList.ElementAt(i).ElementAt(0).Equals("(")) { continue; }
                    conjList.RemoveAt(i);
                    j++;
                }
                else
                {
                    if (setReturn.Count > 0)
                    {
                        switch (operaciones.ElementAt(j))
                        {
                            case '⋃':
                                setReturn.UnionWith(conjList.ElementAt(i));
                                break;
                            case '⋂':
                                setReturn.IntersectWith(conjList.ElementAt(i));
                                break;
                            case '-':
                                setReturn.ExceptWith(conjList.ElementAt(i));
                                break;
                            case '∆':
                                setReturn.SymmetricExceptWith(conjList.ElementAt(i));
                                break;
                            case 'ᶜ':
                                HashSet<string> set = GetUniverso();
                                set.ExceptWith(setReturn);
                                setReturn = set;
                                break;
                            case 'x':
                                setReturn.Join(conjList.ElementAt(i), x => true, y => true, (m, n) => new { m, n });
                                break;
                            case '*':
                                kleeneMethod(3);
                                break;
                            case '+':
                                kleeneMethod(4);
                                break;
                            default:
                                InfoBox.Text = "Error";
                                break;
                        }
                        //j++;
                        operaciones.RemoveAt(j);
                    }
                    if (setReturn.Count == 0)
                    {
                        setReturn = conjList.ElementAt(0);

                    }
                    conjList.RemoveAt(i);
                    //i++;
                }
                limit = conjList.Count;
                System.Diagnostics.Debug.WriteLine(returnParen.Count);
            }
            while (conjList.Count > 0);

            foreach (HashSet<String> conj in conjList)
            {
                System.Diagnostics.Debug.WriteLine(String.Join(", ", conj));
            }

            return setReturn;
        }

        private HashSet<String> kleeneClosure(HashSet<String> kleene, int i, int j)
        {
            List<String> KC = new List<String>();
            for (int o = 0; o < j; o++)
            {
                for (int p = 0; p < i; p++)
                {
                    KC.Add(String.Concat(setReturn.ElementAt(o), kleene.ElementAt(p)));
                }
            }
            return KC.ToHashSet();
        }

        private void kleeneMethod(int iteracion)
        {
            HashSet<string> kleene = new HashSet<string>();
            List<string> kleeneAux = new List<string>();
            kleene = setReturn;
            int kleeneLenght = setReturn.Count;
            int g, kp;
            for (g = 1; g < iteracion; g++)
            {
                kp = (int)Math.Pow(kleeneLenght, g);

                kleeneAux.AddRange(kleeneClosure(kleene, kp, kleeneLenght));
                //kleeneAux.InsertRange(0, setReturn);
                kleene = kleeneAux.ToHashSet();
            }

            if (iteracion <= 3)
            {
                setReturn.Prepend("E");
                kleeneAux.InsertRange(0, setReturn);
            }
            setReturn = kleeneAux.ToHashSet();
        }

        private static bool comprobarLenguaje(String[] valores, string alfabeto)
        {
            for (int i = 0; i < valores.Length; i++)
            {
                char[] cs = valores[i].ToCharArray();
                foreach (char c in cs)
                {
                    if (!alfabeto.Contains(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private HashSet<string> GetUniverso()
        {
            string universoString = "";
            foreach (HashSet<string> s in conjList)
            {
                universoString = String.Join(" ", s);
            }
            return universoString.Split(" ").ToHashSet();
        }

        

        //private HashSet<string> GetAlfabeto()
        //{
        //    string alfabetoString = "";
        //    foreach (HashSet<string> s in conjList)
        //    {
        //        alfabetoString = String.Join(" ", s);
        //    }
        //    return alfabetoString.Split(" ").ToHashSet();
        //}


    }
}