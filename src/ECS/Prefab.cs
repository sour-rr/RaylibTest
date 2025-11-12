using System;
using Raylib_cs;

public static class Prefab
{
    public static Object CreateObject<T>(T type) where T : Mesh
    {
        Object new_object = new Object();
        //Create the mesh component 
        T mesh = type;
        new_object.AddComponent<T>(mesh);

        //Attach a Transform and Color component to the Object
        new_object.AddComponent<Transform>(new Transform());
        new_object.AddComponent<Colour>(new Colour());
        new_object.GetComponent<Colour>().UpdateColour(Color.Red);
        
        return new_object;
    }
}