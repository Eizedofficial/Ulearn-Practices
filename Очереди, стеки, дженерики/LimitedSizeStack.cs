using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        public Dictionary<int,T> Stack;
        public int Limit;
        public int Head;
        public int Tail;
        public LimitedSizeStack(int limit)
        {
            Stack = new Dictionary<int, T>();
            Limit = limit;
            Head = 0;
            Tail = 0;
        }

        public void Push (T item)
        {
            if (Limit == 0) return;
            if(Stack.Count() == Limit)
            {
                Stack.Remove(Tail);
                Tail++;
            }
            Stack.Add(Head, item);
            Head++;
        }

        public T Pop()
        {
            var output = Stack[Head- 1];
            Stack.Remove(Head - 1);
            Head--;
            return output;
        }

        public int Count
        {
            get
            {
                return Stack.Count();
            }
        }
    }
}
