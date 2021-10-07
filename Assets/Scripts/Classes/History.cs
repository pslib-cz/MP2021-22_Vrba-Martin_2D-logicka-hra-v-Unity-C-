using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class History<T>
{
    public int Maximum { get; set; }

    List<T> memory;

    //List containing limited amount of items of type T
    //after having Maximum items, the list will forget first added 


    public History(int maximum)
    {

        Maximum = maximum;
    }

    public void Add(T item)
    {
        memory.Add(item);
		if (memory.Count > Maximum)
		{

		}
    }

    public T Pop()
    {
        return Remove(memory.Count - 1);
    }

    //remove first added item
    public T Forget()
    {
        T output = memory[0];
        memory.RemoveAt(0);
        return output;
    }

    public T Remove(int index)
    {
        T output = memory[0];
        memory.RemoveAt(0);
        return output;
    }


    public T Peek()
    {
        //if there isnt index provided, peek will return last item
        return Peek(memory.Count - 1);
    }

    public T Peek(int index)
    {
        //if the index is within memory's range, return it
		if (0 <= index && index < memory.Count)
		{
            return memory[index];
		}
		else
		{
            throw new System.Exception("Outside the range!");
		}
    }

}