using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringMachine;

namespace TuringMachine
{
    public class TuringMachine
    {
        private List<char>[] banda;
        private int pozBanda1, pozBanda2;
        
        enum States { Idle, MustWrite, MustMove};
        private States currentState;


        public const char lambda = '~';

        public TuringMachine()
        {
            banda = new List<char>[2];
            banda[0] = new List<char>();
            banda[1] = new List<char>();

            pozBanda1 = 0;
            pozBanda2 = 0;

            currentState = States.Idle;
        }

        public void erase(bool eraseBoth = true, int whichOne = 1)
        {
            if (currentState != States.Idle)
            {
                return;
            }

            banda[whichOne].Clear();
            if (eraseBoth)
                banda[1 - whichOne].Clear();
        }

        public void load(int index, string content)
        {
            if (currentState != States.Idle)
                return;

            banda[index].Clear();

            if (index == 0)
                pozBanda1 = 0;
            else
                pozBanda2 = 0;

            for (int i = 0; i < content.Count(); i++)
            {
                banda[index].Add(content[i]);
            }
        }

        public void load(string content1, string content2)
        {
            load(0, content1);
            load(1, content2);
        }

        private string removelambdas(string str)
        {
            int beg = -1, end = -1;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == lambda)
                    beg = i ;

                if (str[i] != lambda)
                    break;
            }

            if(beg != -1)
                str = str.Substring(beg + 1);

            for(int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] == lambda)
                    end = i;
                if (str[i] != lambda)
                    break;
            }

            if(end != -1)
                str = str.Substring(0, end);

            return str;
        }

        public Tuple<string, string> getInfo()
        {
            if (currentState != States.Idle)
                return new Tuple<string, string>("", "");

            string c1 = "", c2 = "";

            for (int i = 0; i < banda[0].Count; i++)
                c1 += banda[0][i];
            for (int i = 0; i < banda[1].Count; i++)
                c2 += banda[1][i];

            c1 = removelambdas(c1);
            c2 = removelambdas(c2);

            return new Tuple<string, string>(c1, c2);
        }

        public Tuple<char, char> read()
        {
            if (currentState != States.Idle)
                return new Tuple<char, char>('E', 'E');

            char r1 = ' ', r2 = ' ';

            if (pozBanda1 < 0 || pozBanda1 >= banda[0].Count)
                r1 = lambda;
            if (pozBanda2 < 0 || pozBanda2 >= banda[1].Count)
                r2 = lambda;

            if (r1 != lambda)
                r1 = banda[0][pozBanda1];
            if (r2 != lambda)
                r2 = banda[1][pozBanda2];

            currentState = States.MustWrite;

            return new Tuple<char, char>(r1, r2);
        }

        public void write(char c0, char c1)
        {

            if (currentState != States.MustWrite)
                return;

            if (pozBanda1 < 0)
            {
                while (pozBanda1 < 0)
                {
                    banda[0].Insert(0, lambda);
                    pozBanda1++;
                }
            }

            if(pozBanda1 >= banda[0].Count)
            {
                while(banda[0].Count <= pozBanda1)
                {
                    banda[0].Add(lambda);
                }
            }

            if (pozBanda2 < 0)
            {
                while (pozBanda2 < 0)
                {
                    banda[1].Insert(0, lambda);
                    pozBanda2++;
                }
            }

            if (pozBanda2 >= banda[1].Count)
            {
                while (banda[1].Count <= pozBanda2)
                {
                    banda[1].Add(lambda);
                }
            }

            banda[0][pozBanda1] = c0;
            banda[1][pozBanda2] = c1;

            currentState = States.MustMove;
        }

        public void write(Tuple<char, char> input)
        {
            write(input.Item1, input.Item2);
        }

        public void move(int direction0, int direction1)
        {
            if (currentState != States.MustMove)
                return;

            if (direction0 < 0)
                pozBanda1--;
            else if (direction0 > 0)
                pozBanda1++;

            if (direction1 < 0)
                pozBanda2--;
            else if (direction1 > 0)
                pozBanda2++;

            currentState = States.Idle;

        }

        public void moveToStart(int index = -1)
        {
            if(currentState != States.Idle)
                return;

            if (index == -1)
            {
                pozBanda1 = 0;
                pozBanda2 = 0;
            }

            else if (index == 0)
                pozBanda1 = 0;
            else
                pozBanda2 = 0;
        }

        public void moveToEnd(int index = -1)
        {
            if (currentState != States.Idle)
                return;

            if (index == -1)
            {
                pozBanda1 = banda[0].Count - 1;
                pozBanda2 = banda[1].Count - 1;
            }

            else if (index == 0)
                pozBanda1 = banda[0].Count - 1;
            else
                pozBanda2 = banda[0].Count - 1;
        }
    }
}
