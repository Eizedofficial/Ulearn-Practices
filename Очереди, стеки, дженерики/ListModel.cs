using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        public LimitedSizeStack<UserAction<TItem>> Stack;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            Stack = new LimitedSizeStack<UserAction<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            var action = new UserAction<TItem>("add", Items.Count, item);
            Stack.Push(action);
            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            if (Items.Count == 0) return;
            var action = new UserAction<TItem>("remove", index, Items[index]);
            Stack.Push(action);
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            var size = Stack.Count;
            return size != 0 && size < Limit;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var action = Stack.Pop();
                if (action.Event == "remove")
                    Items.Insert(action.Index, action.Value);
                else if(action.Event == "add")
                    Items.RemoveAt(action.Index);
            }
        }
    }

    public class UserAction<TItem>
    {
        public string Event;
        public int Index;
        public TItem Value;
        public UserAction(string action, int index, TItem value)
        {
            Event = action;
            Value = value;
            Index = index;
        }
    }
}
