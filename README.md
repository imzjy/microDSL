microDSL
========

DSL(Domain Specific Language) will make life easier. 

Sometimes, we want to create micro DSL for internal use.
This DSL is not that complete, just make life easier.

In micro DSL, we define the name convention and parse it with regular expression.

In CSharp example, we want to create a configuration file which can define rules that how to draw something on graphic devices.
Something like:
>Rect(10,10,30,40)   
>Line(40,40,100, 0)
>Text("你好","黑体",20,10,20);
It's just like a function call wrapper, but it friendly to users, more than this, we can change the configuration settings dynamically.

