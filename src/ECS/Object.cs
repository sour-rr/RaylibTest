using System;

public class Object
{
    public Dictionary<Type, Component> components = new Dictionary<Type, Component>();
    
    //Add components to the dict 
    public void AddComponent<T>(T component) where T : Component
    {
        var type = typeof(T);

        //Don;t add duplicate components to the game object
        if(components.ContainsKey(type))
        {   
            throw new Exception($"Component of type {type} already exists on the object");
        }

        components[type] = component;
        component.Attach(this);
    }

    //Get components from the dict based on the type
    public T GetComponent<T>() where T : Component
    {
        //get the comp from the dict 
        //components.TryGetValue(typeof(T), out var component);
        foreach (var comp in components.Values)
        {
            if (comp is T tComp)
            {
                return tComp;
            }
        }
        return default;
    }

    public void Update()
    {
        foreach(var component in components.Values)
            component.Update();
    }

}