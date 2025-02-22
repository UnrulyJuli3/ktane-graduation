using System;

public struct Major
{
    public string Name { get; }

    public string[] Courses { get; }

    public Major(string name, params string[] courses)
    {
        if (courses.Length != 3)
            throw new ArgumentException("You must provide exactly 3 courses!", nameof(courses));

        Name = name;
        Courses = courses;
    }
}