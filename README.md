# calc-lang

A custom language!

```
function foo(a: Int32, b: Int32): Int32 {
    a = a * 2;
    return pow(a, b);
}

function addAll(all: ...Int32): Int32 {
    for (i, x) in all {
        all[i] = foo(x, i)
    }
    return sum(all)
}

1 + ( foo(10, 11) + 11 ) * addAll(1, 2, 3, 4)
```


## progress

[x] parse identifiers
[x] parse numeric expression
[ ] parse expressions calling builtin functions