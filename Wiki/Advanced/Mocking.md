Mocking
----

The library works well with [Moq](https://github.com/moq/moq) as far as I have tested. You can use mocks in all test methods without issue.

You can add Moq to your project using:
```
dotnet add package Moq --version <LATEST VERSION>
```

For now mocking scenes is impossible due to how nodes are implemented in C#. Im planning to do some roundaround version of mocking scenes to achieve the same effect.