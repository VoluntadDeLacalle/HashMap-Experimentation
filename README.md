# AI Data Structure - Associative Arrays 

## Overview

The data structure known generally as the associative array is simple to implement and can be very powerful. While it may come off as a daunting data structure to learn initially, once the plunge has been taken, a whole new world of opportunities will shine. The following document covers the implementation of an associative array written in C# with two examples provided through the Unity game engine.

## How does it work?

To implement the data structure, one must first understand how the data structure functions. For starters, this data structure, when implemented, generally deals with two generic parameters, a key parameter and a value parameter; these will be explained in greater detail later. For this implementation, I followed the work of Abdullah Ozturk in a blog he wrote titled “Simple Hash Map (Hash Table) Implementation in C++.” Abdullah’s take on the implementation breaks the data structure down into two classes and a struct. Since I chose to write this implementation in C# for Unity, I changed the struct into a class for easier functionality in the language.
	
The first class is called HashNode, this class deals with each index in the associative array and holds three primary variables, the key and value of the current HashNode, and a reference to the next HashNode in the array.
	
    public ulong hashFunction(K key, int TABLE_SIZE)
    {
        return (ulong)(Mathf.Abs(key.GetHashCode()) % TABLE_SIZE);
    }

The second class is called KeyHash and only has one function, the hashing function. This is the bread and butter of any associative array implementation. The hashing function takes in the key parameter and returns the index, in this case an unsigned long, used to place value associated with this key in the associative array. Hashing can be a little complicated because it can lead to two completely different variables having the same index in your array, thankfully, C# has a built-in hashing function called GetHashCode that does most of the hard work.

The third and final class is, what I called HashMap. This class incorporates the other two classes to create a table (array) of the different HashNodes that have been passed into it. This class generally has 4 functions associated with it, Get, Put/Add, Find, and Remove. Get takes in a key and returns the value associated with that key, Put/Add takes in a key and value and creates a new HashNode for an object that is not in the associated array (or it does nothing if the key is already in the array, Find takes in a key and returns a true or false value depending on if the key is found in the array, and Remove takes in a key and deletes the related HashNode from the array.

    private HashMap<string, double> listOfStudents = 
    new HashMap<string, double>(numbOfStudents);


Something to keep in mind, my implementation takes in a constructor for the size of the associative array. Within predefined library implementations such as the Dictionary in C# or the STL map in C++, the size of the table is handled for you and scales properly.

## How is this data structure useful?

Associative arrays are **extremely** useful, for many, many things. There are many times in code and in life where something is ultimately associated with another. Students are given ID numbers, the amount of money in a bank account is related to the account number, and the price of any retail item is associated with the retail item itself. The time complexity for associative array libraries normally average to O(1), or constant time. For all of the examples above, it is much more effect and less time consuming to find a value through direct relations rather than, say, a for-loop that runs through the entire list with an average time complexity of O(n), or linear time where n is the number of items in the list.

	    Ex. Let’s say we had a list of students in a school system. Each student has a GPA associated with them, how we calculate the GPA is not important for this problem. One of the better solutions for storing and finding said students is to, of course, use an associative array! We could set the key as a string of the student’s name itself, and the value for said name could be a double which represents the GPA.

## Reference Materials and Other Sources

"Simple Hash Map (Hash Table) Implementation in C++" by Abdullah Ozturk.

    https://medium.com/@aozturk/simple-hash-map-hash-table-implementation-in-c-931965904250

"How to Add Voice Recognition to Your Game - Unity Tutorial" by Youtube user Dapper Dino.

    https://www.youtube.com/watch?v=29vyEOgsW8s&t=748s

C# - Dictionary<TKey, TValue> by TutorialsTeacher.

    https://www.tutorialsteacher.com/csharp/csharp-dictionary